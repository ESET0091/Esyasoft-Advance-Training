using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramids_Practice
{
    public class PyramidPatterns
    {
        public static void SimpleStarPyramid(int rows)
        {
            Console.WriteLine("Simple Star Pyramid:");
            for (int i = 1; i <= rows; i++)
            {
                // Print spaces
                for (int j = 1; j <= rows - i; j++)
                {
                    Console.Write(" ");
                }

                // Print stars
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        public static void NumberPyramid(int rows)
        {
            Console.WriteLine("Number Pyramid:");
            for (int i = 1; i <= rows; i++)
            {
                // Print spaces
                for (int j = 1; j <= rows - i; j++)
                {
                    Console.Write(" ");
                }

                // Print numbers
                for (int k = 1; k <= i; k++)
                {
                    Console.Write(k + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void InvertedStarPyramid(int rows)
        {
            Console.WriteLine("Inverted Star Pyramid:");
            for (int i = rows; i >= 1; i--)
            {
                // Print spaces
                for (int j = 1; j <= rows - i; j++)
                {
                    Console.Write(" ");
                }

                // Print stars
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void AlphabetPyramid(int rows)
        {
            Console.WriteLine("Alphabet Pyramid:");
            char ch = 'A';

            for (int i = 1; i <= rows; i++)
            {
                // Print spaces
                for (int j = 1; j <= rows - i; j++)
                {
                    Console.Write(" ");
                }

                // Print alphabets
                for (int k = 1; k <= i; k++)
                {
                    Console.Write(ch + " ");
                    ch++;
                }
                Console.WriteLine();
                ch = 'A';
            }
            Console.WriteLine();
        }

        public static void DiamondPattern(int rows)
        {
            Console.WriteLine("Diamond Pattern:");

            // Upper half
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= rows - i; j++)
                {
                    Console.Write(" ");
                }
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }

            // Lower half
            for (int i = rows - 1; i >= 1; i--)
            {
                for (int j = 1; j <= rows - i; j++)
                {
                    Console.Write(" ");
                }
                for (int k = 1; k <= 2 * i - 1; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
