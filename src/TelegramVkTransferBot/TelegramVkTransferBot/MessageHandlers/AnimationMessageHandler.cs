using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramVkTransferBot.MessageHandlers;

public class AnimationMessageHandler : MessageHandlerBase
{
    public AnimationMessageHandler(ITelegramBotClient botClient) : base(botClient)
    {
    }

    protected override void GetFileId()
    {
        var animation = Message.Animation;
        if (animation is null)
            throw new ArgumentException("тип вложения указан как Animation, но анимация отсутствует");
        
        FileId = animation.FileId;
    }
}