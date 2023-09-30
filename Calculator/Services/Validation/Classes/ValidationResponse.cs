namespace Calculator.Services.Validation.Classes
{
    public class ValidationResponse : Response
    {
        public ValidationResponse()
        {
            Success = true;
        }
        public ValidationResponse(string message)
        {
            Success = false;
            Message = message;
        }
    }
}
