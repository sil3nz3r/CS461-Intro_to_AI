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
        public int Size { get; }

        /// <summary>
        /// The current amount of liquid inside the bucket
        /// </summary>
        public int AmountOfWater { get; set; } = 0;
    }
}
