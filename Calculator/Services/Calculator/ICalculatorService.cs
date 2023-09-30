namespace Calculator.Services.Calculator
{
    public interface ICalculatorService
    {
        public CalculationResponse Calculate(string expression);
    }
}
