using Calculator.Services.Validation.Classes;

namespace Calculator.Services.Validation
{
    public class ExpressionValidator : IExpressionValidator
    {

        private IList<IExpressionValidationRule> rules;
        public ExpressionValidator() {
            rules = new List<IExpressionValidationRule>();
        }

        public void AddRule(IExpressionValidationRule rule)
        {
            rules.Add(rule);
        }
        public void SetRules(IList<IExpressionValidationRule> rules)
        {
            this.rules = rules;
        }
        public void RemoveRules()
        {
            rules.Clear();
        }
        public void RemoveRule(IExpressionValidationRule rule)
        {
                rules.Remove(rule);
        }


        public ValidationResponse Validate(string expression)
        {
            for (var i = 0; i < rules.Count; i++)
            {
                ValidationResponse valid = rules[i].Validate(expression);
                if (!valid.Success)
                {
                    return valid;
                }
            }

            return new ValidationResponse();
        }
    }
}
