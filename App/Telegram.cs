using System;
using System.Collections.Generic;
using App.Models;
using App.Messages;

namespace App.Channels
{
    
    public class TelegramChannel : IChannel
    {
        public string Name => "Telegram";

    
        public IEnumerable<RecipientType> AllowedRecipientTypes => new[]
        {
            RecipientType.PhoneNumber,
            RecipientType.Username
        };

        public void Send(Message message, Recipient recipient)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (recipient == null) throw new ArgumentNullException(nameof(recipient));

            if (!IsRecipientAllowed(recipient))
                throw new InvalidOperationException($"O tipo de destinatário '{recipient.Type}' não é suportado pelo Telegram.");

            SendImplementation(message, recipient);
        }

        private bool IsRecipientAllowed(Recipient recipient)
        {
            foreach (var allowed in AllowedRecipientTypes)
                if (allowed == recipient.Type)
                    return true;
            return false;
        }

        
        private void SendImplementation(Message message, Recipient recipient)
        {
            Console.WriteLine($"[Telegram] Enviando mensagem '{message.Type}' para {recipient.Value}...");

            switch (message)
            {
                case TextMessage textMsg:
                    SendText(textMsg, recipient);
                    break;
                case PhotoMessage photoMsg:
                    SendPhoto(photoMsg, recipient);
                    break;
                case VideoMessage videoMsg:
                    SendVideo(videoMsg, recipient);
                    break;
                case FileMessage fileMsg:
                    SendFile(fileMsg, recipient);
                    break;
                default:
                    throw new NotSupportedException($"Tipo de mensagem '{message.Type}' não suportado no Telegram.");
            }

            Console.WriteLine("[Telegram] Envio concluído com sucesso (simulação).");
        }


        private void SendText(TextMessage msg, Recipient recipient)
        {
            // Aqui poderia estar a chamada para a API do Telegram Bot
            // Exemplo real: TelegramBotClient.SendTextMessageAsync(...)
            Console.WriteLine($"[Telegram/Text] Para: {recipient.Value}");
            Console.WriteLine($"Conteúdo: {msg.Body}");
        }

        private void SendPhoto(PhotoMessage msg, Recipient recipient)
        {
            Console.WriteLine($"[Telegram/Photo] Para: {recipient.Value}");
            Console.WriteLine($"Arquivo: {msg.Photo.Name} ({msg.Photo.Format})");
            if (!string.IsNullOrEmpty(msg.Body))
                Console.WriteLine($"Legenda: {msg.Body}");
        }

        private void SendVideo(VideoMessage msg, Recipient recipient)
        {
            Console.WriteLine($"[Telegram/Video] Para: {recipient.Value}");
            Console.WriteLine($"Arquivo: {msg.Video.Name} ({msg.Video.Format})");
            if (msg.Video.Duration.HasValue)
                Console.WriteLine($"Duração: {msg.Video.Duration.Value.TotalSeconds} segundos");
            if (!string.IsNullOrEmpty(msg.Body))
                Console.WriteLine($"Legenda: {msg.Body}");
        }

        private void SendFile(FileMessage msg, Recipient recipient)
        {
            Console.WriteLine($"[Telegram/File] Para: {recipient.Value}");
            Console.WriteLine($"Arquivo: {msg.File.Name} ({msg.File.Format})");
            if (!string.IsNullOrEmpty(msg.Body))
                Console.WriteLine($"Mensagem: {msg.Body}");
        }
    }
}
