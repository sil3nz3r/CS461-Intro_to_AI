using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_3Bucket
{
    class State : IEquatable<State>
    {
        public State()
        {
        }
        public State(Bucket smallBucket, Bucket mediumBucket, Bucket largeBucket, bool isFinalState)
        {
            SmallBucket = smallBucket;
            MediumBucket = mediumBucket;
            LargeBucket = largeBucket;
            IsFinalState = isFinalState;
        }

        public Bucket SmallBucket { get; set; }
        public Bucket MediumBucket { get; set; }
        public Bucket LargeBucket { get; set; }
        public bool IsFinalState { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            State stateObj = obj as State;
            if (stateObj == null)
            {
                return false;
            }
            else
            {
                return this.Equals(obj as State);
            }
        }

        public bool Equals(State otherState)
        {
            if (otherState == null)
            {
                return false;
            }

            //Iif this or the other state is a final state,
            // only compare the small bucket amount
            if (this.IsFinalState || otherState.IsFinalState)
            {
                return this.SmallBucket.AmountOfWater == otherState.SmallBucket.AmountOfWater;
            }
            else
            {
                bool areSmallBucketsEqual = this.SmallBucket.AmountOfWater == otherState.SmallBucket.AmountOfWater;
                bool areMediumBucketsEqual = this.MediumBucket.AmountOfWater == otherState.MediumBucket.AmountOfWater;
                bool areLargeBucketsEqual = this.LargeBucket.AmountOfWater == otherState.LargeBucket.AmountOfWater;
                return areLargeBucketsEqual && areMediumBucketsEqual && areLargeBucketsEqual;
            }
        }
    }
}
