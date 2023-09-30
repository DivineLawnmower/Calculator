using Calculator.Services.Evaluation;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.Services.Calculator.Calculations;

namespace CalculatorTests
{
    public class EvaluatorTests
    {
        public static IEnumerable<object[]> EvaluationDataSuccess =>
          new List<object[]>
          {
                new object[] { GetBasicOperations(), "4+5*2", new List<string>() { "4","5", "2","*","+" } },
                new object[] { GetBasicOperations(), "4+5/2", new List<string>() { "4", "5", "2", "/", "+" } },
                new object[] { GetBasicOperations(), "4+5/2-1", new List<string>() { "4", "5", "2","/", "+", "1", "-" }},
          };
        public static Dictionary<string, IOperation> GetBasicOperations()
        {
            return new Dictionary<string, IOperation>
            {
                { "+", new Addition() },
                { "-", new Subtraction() },
                { "*", new Multiplication() },
                { "/", new Division() }
            };
        }

        [Theory]
        [MemberData(nameof(EvaluationDataSuccess))]
        public void EvaluateExpression_ShouldReturnCorrectResult(
        Dictionary<string, IOperation> operations,
        string expression, List<string> shuntYard)
        {

            Evaluator evaluator = new Evaluator();
            evaluator.SetOperations(operations);

            EvaluatorResponse evaluation = evaluator.Evaluate(expression);

            Assert.Equal(evaluation.Result, shuntYard);

        }


    }
}
