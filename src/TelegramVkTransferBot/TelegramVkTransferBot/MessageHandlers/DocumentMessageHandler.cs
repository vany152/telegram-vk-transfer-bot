using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramVkTransferBot.MessageHandlers;

public class DocumentMessageHandler : MessageHandlerBase
{
    public DocumentMessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    protected override void GetFileId()
    {
        var document = Message.Document;
        if (document is null)
            throw new ArgumentException("тип вложения указан как Document, но документ отсутствует");
        
        FileId = document.FileId;
    }
}