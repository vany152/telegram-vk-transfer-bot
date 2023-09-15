using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramVkTransferBot.MessageHandlers;

public class VideoMessageHandler : MessageHandlerBase
{
    public VideoMessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    protected override void GetFileId()
    {
        var video = Message.Video;
        if (video is null)
            throw new ArgumentException("тип вложения указан как Video, но видео отсутствует");
        
        FileId = video.FileId;
    }
}