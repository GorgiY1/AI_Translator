using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace Translator.Handlers
{
    public interface ICommandHandler
    {
        bool CanHandle(string command);
        Task HandleAsync(Message message, ITelegramBotClient bot, CancellationToken cancellationToken);
    }
}
