using Decibear.Bot.Models;
using Decibear.Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Decibear.Bot.Handlers;

public class CommandHandler(
    IAvaterService avatarService) : IUpdateHandler
{
    private readonly IAvaterService _avatarService = avatarService;
    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Xatolik: {exception.Message}");

        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            var handleTask = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
                _ => HandleUnknownUpdateAsync(botClient, update, cancellationToken)
            };

            await handleTask;
        }
        catch(Exception ex)
        {
            HandleErrorAsync(botClient, ex, HandleErrorSource.HandleUpdateError, cancellationToken);
        }
    }

    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not null && message.Text.StartsWith("/"))
        {
            var command = message.Text.Split(' ')[0].TrimStart('/').ToLowerInvariant();
            switch (command)
            {
                case "start":
                    await botClient.SendMessage(message.Chat.Id, "Salom! Avataringizni yaratish uchun /avatar buyrug'ini yuboring.", cancellationToken: cancellationToken);
                    break;
                case "fun-emoji":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.FunEmoji,
                            Seed = message.Chat.Id.ToString()
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning avatariz!", cancellationToken: cancellationToken);
                        break;
                    }
                case "avataaars":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.PixelArt,
                            Seed = message.Chat.Id.ToString()
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning avatariz!", cancellationToken: cancellationToken);
                        break;
                    }

                case "bottts":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.Bottts,
                            Seed = message.Chat.Id.ToString()
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning avatariz!", cancellationToken: cancellationToken);
                        break;
                    }
                case "pixel-art":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.PixelArt,
                            Seed = message.Chat.Id.ToString()
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning avatariz!", cancellationToken: cancellationToken);
                        break;
                    }
                default:
                    await botClient.SendMessage(message.Chat.Id, "Noma'lum buyruq. Iltimos, /start yoki /avatar yuboring.", cancellationToken: cancellationToken);
                    break;
            }
        }
    }

    private Task HandleUnknownUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Noma'lum yangilanish turi: {update.Type}");

        return Task.CompletedTask;
    }
}
