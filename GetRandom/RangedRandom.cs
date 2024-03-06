using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetRandom
{
    public static class RangedRandom
    {
        private static Random randomNumberGen = new Random();

        public static uint GenerateUnsignedNumber(uint ceiling, uint entropy)
        {
            if (0 != ceiling)
            {
                double myDouble = randomNumberGen.NextDouble() * ceiling;
                return (uint)myDouble;
            }
            else
            {
                return 0;
            }
        }

        public static uint GenerateUnsignedNumber(uint floor, uint ceiling, uint entropy)
        {
            uint temp = 0;
            if (0 != ceiling)
            {
                do
                {
                    temp = GenerateUnsignedNumber(ceiling, entropy);
                } while (temp < floor);
            }
            return temp;
        }

        public static void PrimeRandomNumberGenerator(uint length = 4, uint entropy = 0)
        {
            randomNumberGen = new Random();

            GenerateUnsignedNumber(length, entropy);
        }
    }
}
