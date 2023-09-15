using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using TelegramVkTransferBot.MessageHandlers;
using VkBotFramework;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace TelegramVkTransferBot;

public class VkMessageSender : IMessageSender
{
    public void Send(ConsolidatedMessage message)
    {
        // var attachmentsCount = message.AttachmentIds.Count;
        var preparedForSendingMessage = PrepareForSending(message);
        Send(preparedForSendingMessage);
    }

    
    
    private static PreparedForSendingMessage PrepareForSending(ConsolidatedMessage message)
    {
        var preparedForSendingMessage = new PreparedForSendingMessage
        {
            Sender = message.Sender,
            Text = message.Text,
            IsAttached = message.IsAttached,
            AttachmentPaths = new List<string>()
        };
        
        if (!message.IsAttached)
            return preparedForSendingMessage;

        // var downloadTasks = new List<Task>();
        foreach (var attachmentId in message.AttachmentIds)
        {
            var fileDownloader = new MediaFileDownloader(GlobalStorage.BotClient);
            var downloadTask = fileDownloader.Download(attachmentId, GlobalStorage.Cts);
            // downloadTasks.Add(downloadTask);
            // todo result блокирует вызывающий поток. Нужно сделать так, чтобы поток не блокировался до выхода из цикла
            preparedForSendingMessage.AttachmentPaths.Add(downloadTask.Result);  
        }

        // foreach (var task in downloadTasks) 
            // task.Wait();

        return preparedForSendingMessage;
    }

    private void Send(PreparedForSendingMessage message)
    {
        var vkBot = ConfigureVkBot();
        var rnd = new Random();

        var messageText = $"{message.Text}\n\n" +
                          $"Отправитель: @{message.Sender}";

        var attachments = new List<MediaAttachment>();
        foreach (var attachmentPath in message.AttachmentPaths)
        {
            var uploadServer = vkBot.Api.Photo.GetMessagesUploadServer(2000000002);
            
            using var client = new HttpClient();
            
            // client.DefaultRequestHeaders.Remove("charset");
            // client.DefaultRequestHeaders.Remove("Content-Type");
            // client.DefaultRequestHeaders.Add("charset", "UTF-8");
            // client.DefaultRequestHeaders.Add("Accept", "application/json; charset=utf-8");

            var request = new HttpRequestMessage(HttpMethod.Post, uploadServer.UploadUrl);

            var content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(File.ReadAllBytes(attachmentPath)), "photo", Path.GetFileName(attachmentPath));
            request.Content = content;
            // request.Headers.Add("Accept", "application/json; charset=utf-8");

            // request.Headers.Remove("Content-Type");
            // request.Headers.Remove("charset");
            // request.Headers.Add("Content-Type", "application/json");
            // request.Headers.Add("charset", "UTF-8");
            
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            // var responseBody = response.Content.ReadAsStringAsync().Result;
            
            var buffer = response.Content.ReadAsByteArrayAsync().Result;
            var byteArray = buffer.ToArray();
            var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            // var dyn = JsonSerializer.Deserialize<dynamic>(responseString);

            var photo = vkBot.Api.Photo.SaveMessagesPhoto(responseString);
            attachments.Add(photo.First());
        }

        
        vkBot.Api.Messages.Send(new MessagesSendParams
        {
            PeerId = 2000000002, 
            RandomId = rnd.Next(), 
            Message = messageText,
            Attachments = attachments,
        });
    }

    private static VkBot ConfigureVkBot()
    {
        const string accessToken = "vk1.a.5-OLgN5ic2d8dBQnJSZ_zl3Kat4kZ3kO1Gb0QFg9akW0XSUw-ONiuNCAVnQSKFIngOrKFuoqfiV9qi5swd5Q1Q5zY-qdUf7XscGQv1arYtK2uwbp6vWVG4pAuLwMvezZ0qaf2_-AZO5oOdrb6nfh2e-kv9xCfVPFxMDQXZsYbCFUo4p5T2fOwI5saCg8R6dcxGwcSzDFf0DnTpUvMT3ruw";
        const string groupUrl = "https://vk.com/club222393892";
        var vkBot = new VkBot(accessToken, groupUrl);

        return vkBot;
    }
}