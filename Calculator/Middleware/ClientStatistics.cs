namespace Calculator.Middleware
{
    public class ClientStatistics
    {
        public DateTime LastSuccessfulResponseTime { get; set; }
        public int NumberofRequestsCompletedSuccessfully { get; set; }
    }
}
