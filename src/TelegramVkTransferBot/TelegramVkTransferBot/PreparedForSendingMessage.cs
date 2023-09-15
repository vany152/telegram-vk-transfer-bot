namespace TelegramVkTransferBot;

public struct PreparedForSendingMessage
{
    public string Sender;
    public string? Text;
    public bool IsAttached;

    public ICollection<string> AttachmentPaths;
}