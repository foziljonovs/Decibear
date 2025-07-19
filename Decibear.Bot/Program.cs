using Decibear.Bot.Handlers;
using Decibear.Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IUpdateHandler, CommandHandler>();
builder.Services.AddTransient<IAvaterService, AvaterService>();
builder.Services.AddHostedService<TelegramBotService>();
builder.Services.AddSingleton<ITelegramBotClient>(
    new TelegramBotClient(builder.Configuration["Decibear:Token"]));

builder.Services.AddHttpClient();

builder.Build().Run();
