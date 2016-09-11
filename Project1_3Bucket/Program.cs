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
            Bucket smallBucket = new Bucket(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Size of the MEDIUM bucket? ");
            Bucket mediumBucket = new Bucket(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Size of the LARGE bucket? ");
            Bucket largeBucket = new Bucket(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Final amount of water in the SMALL bucket? ");
            int finalAmountInSmall = Convert.ToInt32(Console.ReadLine());

            bool isEqual = smallBucket.Equals(largeBucket);

            //Console.WriteLine("Large bucket = small Bucket? " + ;
        }

        //Queue<State> FindFinalSmallBucket(State finalState)
        //{

        //}

        //Stack<State> FindFinalSmallBucketWork(State currentState, State finalState, ref IList<State> visitedState)
        //{
        //    if (currentState.Equals(finalState));
        //}
    }
}
