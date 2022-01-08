using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorTask
{
    public static class Calculator
    {
        private enum Operation
        {
            Add,
            Subtract,
            Multiply
        }

        static void CheckForException(string number)
        {
            if (number[0] == '-')
            {
                throw new ArgumentException("number can't be negative", (number));
            }
        }

        public static string Multiply(string firstNumber, string secondNumber)
        {

            CheckForException(firstNumber);
            CheckForException(secondNumber);
            return DoOperationWithBigInt(firstNumber, secondNumber, Operation.Multiply);
        }

        public static string Plus(string firstNumber, string secondNumber)
        {

            CheckForException(firstNumber);
            CheckForException(secondNumber);
            return DoOperationWithBigInt(firstNumber, secondNumber, Operation.Add);
        }
        public static string Subtract(string firstNumber, string secondNumber)
        {

            CheckForException(firstNumber);
            CheckForException(secondNumber);
            return DoOperationWithBigInt(firstNumber, secondNumber, Operation.Subtract);
        }
        private static string DoOperationWithBigInt(string firstNumber, string secondNumber, Operation op)
        {
            var text = new StringBuilder();
            string smallerNumber = secondNumber;
            string biggerNumber = firstNumber;
            bool firstMoreThanSecond = FirstMoreThanSecond(smallerNumber, biggerNumber);

            if (firstMoreThanSecond)
            {
                smallerNumber = firstNumber;
                biggerNumber = secondNumber;
            }

            int decimalPart = 0;
            int resultOfOperation = 0;

            if (Operation.Multiply == op) text.Append("0");
            int[] masSmall = smallerNumber.ToDigitArray();
            int[] masBig = biggerNumber.ToDigitArray();

            for (int i = 1; i <= smallerNumber.Length; i++)
            {
                if (op == Operation.Multiply)
                {
                    var textForMultiply = new StringBuilder();
                    textForMultiply.Append('0', i - 1);
                    
                    for (int j = 1; j <= masBig.Length; j++)
                    {
                        int sum = masSmall[^i] * masBig[^j] + decimalPart;
                        if (sum >= 10)
                        {
                            decimalPart = (sum - (sum % 10)) / 10;
                            sum %= 10;
                        }
                        else
                        {
                            decimalPart = 0;
                        }

                        textForMultiply.Insert(0, sum);
                    }

                    if (decimalPart != 0) textForMultiply.Insert(0, decimalPart);

                    string result = DoOperationWithBigInt(text.ToString(), textForMultiply.ToString(), Operation.Add);
                    text = new StringBuilder(result);
                    decimalPart = 0;
                }
                else
                {
                    resultOfOperation = DoOperation(masBig[^i], masSmall[^i], op) + decimalPart;

                    if (resultOfOperation >= 10 || resultOfOperation < 0)
                    {
                        decimalPart = (op == Operation.Add) ? 1 : -1;
                        resultOfOperation = (op == Operation.Add) ? resultOfOperation % 10 : resultOfOperation + 10;
                    }
                    else
                    {
                        decimalPart = 0;
                    }

                    text.Insert(0, resultOfOperation);
                }
            }

            if (decimalPart != 0)
            {
                for (int i = smallerNumber.Length + 1; i <= biggerNumber.Length; i++)
                {
                    resultOfOperation = masBig[^i] + decimalPart;

                    if (resultOfOperation <= 10 && resultOfOperation >= 0)
                    {
                        decimalPart = 0;
                        text.Insert(0, resultOfOperation);
                        text.Insert(0, biggerNumber[0..(biggerNumber.Length - i)]);
                        break;
                    }

                    decimalPart = (op == Operation.Add) ? 1 : -1;
                    resultOfOperation = (op == Operation.Add) ? resultOfOperation % 10 : resultOfOperation + 10;
                    text.Insert(0, resultOfOperation);
                }

                if (decimalPart != 0) text.Insert(0, decimalPart);
            }
            else
            {
                if (Operation.Multiply != op)
                {
                    text.Insert(0, biggerNumber[0..(masBig.Length - masSmall.Length)]);
                }
            }

            while (text[0] == '0' && text.Length > 1) text.Remove(0, 1);
            if (Operation.Subtract == op && firstMoreThanSecond && text[0] != '0') text.Insert(0, '-');

            return text.ToString();
        }

        static bool FirstMoreThanSecond(string firstString, string secondString)
        {
            if (firstString.Length > secondString.Length)
            {
                return true;
            }
            else if (firstString.Length < secondString.Length)
            {
                return false;
            }

            for (int i = 0; i < firstString.Length; i++)
            {
                if (firstString[i].ToDigit() > secondString[i].ToDigit())
                {
                    return true;
                }
                else if (firstString[i].ToDigit() < secondString[i].ToDigit())
                {
                    return false;
                }
            }

            return true;
        }
        static int DoOperation(int x, int y, Operation op)
        {
            int result = op switch
            {
                Operation.Add => x + y,
                Operation.Subtract => x - y,
                _ => throw new NotImplementedException(),
            };

            return result;
        }
    }
}
