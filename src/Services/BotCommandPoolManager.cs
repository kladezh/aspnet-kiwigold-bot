﻿using System.Collections;

using Telegram.Bot.Types;

using KiwigoldBot.Interfaces;

namespace KiwigoldBot.Services
{
    public class BotCommandPoolManager : IBotCommandPoolManager
    {
        private readonly Queue<Func<Message, CancellationToken, Task>> _pool = new();

        public bool IsActive => _pool.Count > 0;

        public IBotCommandPoolManager Add(Func<Message, CancellationToken, Task> action)
        {
            _pool.Enqueue(action);

            return this;
        }

        public async Task ExecuteLastAsync(Message message, CancellationToken cancellationToken)
        {
            if (_pool.TryDequeue(out var action)) 
            {
                await action.Invoke(message, cancellationToken);
            }
        }
    }
}
