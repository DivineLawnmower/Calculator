namespace Calculator.Services.Validation.Classes
{
    public interface IExpressionValidationRule
    {
        ValidationResponse Validate(string expression);
    }
}
