using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_3Bucket
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Size of the SMALL bucket? ");
            int smallBucketSize = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Size of the MEDIUM bucket? ");
            int mediumBucketSize = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Size of the LARGE bucket? ");
            int largeBucketSize = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Final amount of water in the SMALL bucket? ");
            int finalAmountInSmall = Convert.ToInt32(Console.ReadLine());

            Bucket smallBucket = new Bucket(smallBucketSize, 0);
            Bucket mediumBucket = new Bucket(mediumBucketSize, 0);
            Bucket largeBucket = new Bucket(largeBucketSize, 0);
            Bucket finalSmallBucket = new Bucket(smallBucketSize, finalAmountInSmall);

            State startingState = new State(smallBucket, mediumBucket, largeBucket, false);
            State finalState = new State(finalSmallBucket, mediumBucket, largeBucket, true);
            //Console.WriteLine("2 states same? " + starterState.Equals(new State(new Bucket(2,0), new Bucket(2,0), new Bucket(2,0), false)));

            FindBucket(startingState, finalState);
        }

        static void FindBucket(State startingState, State finalState)
        {
            // Create the working queue and start at root node
            Queue<State> queue = new Queue<State>();
            queue.Enqueue(startingState);

            State solutionState = null;

            IList<State> visitedStates = new List<State>();
            Stack<State> solutionStack = new Stack<State>();

            while (queue.Count > 0)
            {
                State currentState = queue.Dequeue();
                if (visitedStates.Any(state => state == currentState))
                {
                    //visitedStates.Add(currentState);
                    //Console.WriteLine("Dead end!");
                    continue;
                }
                visitedStates.Add(currentState);
                queue = GenerateMoreStates(currentState, queue);
                if (currentState.Equals(finalState))
                {
                    solutionState = currentState;
                    Console.WriteLine("Solution found!");
                    Console.WriteLine("(" + currentState.SmallBucket.AmountOfWater + ", " +
                        currentState.MediumBucket.AmountOfWater + ", " +
                        currentState.LargeBucket.AmountOfWater + ")");
                    break;
                }
            }

            Console.WriteLine(">>>>>>Done");
            Console.WriteLine("Number of states visited (non-unique): " + visitedStates.Count);

            if (solutionState != null)
            {
                Console.WriteLine("Solution:");
                while (solutionState != null)
                {
                    solutionStack.Push(solutionState);
                    solutionState = solutionState.Parent;
                }
                foreach (var member in solutionStack)
                {
                    string smallWaterBucketStr = member.SmallBucket.AmountOfWater + "/" + member.SmallBucket.Size;
                    string mediumWaterBucketStr = member.MediumBucket.AmountOfWater + "/" + member.MediumBucket.Size;
                    string largeWaterBucketStr = member.LargeBucket.AmountOfWater + "/" + member.LargeBucket.Size;
                    Console.WriteLine(smallWaterBucketStr.PadRight(8) + mediumWaterBucketStr.PadRight(8) + largeWaterBucketStr.PadRight(8));
                }
            }
            else
            {
                Console.WriteLine("No slution found!");
            }
            Console.ReadKey();
        }

        static Queue<State> GenerateMoreStates(State currentNode, Queue<State> currentqueue)
        {
            // Generate states
            // LARGE bucket
            if (currentNode.LargeBucket.AmountOfWater == 0)
            {
                // Fill the LARGE bucket if it doesn't have water
                State nextState = currentNode.DeepCopy();
                nextState.LargeBucket.AmountOfWater = nextState.LargeBucket.Size;

                // Generate a state
                currentqueue.Enqueue(nextState);
                currentNode.InsertChild(nextState);
            }
            // The LARGE bucket has water, pour it into other bucket
            if (currentNode.LargeBucket.AmountOfWater > 0)
            {
                // Try pouring it into the MEDIUM bucket
                if (currentNode.MediumBucket.AvailableVolume > 0)
                {
                    State nextState = currentNode.DeepCopy();

                    Bucket pourer = nextState.LargeBucket;
                    Bucket pouree = nextState.MediumBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.LargeBucket = pourer;
                    nextState.MediumBucket = pouree;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
                // Try pouring it into the SMALL bucket
                if (currentNode.SmallBucket.AvailableVolume > 0)
                {
                    State nextState = currentNode.DeepCopy();

                    Bucket pourer = nextState.LargeBucket;
                    Bucket pouree = nextState.SmallBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.LargeBucket = pourer;
                    nextState.SmallBucket = pouree;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
                // Last case: dump the bucket
                if (true)
                {
                    State nextState = currentNode.DeepCopy();

                    nextState.LargeBucket.AmountOfWater = 0;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
            }

            // MEDIUM bucket
            // Fill the MEDIUM bucket if it doesn't have water
            if (currentNode.MediumBucket.AmountOfWater == 0)
            {
                State nextState = currentNode.DeepCopy();
                nextState.MediumBucket.AmountOfWater = currentNode.MediumBucket.Size;

                // Generate a state
                currentqueue.Enqueue(nextState);
                currentNode.InsertChild(nextState);
            }
            // The MEDIUM bucket has water, pour it into other bucket
            if (currentNode.MediumBucket.AmountOfWater > 0)
            {
                // Try pouring it into the SMALL bucket
                if (currentNode.SmallBucket.AvailableVolume > 0)
                {
                    State nextState = currentNode.DeepCopy();

                    Bucket pourer = nextState.MediumBucket;
                    Bucket pouree = nextState.SmallBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.MediumBucket = pourer;
                    nextState.SmallBucket = pouree;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
                // Try pouring it into the LARGE bucket
                if (currentNode.LargeBucket.AvailableVolume > 0)
                {
                    State nextState = currentNode.DeepCopy();

                    Bucket pourer = nextState.MediumBucket;
                    Bucket pouree = nextState.LargeBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.MediumBucket = pourer;
                    nextState.LargeBucket = pouree;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
                // Last resort: dump the bucket
                if (true)
                {
                    State nextState = currentNode.DeepCopy();

                    nextState.MediumBucket.AmountOfWater = 0;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
            }

            // SMALL bucket
            // Fill the SMALL bucket if it doesn't have water
            if (currentNode.SmallBucket.AmountOfWater == 0)
            {
                // Fill the SMALL bucket if it doesn't have water
                State nextState = currentNode.DeepCopy();
                nextState.SmallBucket.AmountOfWater = currentNode.SmallBucket.Size;

                // Generate a state
                currentqueue.Enqueue(nextState);
                currentNode.InsertChild(nextState);
            }
            // The SMALL bucket has water, pour it into other bucket
            if (currentNode.SmallBucket.AmountOfWater > 0)
            {
                // Try pouring it into the LARGE bucket
                if (currentNode.LargeBucket.AvailableVolume > 0)
                {
                    State nextState = currentNode.DeepCopy();

                    Bucket pourer = nextState.SmallBucket;
                    Bucket pouree = nextState.LargeBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.SmallBucket = pourer;
                    nextState.LargeBucket = pouree;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
                // Try pouring it into the MEDIUM bucket
                if (currentNode.MediumBucket.AvailableVolume > 0)
                {
                    State nextState = currentNode.DeepCopy();

                    Bucket pourer = nextState.SmallBucket;
                    Bucket pouree = nextState.MediumBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.SmallBucket = pourer;
                    nextState.MediumBucket = pouree;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
                // Last resort: dump the bucket
                if (true)
                {
                    State nextState = currentNode.DeepCopy();

                    nextState.SmallBucket.AmountOfWater = 0;

                    // Generate a state
                    currentqueue.Enqueue(nextState);
                    currentNode.InsertChild(nextState);
                }
            }
            return currentqueue;
        }

        public static void PourWater(ref Bucket pourer, ref Bucket pouree)
        {
            if (pouree.AvailableVolume >= pourer.AmountOfWater)
            {
                // Pour everything if there is available space
                pouree.AmountOfWater += pourer.AmountOfWater;
                pourer.AmountOfWater = 0;
            }
            else
            {
                // Pour a portion of the water if there is not enough space
                int amountOfWaterTransfer = pouree.AvailableVolume;
                pouree.AmountOfWater += amountOfWaterTransfer;
                pourer.AmountOfWater -= amountOfWaterTransfer;
            }
        }
    }
}
