using Telegram.Bot.Types;
using Timer = System.Threading.Timer;

namespace TelegramVkTransferBot;

/// <summary>
/// Контролирует буфер сообщений и запускает процесс пересылки сообщений в другой сервис 
/// </summary>
public class MessageBufferController
{
    private readonly MessageBuffer _messageBuffer = new ();
    
    /// <summary>
    /// Таймеры обработки сообщениий, отправленных в определенное время
    /// </summary>
    private readonly IDictionary<DateTime, Timer> _handleMessageTimers = new Dictionary<DateTime, Timer>();

    /// <summary>
    /// Метод добавляет сообщение в буфер и, если поступившее сообщение с таким временем отправки из телеграма
    /// первое, запускает таймер, по истечении которого запускается процесс группировки сообщений с таким же
    /// временем отправки из телеграма, как и у текущего, и пересылки в другой сервис. В противном случае новый
    /// таймер не создается   
    /// </summary>
    /// <param name="message">Поступившее сообщение</param>
    public void Add(Message message)
    {
        _messageBuffer.Add(message);

        var sendTime = message.Date;
        if (!_handleMessageTimers.ContainsKey(sendTime))
            _handleMessageTimers[sendTime] = CreateTimer(sendTime);
    }

    private Timer CreateTimer(DateTime sendTime)
    {
        var callback = new TimerCallback(SendMessages);
        var timer = new Timer(callback, sendTime, 2000, Timeout.Infinite); // todo увеличить время

        return timer;
    }

    /// <summary>
    /// Пересылка сообщений, отправленных из телеграма в одно и то же время
    /// </summary>
    /// <param name="sendTime">Время отправка сообщений из телеграма</param>
    private void SendMessages(object? sendTime)
    {
        var messages = GetMessagesBySendTime(sendTime);
        var consolidatedMessage = ConsolidateMessages(messages);
        SendMessage(consolidatedMessage);
    }

    private ICollection<Message> GetMessagesBySendTime(object? sendTimeObj)
    {
        if (sendTimeObj is null) throw new ArgumentNullException(paramName: nameof(sendTimeObj));

        var sendTime = (DateTime)sendTimeObj;
        return _messageBuffer.GetMessagesBySendTime(sendTime);
    }

    private static ConsolidatedMessage ConsolidateMessages(ICollection<Message> messageParts)
    {
        var consolidator = new MessageConsolidator(messageParts);
        var consolidatedMessage = consolidator.Consolidate();

        return consolidatedMessage;
    }

    private static void SendMessage(ConsolidatedMessage message)
    {
        var sender = new VkMessageSender();
        sender.Send(message);
    }
}
