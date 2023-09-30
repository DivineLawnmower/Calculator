namespace Calculator.Services.Calculator
{
    public class CalculationResponse : Response
    {
        public CalculationResponse()
        {

        }
        public CalculationResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public CalculationResponse(double result)
        {
            Success = true;
            Result = result;
        }

        public double Result { get; set; }
    }
}
