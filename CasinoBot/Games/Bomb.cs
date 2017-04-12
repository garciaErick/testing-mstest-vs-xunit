using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Bomb : Game
    {
        int time = 10;
        int prize = 70;
        string[] colors = { "red", "blue", "green", "yellow", "black" };

        string player;
        string host;
        string solution;

        public Bomb(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            solution = colors[bot.Random.Next(0, colors.Length)];
            player = e.Data.Nick;
            host = bot.GetHost(player);
        }

        public override void Start()
        {
            string tmp = bot.Prefix;
            foreach (string s in colors)
                tmp += s + "/";

            chan.StartTimer(time);
            chan.SendMessage(player + " recieves the bomb. You have " + time + " seconds to defuse it using by cutting the right cable." +
            " Choose you destiny: " + Msg.Bold(tmp));
        }

        public override void Update(IrcEventArgs e)
        {
            string[] msgArray = e.Data.MessageArray;

            if (bot.GetHost(e.Data.Nick) == host)
            {
                if (msgArray[0].ToLower() == solution || msgArray[0].ToLower() + bot.Prefix == solution)
                {
                    bot.UpdateMoney(host, prize);
                    chan.SendMessage(player + " defused the bomb. Seems like he was wise enough to buy a defuse kit. You win " + prize + "$");
                }
                else
                {
                    chan.SendMessage("The bomb explodes in " + player + "'s hands. You lost your life and - even worse - $20." +
                        " The right color would have been " + Msg.Bold(solution));
                    bot.UpdateMoney(host, -20);
                }
                chan.DisposeGame();
            }   
        }

        public override void TimeOut()
        {
            chan.SendMessage("the bomb explodes in front of " + player + ". Seems like you did not even notice the big beeping suitcase. You loose $50");
            bot.UpdateMoney(host, -50);
            chan.DisposeGame();
        }
    }
}