namespace Calculator.Services.Evaluation
{
    public class EvaluatorResponse : Response
    {

        public EvaluatorResponse()
        {
            
        }
        public EvaluatorResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public EvaluatorResponse(List<string> result)
        {
            Success = true;
            Result = result;
        }

        public List<string> Result { get; set; }
    }
}
