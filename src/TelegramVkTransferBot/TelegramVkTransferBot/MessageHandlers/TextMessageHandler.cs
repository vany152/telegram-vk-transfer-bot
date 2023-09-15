using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramVkTransferBot.MessageHandlers;

public class TextMessageHandler : MessageHandlerBase
{
    public TextMessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    protected override void GetFileId()
    {
        throw new NotImplementedException();
    }
}