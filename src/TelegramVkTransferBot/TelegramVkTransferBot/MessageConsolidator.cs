using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramVkTransferBot;

public class MessageConsolidator
{
    private ICollection<Message> _messagesPart;
    private bool _messageContainsOnlyText;

    private ConsolidatedMessage _consolidatedMessage;

    public MessageConsolidator(ICollection<Message> messagesPart)
    {
        _messagesPart = messagesPart;
    }

    public ConsolidatedMessage Consolidate()
    {
        InitConsolidatedMessage();
        
        SetFlagIfMessageContainsOnlyText();
        if (_messageContainsOnlyText)
            ProcessTextMessage();
        else
            ProcessAttachedMessage();

        return _consolidatedMessage;
    }



    private void SetFlagIfMessageContainsOnlyText() => 
        _messageContainsOnlyText = _messagesPart.First().Type is MessageType.Text;

    private void InitConsolidatedMessage()
    {
        var firstPart = _messagesPart.First();
        
        _consolidatedMessage = new ConsolidatedMessage
        {
            Sender = firstPart.From?.Username ?? throw new InvalidOperationException("Отправитель сообщения не указан"), 
            SendTime = firstPart.Date,
            AttachmentIds = new List<string>()
        };
    }

    private void ProcessTextMessage() => 
        _consolidatedMessage.Text = _messagesPart.First().Text;

    private void ProcessAttachedMessage()
    {
        foreach (var part in _messagesPart)
        {
            _consolidatedMessage.Text ??= part.Caption;
            _consolidatedMessage.IsAttached = true;

            var attachmentId = AttachmentIdProvider.ProvideId(part);
            _consolidatedMessage.AttachmentIds.Add(attachmentId);
        }
    }
}