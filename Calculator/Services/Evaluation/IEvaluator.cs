using static Calculator.Services.Calculator.Calculations;

namespace Calculator.Services.Evaluation
{
    public interface IEvaluator
    {
        public EvaluatorResponse Evaluate(string expression);
        public void SetOperations(Dictionary<string, IOperation> operations);

        public void PushOperation(string operationName, IOperation operation);

        public void PopOperation(string operationName);
     
    }
}
