using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramVkTransferBot.MessageHandlers;

namespace TelegramVkTransferBot;

public static class AttachmentIdProvider
{
    public static string ProvideId(Message message) =>
        message.Type switch
        {
            MessageType.Photo => GetPhotoId(message),
            MessageType.Animation => GetAnimationId(message),
            MessageType.Video => GetVideoId(message),
            MessageType.Audio => GetAudioId(message),
            MessageType.Document => GetDocumentId(message),
            _ => throw new InvalidOperationException("Некорректный тип сообщения")
        };

    private static string GetPhotoId(Message message) =>
        message.Photo?.Last().FileId ?? throw new InvalidOperationException("Тип сообщения указан как Photo, но фото отсутствует");
    
    private static string GetAnimationId(Message message) =>
        message.Animation?.FileId ?? throw new InvalidOperationException("Тип сообщения указан как Animation, но анимация отсутствует");
    
    private static string GetVideoId(Message message) =>
        message.Video?.FileId ?? throw new InvalidOperationException("Тип сообщения указан как Video, но видео отсутствует");
    
    private static string GetAudioId(Message message) =>
        message.Audio?.FileId ?? throw new InvalidOperationException("Тип сообщения указан как Audio, но аудио отсутствует");
    
    private static string GetDocumentId(Message message) =>
        message.Document?.FileId ?? throw new InvalidOperationException("Тип сообщения указан как Document, но документ отсутствует");
}