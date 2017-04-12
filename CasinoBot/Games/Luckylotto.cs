using Meebey.SmartIrc4net;
using System.Collections.Generic;

namespace CasinoBot
{
    class Luckylotto : Game
    {
        const int time = 15;
        const int prize = 50;

        int solution;
        List<string> player;
        List<string> host;
        List<int> bet;

        public Luckylotto(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            solution = bot.Random.Next(0, 10);
            player = new List<string>();
            host= new List<string>();
            bet = new List<int>();   
        }

        public override void Start()
        {
            chan.StartTimer(time);
            chan.SendMessage("Everybody in the channel has " + time + " seconds to choose a number between 0 and 9." +
            " Each player who guessed the correct number wins. The more players joined, the higher the prize.");
        }


        public override void Update(IrcEventArgs e)
        {
            int tmp;
            if (e.Data.Message.Length == 1 && !player.Contains(e.Data.Nick) && int.TryParse(e.Data.Message, out tmp))
            {
                player.Add(e.Data.Nick);
                host.Add(e.Data.Host);
                bet.Add(tmp);
            }
        }

        public override void TimeOut()
        {
            if (player.Count == 0)
                chan.SendMessage("Nobody entered the game in time - it would have been " + Msg.Bold(solution.ToString()));
            else
            {
                string tmp = "";
                int earned = prize + 25 * player.Count;
                int lost = 10 + player.Count;

                for (int i = 0; i < bet.Count; i++)
                {
                    if (bet[i] == solution)
                    {
                        tmp += " " + player[i];
                        bot.UpdateMoney(host[i], earned);
                    }
                    else
                        bot.UpdateMoney(host[i], -lost);
                }

                if (tmp.Length == 0)
                    chan.SendMessage("Every player lost " + lost + "$ - the correct number would have been " + Msg.Bold(solution));
                else
                    chan.SendMessage("The following players were lucky and win " + earned + "$:" + 
                        Msg.Bold(tmp) + ". Everybody else lost " + lost + "$. Correct was: " + Msg.Bold(solution));
            }
            chan.DisposeGame();
        }
    }
}