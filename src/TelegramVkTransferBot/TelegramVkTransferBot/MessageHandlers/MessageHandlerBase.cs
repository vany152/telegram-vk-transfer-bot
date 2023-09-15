using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace TelegramVkTransferBot.MessageHandlers;

/// <summary>
/// Интерфейс для классов
/// </summary>
public abstract class MessageHandlerBase
{
    private CancellationToken _cts;

    private ITelegramBotClient _botClient;
    protected Message Message = null!;
    protected string FileId = null!;

    protected MessageHandlerBase(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }
    
    public async Task Handle(Message message, CancellationToken cts)
    {
        Message = message;
        _cts = cts;
        
        if (message.Type is MessageType.Text)
            HandleTextMessage();
        else
            await HandleAttachedMessage();
    }

    private void HandleTextMessage()
    {
        var text = GetText();
        // todo отпарвка в вк
    }
    
    private string GetText()
    {
        var text = Message.Text;
        if (text is null)
            throw new InvalidOperationException("В текстовом сообщении отсутствует текст");

        return text;
    }

    private async Task HandleAttachedMessage()
    {
        GetFileId();
        await DownloadFileById();
    }
    
    protected abstract void GetFileId();

    private async Task DownloadFileById()
    {
        var fileDownloader = new MediaFileDownloader(_botClient);
        await fileDownloader.Download(FileId, _cts);
    }
}
