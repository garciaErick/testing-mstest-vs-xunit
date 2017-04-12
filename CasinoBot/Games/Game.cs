using Meebey.SmartIrc4net;

namespace CasinoBot
{
    abstract class Game
    {
        protected readonly Bot bot;
        protected readonly Channel chan;

        public Game(Bot bot, Channel chan)
        {
            this.bot = bot;
            this.chan = chan;
        }

        public abstract void Start();

        public abstract void Update(IrcEventArgs e);

        public abstract void TimeOut();

    }
}
