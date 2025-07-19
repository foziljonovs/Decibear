
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Decibear.Bot.Services;

public class TelegramBotService(
    ITelegramBotClient botClient,
    IUpdateHandler updateHandler) : BackgroundService
{
    private readonly ITelegramBotClient _botClient = botClient;
    private readonly IUpdateHandler _updateHandler = updateHandler;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var me = await _botClient.GetMe(stoppingToken);

        _botClient.StartReceiving(
            _updateHandler,
            new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message
                } 
            },
            stoppingToken);
    }
}
