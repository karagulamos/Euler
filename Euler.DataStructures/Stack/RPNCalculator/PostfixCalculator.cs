using System;
using System.Collections.Generic;

namespace Euler.DataStructures.Stack.RPNCalculator
{
    public class PostfixCalculator : Calculator
    {
        public override double Evaluate(List<string> expression)
        {
            var operands = new Stack<double>();

            foreach (var token in expression)
            {
                switch (token)
                {
                    case "^":
                        var b = operands.Pop();
                        var a = operands.Pop();
                        operands.Push(Math.Pow(a, b));
                        break;
                    case "*":
                        operands.Push(operands.Pop() * operands.Pop());
                        break;
                    case "/":
                        b = operands.Pop();
                        a = operands.Pop();
                        operands.Push(a / b);
                        break;
                    case "+":
                        operands.Push(operands.Pop() + operands.Pop());
                        break;
                    case "-":
                        b = operands.Pop();
                        a = operands.Pop();
                        operands.Push(a - b);
                        break;
                    default:
                        operands.Push(Convert.ToDouble(token));
                        break;
                }
            }

            return operands.Pop();
        }
    }
}