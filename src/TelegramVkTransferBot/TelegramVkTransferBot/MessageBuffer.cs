using Telegram.Bot.Types;

namespace TelegramVkTransferBot;

public class MessageBuffer
{
    private readonly IDictionary<DateTime, ICollection<Message>> _buffer = new Dictionary<DateTime, ICollection<Message>>();

    public void Add(Message message)
    {
        var sendTime = message.Date;
        
        if (!_buffer.ContainsKey(sendTime))
            _buffer[sendTime] = new List<Message>();

        _buffer[sendTime].Add(message);
    }

    public void Remove(DateTime sendTime) => 
        _buffer.Remove(sendTime);

    public ICollection<Message> GetMessagesBySendTime(DateTime sendTime) =>
        _buffer[sendTime];
}
