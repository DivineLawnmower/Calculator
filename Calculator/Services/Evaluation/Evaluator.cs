using Calculator.Services.Validation;
using Calculator.Services.Validation.Classes;
using System.Text.RegularExpressions;
using static Calculator.Services.Calculator.Calculations;

namespace Calculator.Services.Evaluation
{
    public class Evaluator : IEvaluator
    {

        private Dictionary<string, IOperation> Operations { get; set; }
        public void SetOperations(Dictionary<string, IOperation> operations)
        {
            Operations = operations;
        }
        public void PushOperation(string operationName, IOperation operation)
        {
            Operations[operationName] = operation;
        }
        public void PopOperation(string operationName)
        {
            Operations[operationName] = null;
        }

        public EvaluatorResponse Evaluate(string expression)
        {
            var tokens = Tokenize(expression);
            var precedence = ShuntingYardAlgorithm(tokens);
            return new EvaluatorResponse(precedence);

        }

        private List<string> Tokenize(string expression)
        {
            var matches = Regex.Matches(expression, @"([*+/\-)(])|([0-9]+)");
            return matches.Select(x => x.Value).ToList();
        }

        private List<string> ShuntingYardAlgorithm(List<string> tokens)
        {
            var output = new List<string>();
            var operatorStack = new Stack<IOperation>();

            foreach (var token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    output.Add(token);
                }
                else if (Operations.ContainsKey(token))
                {
                    var op = Operations.FirstOrDefault(x => x.Key == token);

                    if (op.Equals(new KeyValuePair<string, IOperation>()))
                    {
                        throw new KeyNotFoundException(token);
                    }


                    while (operatorStack.Count > 0 &&
                           Operations.ContainsKey(operatorStack.Peek().Token) &&
                           operatorStack.Peek().Precedence >= op.Value.Precedence)
                    {
                        output.Add(operatorStack.Pop().Token);
                    }
                    operatorStack.Push(op.Value);
                }
                else
                {
                    throw new ArgumentException("Invalid token: " + token);
                }
            }

            while (operatorStack.Count > 0)
            {
                output.Add(operatorStack.Pop().Token);
            }

            return output;
        }
    }
}
