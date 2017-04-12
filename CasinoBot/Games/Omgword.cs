using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Omgword : Game
    {
        const int time = 30;
        const int prize = 20;

        string solution;

        public Omgword(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            solution = bot.RandomWord;
        }

        public override void Start()
        {
            string word = "";
            //randomize the order of the chars
            for (int i = 0; i < solution.Length; i++)
            {
                if (bot.Random.Next(0, 2) == 0)
                    word = word + solution.Substring(i, 1);
                else
                    word = solution.Substring(i, 1) + word;
            } 

            chan.StartTimer(time);
            chan.SendMessage("You have " + time + " seconds to solve this: " + Msg.Bold(word)); 
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