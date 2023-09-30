namespace Calculator.Services.Validation.Classes
{
    public class ContainsInvalidCharacters : IExpressionValidationRule
    {
       public ValidationResponse Validate(string expression)
        {
            for (var i = 0; i < expression.Length; i++)
            {
                if (char.IsLetter(expression[i]))
                {
                    return new ValidationResponse($"Invalid character at position {i+1}: {expression.Substring(i)}");
                }
            }
            return new ValidationResponse();
        }
    }
}
