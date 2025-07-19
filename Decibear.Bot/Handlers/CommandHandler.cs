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
        var userId = message.From?.Id;

        if (message.Text is not null && message.Text.StartsWith("/"))
        {
            var parts = message.Text.Split(" ", 2);
            var command = parts[0].TrimStart('/').ToLowerInvariant();
            var seed = parts.Length > 1 ? parts[1] : null;

            if(command is "start")
            {
                await botClient.SendMessage(message.Chat.Id, "Salom! Avataringizni yaratish uchun /avataaars, /bottts, /fun-emoji, /pixel-art buyrug'laridan birini yuboring.", cancellationToken: cancellationToken);
                return;
            }

            if (string.IsNullOrWhiteSpace(seed))
            {
                await botClient.SendMessage(message.Chat.Id, "Iltimos, buyruqdan keyin matn (seed) kiriting. Misol: /fun-emoji Ali", cancellationToken: cancellationToken);
                return;
            }

            switch (command)
            {
                case "fun-emoji":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.FunEmoji,
                            Seed = seed
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning emoji!", cancellationToken: cancellationToken);
                        Console.WriteLine($"user id: {userId}, seed: {request.Seed} style: {request.Style}");
                        break;
                    }
                case "avataaars":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.PixelArt,
                            Seed = seed
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning avatar!", cancellationToken: cancellationToken);
                        Console.WriteLine($"user id: {userId}, seed: {request.Seed} style: {request.Style}");
                        break;
                    }

                case "bottts":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.Bottts,
                            Seed = seed
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning boots!", cancellationToken: cancellationToken);
                        Console.WriteLine($"user id: {userId}, seed: {request.Seed} style: {request.Style}");
                        break;
                    }
                case "pixel-art":
                    {
                        var request = new Models.Request
                        {
                            Style = Style.PixelArt,
                            Seed = seed
                        };

                        var avatarBytes = await _avatarService.GetAsync(request);
                        using var stream = new MemoryStream(avatarBytes);
                        await botClient.SendPhoto(message.Chat.Id, stream, "Bu sizning pixel-art!", cancellationToken: cancellationToken);
                        Console.WriteLine($"user id: {userId}, seed: {request.Seed} style: {request.Style}");
                        break;
                    }
                default:
                    await botClient.SendMessage(message.Chat.Id, "Noma'lum buyruq. Quyidagilardan birini yuboring: /fun-emoji, /bottts, /avataaars, /pixel-art", cancellationToken: cancellationToken);
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
