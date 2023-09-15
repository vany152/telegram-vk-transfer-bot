using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramVkTransferBot.MessageHandlers;

namespace TelegramVkTransferBot;

public static class BotMessageHandler
{
    private static readonly MessageBufferController MessageBufferController = new ();
    
    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cts)
    {
        // await Console.Out.WriteLineAsync($"{JsonSerializer.Serialize(update)}\n\n");
    
        if (update.Type is not UpdateType.Message)
            return;
    
        var message = update.Message;
        if (message is null)
            return;
        
        if (!IsMessageTypeCorrect(message))
        {
            await SendUnsupportedAttachmentTypeErrMessage(botClient, message, cts);
            return;
        }
        
        /*
         * При поступлении сообщений добавляем их в буфер. После окончания сообщений, отпраяленных в одно время
         * (относящихся к одному сообщению в телеге), из буфера нужно извлечь сообщения, пришедшие одновременно
         * и сгруппировать их по отправителю. После группировки сообщения объединяются в одно и отправляются в вк
         */
        try
        {
            MessageBufferController.Add(message);
        }
        catch (Exception err) // todo не работает
        {
            await botClient.SendTextMessageAsync(message.Chat, "Произошла ошибка", cancellationToken: cts);
        }

        // MessageHandlerBase? messageHandler = message.Type switch
        // {
        //     MessageType.Text => new TextMessageHandler(botClient),
        //     MessageType.Photo => new PhotoMessageHandler(botClient),
        //     MessageType.Animation => new AnimationMessageHandler(botClient),
        //     MessageType.Video => new VideoMessageHandler(botClient),
        //     MessageType.Audio => new AudioMessageHandler(botClient),
        //     MessageType.Document => new DocumentMessageHandler(botClient),
        //     _ => null
        // };
        
        // if (messageHandler is null)
        // {
        //     await SendUnsupportedAttachmentTypeErrMessage(botClient, message, cts);
        //     return;
        // }
        
        // await messageHandler.Handle(message, cts);
        
        await botClient.SendTextMessageAsync(message.Chat, "сообщение получено", cancellationToken: cts);
    }
    
    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cts) => 
        await Console.Error.WriteLineAsync(JsonSerializer.Serialize(exception));



    private static bool IsMessageTypeCorrect(Message message) =>
        message.Type is MessageType.Text 
            or MessageType.Photo 
            or MessageType.Animation 
            or MessageType.Video
            or MessageType.Audio 
            or MessageType.Document;

    private static async Task SendUnsupportedAttachmentTypeErrMessage(ITelegramBotClient botClient, Message message, CancellationToken cts)
    {
        var errMessage = $"""
                          Неподдерживаемый тип сообщения: {message.Type}
                          Бот поддерживает следующие типы сообщений:
                          * текст
                          * фото
                          * видео
                          * аудио
                          * документ
                          * анимация
                          """;
        await botClient.SendTextMessageAsync(message.Chat, errMessage, cancellationToken: cts);
    }
}
