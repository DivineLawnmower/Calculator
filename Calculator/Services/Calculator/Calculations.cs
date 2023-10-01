using static Calculator.Services.Calculator.Calculations;

namespace Calculator.Services.Calculator
{
    public class Calculations
    {
        public interface IOperation
        {
            int Precedence { get; }
            string Token { get; }
            double Calculate(double a, double b);
        }
        public class Modulus : IOperation
        {
            public int Precedence { get { return 2; } }
            public string Token { get { return "%"; } }
            public double Calculate(double a, double b)
            {
                return a % b;
            }
        }
        public class Order : IOperation
        {
            public int Precedence { get { return 3; } }
            public string Token { get { return "^"; } }
            public double Calculate(double a, double b)
            {
                return Math.Pow(a, b);
            }
        }
        public class Addition : IOperation
        {
            public int Precedence { get { return 1; } }
            public string Token { get { return "+"; } }
            public double Calculate(double a, double b)
            {
                return a + b;
            }
        }

        public class Subtraction : IOperation
        {
            public int Precedence { get { return 1; } }
            public string Token { get { return "-"; } }
            public double Calculate(double a, double b)
            {
                return a - b;
            }
        }

        public class Multiplication : IOperation
        {
            public int Precedence { get { return 2; } }
            public string Token { get { return "*"; } }
            public double Calculate(double a, double b)
            {
                return a * b;
            }
        }

        public class Division : IOperation
        {
            public int Precedence { get { return 2; } }
            public string Token { get { return "/"; } }

            public double Calculate(double a, double b)
            {
                if (b == 0)
                    throw new ArgumentException("Division by zero is not allowed.");
                return a / b;
            }
        }

        public class CalculatorImplementation
        {
            private readonly IOperation _operation;
            private readonly Dictionary<string, IOperation> _operations;
            public CalculatorImplementation(Dictionary<string, IOperation> operations)
            {
                //_operation = operation ?? throw new ArgumentNullException(nameof(operation));
                _operations = operations;
            }
            public double EvaluateRPN(List<string> rpn)
            {
                var stack = new Stack<double>();

                foreach (var token in rpn)
                {
                    if (double.TryParse(token, out double number))
                    {
                        stack.Push(number);
                    }
                    else if (_operations.ContainsKey(token))
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression");
                        }

                        double rightOperand = stack.Pop();
                        double leftOperand = stack.Pop();

                        double result = _operations[token].Calculate(leftOperand, rightOperand);
                        stack.Push(result);
                    }
                }

                if (stack.Count != 1)
                {
                    throw new ArgumentException("Invalid expression");
                }

                return stack.Pop();
            }
            public double PerformOperation(double a, double b)
            {
                return _operation.Calculate(a, b);
            }
        }
    }
}
