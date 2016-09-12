using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1_3Bucket
{
    class Bucket
    {
        public Bucket(int size, int amountOfWater)
        {
            Size = size;
            AmountOfWater = amountOfWater;
        }

        /// <summary>
        /// Total size of the bucket
        /// </summary>
        /// <param name="size"></param>
        public int Size { get; set; }

        /// <summary>
        /// The current amount of liquid inside the bucket
        /// </summary>
        public int AmountOfWater { get; set; } = 0;

        /// <summary>
        /// The available volume of the bucket
        /// </summary>
        public int AvailableVolume
        {
            get
            {
                return Size - AmountOfWater;
            }
        }

        public Bucket DeepCopy()
        {
            Bucket otherBucket = (Bucket)this.MemberwiseClone();
            otherBucket.Size = this.Size;
            otherBucket.AmountOfWater = this.AmountOfWater;
            return otherBucket;
        }
    }
}
