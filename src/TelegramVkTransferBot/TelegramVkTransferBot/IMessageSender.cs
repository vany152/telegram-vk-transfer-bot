using Telegram.Bot.Types;

namespace TelegramVkTransferBot;

public interface IMessageSender
{
    void Send(ConsolidatedMessage message);
}