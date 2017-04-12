using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Reverse : Game
    {
        const int time = 20;
        const int prize = 10;

        string word;
        string solution = "";

        public Reverse(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            word = bot.RandomWord;
        }

        public override void Start()
        {
            for (int i = 0; i < word.Length; i++)
                solution += word.Substring((word.Length - i - 1), 1);

            chan.StartTimer(time);
            chan.SendMessage("You have " + time + " seconds to write this word in reverse order: " + Msg.Bold(word));
        }

        public override void Update(IrcEventArgs e)
        {
            if (e.Data.Message == solution)
            {
               int earned = bot.UpdateMoney(bot.GetHost(e.Data.Nick), prize + solution.Length, int.Parse(chan.CurrentTime), time);

                chan.SendMessage(e.Data.Nick + " entered the solution in " + chan.CurrentTime +
                    " seconds and wins " + earned + "$. Correct was " + Msg.Bold(solution));
                chan.DisposeGame();
            }
        }

        public override void TimeOut()
        {
            chan.SendMessage("Time is over! The word we looked for was " + Msg.Bold(solution));
            chan.DisposeGame();
        }
    }
}
