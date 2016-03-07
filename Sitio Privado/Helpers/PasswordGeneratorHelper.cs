using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Helpers
{
    public class PasswordGeneratorHelper
    {
        private const int MinLength = 8;
        private const int MaxLength = 16;
        private static Random RandomGenerator;

        public static string GeneratePassword()
        {
            if(RandomGenerator == null)
                RandomGenerator = new Random();

            int size = RandomGenerator.Next(MinLength, MaxLength);

            string[] order = new string[size];

            GenerateOrder(order);

            return GeneratePassword(order);
        }

        private static string GeneratePassword(string[] order)
        {
            string password = "";

            for (int i = 0; i < order.Length; i++)
            {
                if(order[i] == "C")
                {
                    password += GetRandomChar("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                }
                else if(order[i] == "c")
                {
                    password += GetRandomChar("abcdefghijklmnopqrstuvwxyz");
                }
                else
                {
                    password += GetRandomChar("0123456789");
                }
            }

            return password;
        }

        private static char GetRandomChar(string input)
        {
            return input[RandomGenerator.Next(0, input.Length)];
        }

        private static void GenerateOrder(string[] order)
        {
            int limit = order.Length / 3;

            for(int i = 0; i < order.Length; i++)
            {
                int position;
                do
                {
                    position = RandomGenerator.Next(0, order.Length);
                }
                while (order[position] != null);
                 
                if(i < limit)
                {
                    order[position] = "C";
                }
                else if(i >= limit && i < 2 * limit)
                {
                    order[position] = "c";
                }
                else
                {
                    order[position] = "n";
                }
            }
        }
    }
}