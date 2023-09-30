using Calculator.Services.Validation.Classes;

namespace Calculator.Services.Validation
{
    public interface IExpressionValidator
    {
        public ValidationResponse Validate(string expression);
        public void AddRule(IExpressionValidationRule rule);
        public void SetRules(IList<IExpressionValidationRule> rules);
        public void RemoveRules();
        public void RemoveRule(IExpressionValidationRule rule);
    }
}
