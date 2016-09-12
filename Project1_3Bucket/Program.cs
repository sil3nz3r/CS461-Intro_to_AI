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

            FindFinalSmallBucket(startingState, finalState);
        }

        static void FindFinalSmallBucket(State startingState, State finalState)
        {
            IList<State> visistedStates = new List<State>();
            Stack<State> solutionStack = new Stack<State>();
            Stack<State> tempStack = new Stack<State>();
            bool isThereASolution = FindFinalSmallBucketWork(startingState, finalState, ref visistedStates, ref solutionStack);

            Console.WriteLine(">>>>>>Done");
            Console.WriteLine("Number of states visited (non-unique): " + visistedStates.Count);

            if (isThereASolution)
            {
                foreach (var state in solutionStack)
                {
                    tempStack.Push(state);
                }

                
                Console.WriteLine("Solution:");
                foreach (var state in tempStack)
                {
                    string smallWaterBucketStr = state.SmallBucket.AmountOfWater + "/" + state.SmallBucket.Size;
                    string mediumWaterBucketStr = state.MediumBucket.AmountOfWater + "/" + state.MediumBucket.Size;
                    string largeWaterBucketStr = state.LargeBucket.AmountOfWater + "/" + state.LargeBucket.Size;
                    Console.WriteLine(smallWaterBucketStr.PadRight(8) + mediumWaterBucketStr.PadRight(8) + largeWaterBucketStr.PadRight(8));
                }
            }
            else
            {
                Console.WriteLine("No solution found!");
            }
            Console.ReadKey();
        }

        static bool FindFinalSmallBucketWork(State currentState, State finalState, ref IList<State> visitedStates, ref Stack<State> solutionStack)
        {
            Stack<State> generatedStack = new Stack<State>();

            // Right now, we assume that currentState is the undiscovered state
            //Console.WriteLine("State: " +
            //    currentState.SmallBucket.AmountOfWater + "/" + currentState.SmallBucket.Size + " " +
            //    currentState.MediumBucket.AmountOfWater + "/" + currentState.MediumBucket.Size + " " +
            //    currentState.LargeBucket.AmountOfWater + "/" + currentState.LargeBucket.Size);

            // Solution found, we stop and return the stack
            if (currentState.Equals(finalState))
            {
                Stack<State> temp = new Stack<State>();
                solutionStack.Push(currentState);
                visitedStates.Add(currentState);
                Console.WriteLine("Solution found!");
                Console.WriteLine("(" + currentState.SmallBucket.AmountOfWater + ", " +
                    currentState.MediumBucket.AmountOfWater + ", " +
                    currentState.LargeBucket.AmountOfWater + ")");
                return true;
            }

            // We have been to this state before, return
            if (visitedStates.Any(state => state == currentState))
            {
                //visitedStates.Add(currentState);
                //Console.WriteLine("Dead end!");
                return false;
            }

            // Add the visited state immediately
            // if we have not visited it before.
            visitedStates.Add(currentState);
            // Potentially a solution
            // Add this into the solution stack anyway
            solutionStack.Push(currentState);

            // Generate states
            // LARGE bucket
            if (currentState.LargeBucket.AmountOfWater == 0)
            {
                // Fill the LARGE bucket if it doesn't have water
                State nextState = currentState.DeepCopy();
                nextState.LargeBucket.AmountOfWater = nextState.LargeBucket.Size;

                // Generate a state
                generatedStack.Push(nextState);
            }
            // The LARGE bucket has water, pour it into other bucket
            if (currentState.LargeBucket.AmountOfWater > 0)
            {
                // Try pouring it into the MEDIUM bucket
                if (currentState.MediumBucket.AvailableVolume > 0)
                {
                    State nextState = currentState.DeepCopy();

                    Bucket pourer = nextState.LargeBucket;
                    Bucket pouree = nextState.MediumBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.LargeBucket = pourer;
                    nextState.MediumBucket = pouree;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
                // Try pouring it into the SMALL bucket
                if (currentState.SmallBucket.AvailableVolume > 0)
                {
                    State nextState = currentState.DeepCopy();

                    Bucket pourer = nextState.LargeBucket;
                    Bucket pouree = nextState.SmallBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.LargeBucket = pourer;
                    nextState.SmallBucket = pouree;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
                // Last case: dump the bucket
                if (true)
                {
                    State nextState = currentState.DeepCopy();

                    nextState.LargeBucket.AmountOfWater = 0;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
            }

            // MEDIUM bucket
            // Fill the MEDIUM bucket if it doesn't have water
            if (currentState.MediumBucket.AmountOfWater == 0)
            {
                State nextState = currentState.DeepCopy();
                nextState.MediumBucket.AmountOfWater = currentState.MediumBucket.Size;

                // Generate a state
                generatedStack.Push(nextState);
            }
            // The MEDIUM bucket has water, pour it into other bucket
            if (currentState.MediumBucket.AmountOfWater > 0)
            {
                // Try pouring it into the SMALL bucket
                if (currentState.SmallBucket.AvailableVolume > 0)
                {
                    State nextState = currentState.DeepCopy();

                    Bucket pourer = nextState.MediumBucket;
                    Bucket pouree = nextState.SmallBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.MediumBucket = pourer;
                    nextState.SmallBucket = pouree;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
                // Try pouring it into the LARGE bucket
                if (currentState.LargeBucket.AvailableVolume > 0)
                {
                    State nextState = currentState.DeepCopy();

                    Bucket pourer = nextState.MediumBucket;
                    Bucket pouree = nextState.LargeBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.MediumBucket = pourer;
                    nextState.LargeBucket = pouree;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
                // Last resort: dump the bucket
                if (true)
                {
                    State nextState = currentState.DeepCopy();

                    nextState.MediumBucket.AmountOfWater = 0;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
            }

            // SMALL bucket
            // Fill the SMALL bucket if it doesn't have water
            if (currentState.SmallBucket.AmountOfWater == 0)
            {
                // Fill the SMALL bucket if it doesn't have water
                State nextState = currentState.DeepCopy();
                nextState.SmallBucket.AmountOfWater = currentState.SmallBucket.Size;

                // Generate a state
                generatedStack.Push(nextState);
            }
            // The SMALL bucket has water, pour it into other bucket
            if (currentState.SmallBucket.AmountOfWater > 0)
            {
                // Try pouring it into the LARGE bucket
                if (currentState.LargeBucket.AvailableVolume > 0)
                {
                    State nextState = currentState.DeepCopy();

                    Bucket pourer = nextState.SmallBucket;
                    Bucket pouree = nextState.LargeBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.SmallBucket = pourer;
                    nextState.LargeBucket = pouree;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
                // Try pouring it into the MEDIUM bucket
                if (currentState.MediumBucket.AvailableVolume > 0)
                {
                    State nextState = currentState.DeepCopy();

                    Bucket pourer = nextState.SmallBucket;
                    Bucket pouree = nextState.MediumBucket;
                    PourWater(ref pourer, ref pouree);
                    nextState.SmallBucket = pourer;
                    nextState.MediumBucket = pouree;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
                // Last resort: dump the bucket
                if (true)
                {
                    State nextState = currentState.DeepCopy();

                    nextState.SmallBucket.AmountOfWater = 0;

                    // Generate a state
                    generatedStack.Push(nextState);
                }
            }

            foreach (var member in generatedStack)
            {
                bool isSolutionFound = FindFinalSmallBucketWork(member, finalState, ref visitedStates, ref solutionStack);
                // Solution found, return
                // Otherwise, keep going
                if (isSolutionFound)
                {
                    return true;
                }
            }

            // Pop this current state out of the solution stack
            // Because this is not a solution state
            solutionStack.Pop();
            // Return because we have not found the solution yet
            //Console.WriteLine("Couldn't find a solution");
            return false;
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
