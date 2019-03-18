using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Euler.DataStructures.Stack.RPNCalculator
{
    public abstract class Calculator
    {
        public abstract double Evaluate(List<string> expression);

        public static Calculator Create(CalculatorType calculatorType)
        {
            switch (calculatorType)
            {
                case CalculatorType.Postfix:
                    return new PostfixCalculator();
                default:
                    throw new ArgumentException(nameof(calculatorType));
            }
        }

        public static string FormatExpression(string expression)
        {
            var sanitized = expression.Replace(" ", string.Empty);
            return Regex.Replace(sanitized, @"([\/*+\-])", " $1 ");
        }

        public static string ConvertToString(IEnumerable<string> items)
        {
            var text = new StringBuilder();

            foreach (var token in items)
                text.Append(token).Append(" ");

            return text.ToString();
        }
    }
}