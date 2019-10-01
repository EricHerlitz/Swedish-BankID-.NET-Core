namespace Herlitz.BankID
{
    public interface ICollectResponse 
    {
        string OrderRef { get; set; }
        string Status { get; set; }
        string HintCode { get; set; }
        CompletionData CompletionData { get; set; }
    }
}