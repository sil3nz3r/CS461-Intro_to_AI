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
                return areSmallBucketsEqual && areMediumBucketsEqual && areLargeBucketsEqual;
            }
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + SmallBucket.AmountOfWater.GetHashCode();
            hash = (hash * 7) + MediumBucket.AmountOfWater.GetHashCode();
            hash = (hash * 7) + LargeBucket.AmountOfWater.GetHashCode();

            return hash;
        }

        public static bool operator ==(State leftState, State rightState)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(leftState, rightState))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)leftState == null) || ((object)rightState == null))
            {
                return false;
            }

            //Iif this or the other state is a final state,
            // only compare the small bucket amount
            if (leftState.IsFinalState || rightState.IsFinalState)
            {
                return leftState.SmallBucket.AmountOfWater == rightState.SmallBucket.AmountOfWater;
            }
            else
            {
                bool areSmallBucketsEqual = leftState.SmallBucket.AmountOfWater == rightState.SmallBucket.AmountOfWater;
                bool areMediumBucketsEqual = leftState.MediumBucket.AmountOfWater == rightState.MediumBucket.AmountOfWater;
                bool areLargeBucketsEqual = leftState.LargeBucket.AmountOfWater == rightState.LargeBucket.AmountOfWater;
                return areSmallBucketsEqual && areMediumBucketsEqual && areLargeBucketsEqual;
            }
        }

        public static bool operator !=(State leftState, State rightState)
        {
            return !(leftState == rightState);
        }

        public State DeepCopy()
        {
            State other = (State)this.MemberwiseClone();
            other.SmallBucket = new Bucket(this.SmallBucket.Size, this.SmallBucket.AmountOfWater);
            other.MediumBucket = new Bucket(this.MediumBucket.Size, this.MediumBucket.AmountOfWater);
            other.LargeBucket = new Bucket(this.LargeBucket.Size, this.LargeBucket.AmountOfWater);
            other.IsFinalState = this.IsFinalState;
            return other;
        }
    }
}
