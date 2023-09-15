namespace TelegramVkTransferBot;

public struct ConsolidatedMessage
{
    public string Sender;
    public DateTime SendTime;
    
    public string? Text;
    public bool IsAttached;
    public ICollection<string> AttachmentIds;
}