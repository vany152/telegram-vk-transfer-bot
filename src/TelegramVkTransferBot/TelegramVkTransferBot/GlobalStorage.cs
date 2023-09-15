using Telegram.Bot;

namespace TelegramVkTransferBot;

public static class GlobalStorage
{
    public static ITelegramBotClient BotClient { get; set; }
    public static CancellationToken Cts { get; set; }
}