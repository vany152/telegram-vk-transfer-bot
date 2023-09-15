using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramVkTransferBot.MessageHandlers;

public class AudioMessageHandler : MessageHandlerBase
{
    public AudioMessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    protected override void GetFileId()
    {
        var audio = Message.Audio;
        if (audio is null)
            throw new ArgumentException("тип вложения указан как Audio, но аудио отсутствует");
        
        FileId = audio.FileId;
    }
}