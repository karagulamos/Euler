using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Euler.DataStructures.Stack.RPNCalculator
{
    public class InfixToPostfixConverter
    {
        private const string LeftBracket = "(", RightBracket = ")";

        private readonly Dictionary<string, int> _map;

        public InfixToPostfixConverter()
        {
            _map = new Dictionary<string, int>
            {
                {"^", 4},
                {"*", 3}, {"/", 3},
                {"+", 2}, {"-", 2},
                {"(", 1}
            };
        }

        public List<string> Convert(string infixExpression)
        {
            var operators = new Stack<string>();
            var postfix = new List<string>();

            foreach (var token in ParseTokens(infixExpression))
            {
                if (IsNumber(token))
                {
                    postfix.Add(token);
                    continue;
                }

                if (token == LeftBracket)
                {
                    operators.Push(token);
                    continue;
                }

                if (token == RightBracket)
                {
                    var op = operators.Pop();

                    while (operators.Count > 0 && op != LeftBracket)
                    {
                        postfix.Add(op);
                        op = operators.Pop();
                    }

                    continue;
                }

                while (operators.Count > 0 && _map[token] < _map[operators.Peek()])
                {
                    postfix.Add(operators.Pop());
                }

                operators.Push(token);
            }

            while (operators.Count > 0)
                postfix.Add(operators.Pop());

            return postfix;
        }

        private static IEnumerable<string> ParseTokens(string expression)
        {
            var sanitized = expression.Replace(@" ", string.Empty);
            return Regex.Split(sanitized, @"([\/*+\-()^])")
                        .Where(t => !string.IsNullOrEmpty(t));
        }

        private static bool IsNumber(string token)
        {
            return Regex.IsMatch(token, @"\d+");
        }
    }
}
