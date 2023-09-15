using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramVkTransferBot.MessageHandlers;

public class PhotoMessageHandler : MessageHandlerBase
{
    public PhotoMessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    protected override void GetFileId()
    {
        var photo = Message.Photo;
        if (photo is null)
            throw new ArgumentException("тип вложения указан как Photo, но фото отсутствует");
        
        FileId = photo.Last().FileId;
    }
}