using Calculator.Services.Calculator;
using Calculator.Services.Evaluation;
using Calculator.Services.Validation.Classes;
using Calculator.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Calculator.Services.Calculator.Calculations;

namespace CalculatorTests.Setup
{
    public class TestSetup
    {
        public IEnumerable<object[]> CalculatorDataSuccess =>
            new List<object[]>
            {
                new object[] { GetBasicOperations(), "4+5*2", new List<string>() { "4","5", "2","*","+" },  14 },
                new object[] { GetBasicOperations(), "4+5/2", new List<string>() { "4", "5", "2", "/", "+" }, 6.5 },
                new object[] { GetBasicOperations(), "4+5/2-1", new List<string>() { "4", "5", "2","/", "+", "1", "-" }, 5.5 },
            };


        public IEnumerable<object[]> CalculatorDataFailure =>
            new List<object[]>
            {
                new object[] { GetBasicOperations(), "4+5*", new List<string>() { "4","5","*"},  14 },
                new object[] { GetBasicOperations(), "4+5/t+", new List<string>() { "4", "5", "t", "/", "+" }, 6.5 },
                new object[] { GetBasicOperations(), "4+5/0", new List<string>() { "4", "5", "0","/", "+" }, 5.5 },
            };

        public Dictionary<string, IOperation> GetBasicOperations()
        {
            return new Dictionary<string, IOperation>
            {
                { "+", new Addition() },
                { "-", new Subtraction() },
                { "*", new Multiplication() },
                { "/", new Division() }
            };
        }

        public Dictionary<string, IOperation> GetAdvancedOperations()
        {
            var operations = GetBasicOperations();

            return operations;
        }

        public IList<IExpressionValidationRule> SetupValidationRules()
        {
            return new List<IExpressionValidationRule>()
            {
                new ContainsInvalidCharacters()
            };
        }

        public ExpressionValidator SetupValidator()
        {
            ExpressionValidator validator = new ExpressionValidator();
            validator.SetRules(SetupValidationRules());
            return validator;
        }
        public CalculatorService SetupCalcService()
        {
            ExpressionValidator validator = SetupValidator();

            Evaluator evaluator = new Evaluator();
            evaluator.SetOperations(GetBasicOperations());

            return new CalculatorService(validator, evaluator);
        }

    }
}
