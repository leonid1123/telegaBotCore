using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

var botClient = new TelegramBotClient("5289120760:AAGQiwQtVEv397Ww_sOyXlRHiolFj8tK0ao");

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();


async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    ReplyKeyboardMarkup replyKeyboardMarkup1 = new(new[]
        {
        new KeyboardButton[] { "Показать каталог","Наши контакты" },
    })
    {
        ResizeKeyboard = true
    };
    ReplyKeyboardMarkup replyKeyboardMarkup2 = new(new[]
    {
        new KeyboardButton[] { "Смола", "Пластик","Мыло","Назад" },
    })
    {
        ResizeKeyboard = true
    };
    ReplyKeyboardMarkup replyKeyboardMarkup3 = new(new[]
    {
        new KeyboardButton[] { "Пластик", "Мыло","Назад" },
    })
    {
        ResizeKeyboard = true
    };
    ReplyKeyboardMarkup replyKeyboardMarkup4 = new(new[]
{
        new KeyboardButton[] { "Смола", "Мыло","Назад" },
    })
    {
        ResizeKeyboard = true
    };
    ReplyKeyboardMarkup replyKeyboardMarkup5 = new(new[]
{
        new KeyboardButton[] { "Пластик", "Смола","Назад" },
    })
    {
        ResizeKeyboard = true
    };

    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Type != UpdateType.Message)
        return;
    // Only process text messages
    if (update.Message!.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    var messageText = update.Message.Text;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (messageText == "start")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Добро пожаловать",
            replyMarkup: replyKeyboardMarkup1,
            cancellationToken: cancellationToken);
    }
    if (messageText == "Показать каталог")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Ознакомьтесь с нашим каталогом",
            replyMarkup: replyKeyboardMarkup2,
            cancellationToken: cancellationToken);
    }
    if (messageText=="Назад")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Добро пожаловать",
            replyMarkup: replyKeyboardMarkup1,
            cancellationToken: cancellationToken);
    }
    if (messageText == "Смола")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Наша продукция из литьевой смолы",
            replyMarkup: replyKeyboardMarkup3,
            cancellationToken: cancellationToken);
    }
    if (messageText == "Пластик")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Наша продукция, изготовленная на 3D принтере",
            replyMarkup: replyKeyboardMarkup4,
            cancellationToken: cancellationToken);
    }
    if (messageText == "Мыло")
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Наше уникльное мыло ручной работы",
            replyMarkup: replyKeyboardMarkup5,
            cancellationToken: cancellationToken);
    }

}


Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
