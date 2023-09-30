using Calculator.Controllers;
using Calculator.Services.Evaluation;
using Calculator.Services.Validation;
using Calculator.Services.Validation.Classes;
using static Calculator.Services.Calculator.Calculations;

namespace Calculator.Services.Calculator
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IExpressionValidator _validator;
        private readonly IEvaluator _evaluator;
        private Dictionary<string, IOperation> _operations;
        private IList<IExpressionValidationRule> _rules;
        public CalculatorService(IExpressionValidator validator, IEvaluator evaluator)
        {
            _evaluator = evaluator;
            _validator = validator;
            _operations = new Dictionary<string, IOperation>
            {
                { "+", new Addition() },
                { "-", new Subtraction() },
                { "*", new Multiplication() },
                { "/", new Division() }
            };
            _rules = new List<IExpressionValidationRule>()
            {
                new ContainsInvalidCharacters()
            };

        }
     


        public CalculationResponse Calculate(string expression)
        {
            CalculationResponse result = new CalculationResponse();
            try
            {
                _validator.SetRules(_rules);
                ValidationResponse valid = _validator.Validate(expression);

                if (!valid.Success)
                {
                    result.Message = valid.Message;
                    return result;
                }

                _evaluator.SetOperations(_operations);
                EvaluatorResponse evaulation = _evaluator.Evaluate(expression);

                CalculatorImplementation calc = new CalculatorImplementation(_operations);

                result.Result = calc.EvaluateRPN(evaulation.Result);
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }
    }
}
