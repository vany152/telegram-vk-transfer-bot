using Telegram.Bot;

namespace TelegramVkTransferBot;

public class MediaFileDownloader
{
    private readonly ITelegramBotClient _botClient;

    public MediaFileDownloader(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task<string> Download(string fileId, CancellationToken cts)
    {
        var filePath = await GetFilePath(fileId, cts); 

        var (destStream, destFilePath) = CreateDestFileStream(filePath);
        
        // todo проверка размера файла
        await _botClient.DownloadFileAsync(filePath, destStream, cts);
        await destStream.DisposeAsync();
        
        return destFilePath;
    }

    private async Task<string> GetFilePath(string fileId, CancellationToken cts)
    {
        var fileInfo = await _botClient.GetFileAsync(fileId, cts);
        var filePath = fileInfo.FilePath;

        if (filePath is null)
            throw new InvalidOperationException("сервер не вернул путь к запрошенному файлу");
        
        return filePath;
    }

    private static (Stream stream, string filesystemPath) CreateDestFileStream(string filePath)
    {
        // todo вынести в конфиги
        var destPath = $"D:/files/{filePath}";
        CreateFilesDirectoryIfNotExits(destPath);
        
        var destStream = File.Create(destPath);
        return (destStream, destPath);
    }

    private static void CreateFilesDirectoryIfNotExits(string filePath)
    {
        var destPathDirectory = Path.GetDirectoryName(filePath);
        if (destPathDirectory is null)
            throw new InvalidOperationException("не удалось получить родительскую директорию файла");
        
        if (!Directory.Exists(destPathDirectory))
            Directory.CreateDirectory(destPathDirectory);
    }
}