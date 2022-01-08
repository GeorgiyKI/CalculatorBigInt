using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorTask
{
    public static class Extension
    {
        public static int ToDigit(this char letter)
        {
            return letter - 48;
        }
        public static int[] ToDigitArray(this string text)
        {
            int[] mas = new int[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsDigit(text[i]))
                {
                    mas[i] = text[i].ToDigit();
                }
                else
                {
                    throw new ArgumentException("input is not a number");
                }
           
            }

            return mas;
        }
    }
}
