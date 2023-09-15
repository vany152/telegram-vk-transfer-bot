// See https://aka.ms/new-console-template for more information

using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramVkTransferBot;

var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;
GlobalStorage.Cts = cancellationToken;

const string token = "6477538585:AAEme0vseuaFANUFYPLGc0o9gUF2G8VczFQ";
var bot = new TelegramBotClient(token);
GlobalStorage.BotClient = bot;
// MessageHandler.BotClient = bot;
// MessageHandler.CancellationToken = cancellationToken;
// var messageHandler = new MessageHandler(bot, cancellationToken);

var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
bot.StartReceiving(
    BotMessageHandler.HandleUpdateAsync,
    BotMessageHandler.HandleErrorAsync,
    receiverOptions,
    cancellationToken
);

Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

Console.ReadLine();
