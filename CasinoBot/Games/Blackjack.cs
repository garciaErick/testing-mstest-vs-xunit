using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Blackjack : Game
    {

        string[] player = new string[2];
        string[] host = new string[2];
        int[] score = new int[2];

        GameState gameState = GameState.Waiting;
        int turn = 0;
        int count = 2;

        public Blackjack(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            player[0] = e.Data.Nick;
            host[0] = bot.GetHost(e.Data.Nick);
            score[0] = 0;
            score[1] = 0;
        }

        public override void Start()
        {
            chan.StartTimer(30);
            chan.SendMessage(player[0] + " wants to play. Type " + bot.Prefix + "join to play against him :)");
        }

        public override void Update(IrcEventArgs e)
        {
            if (gameState == GameState.Waiting)
            {
                if (bot.isCommand(e, "join") && !(string.Equals(bot.GetHost(e.Data.Nick), host[0])))
                {
                    player[1] = e.Data.Nick;
                    host[1] = bot.GetHost(e.Data.Nick);
                    gameState = GameState.Running;
                    chan.StartTimer(15);

                    chan.SendMessage("The game starts now : " + player[0] + " vs. " + player[1] + ". The player closest to (but not over) 21 points wins.");
                    chan.SendNotice(player[turn], "It's your turn! Use " + Msg.Bold(bot.Prefix + "draw") + " to get cards and "
                        + Msg.Bold(bot.Prefix + "stop") + " to end your turn.");
                }
            }
            //Game running
            else if ((e.Data.Nick == player[0] && turn == 0) || (e.Data.Nick == player[1] && turn == 1))
            {
                if (bot.isCommand(e, "draw"))
                {
                    score[turn] += bot.Random.Next(2, 10);
                    if (score[turn] > 21)
                    {
                        chan.SendNotice(player[turn], "You currently have " + Msg.Bold(score[turn]) + " Points -> Your turn has ended.");
                        Next();
                    }
                    else
                    {
                        chan.SendNotice(player[turn], "You currently have " + Msg.Bold(score[turn]) + " Points -> " + bot.Prefix + "draw/stop");
                        chan.StartTimer(15);
                    }
                }
                else if (bot.isCommand(e, "stop"))
                {
                    if (turn < count - 1)
                        Next();
                    else
                        Over();
                }
            }
        }

        public override void TimeOut()
        {
            if (gameState == GameState.Waiting)
                chan.SendMessage("Sorry " + player[0] + " but nobody wanted to play with you." +
                    " Seems like you should get some friends ;)");
            else
                chan.SendMessage("Gameover - " + player[turn] + " was too slow.");

            chan.DisposeGame();
        }

        private void Next()
        {
            turn++;
            chan.SendNotice(player[turn], "It's your turn! Use " + Msg.Bold(bot.Prefix + "draw") + " to get cards and "
                + Msg.Bold(bot.Prefix + "stop") + " to end your turn.");
            chan.StartTimer(15);
        }

        private void Over()
        {
            chan.SendMessage(player[0] + ": " + Msg.Bold(score[0]) + " vs. " + player[1] + ": " + Msg.Bold(score[1]));

            if (score[0] > 21 && score[1] > 21)
            {
                 chan.SendMessage("Both players lose 20$");
                 bot.UpdateMoney(host[0], -20);
                 bot.UpdateMoney(host[1], -20);
            }
            else if (score[0] > 21 && score[1] <= 21)
                Winner(1);
            else if (score[0] <= 21 && score[1] > 21)
                Winner(0);
            else //both under 21
            {
                if (score[0] == score[1])
                {
                    chan.SendMessage("Both players win 10$");
                    bot.UpdateMoney(host[0], 10);
                    bot.UpdateMoney(host[1], 10);
                }
                else if (score[0] > score[1])
                    Winner(0);
                else
                    Winner(1);
            }
            chan.DisposeGame();
        }

        private void Winner(int winner)
        {
            int loser = (winner + 1) % 2;

            chan.SendMessage(player[winner] + " wins 35$ and " + player[loser] + " loses 20$.");
            bot.UpdateMoney(host[winner], 35);
            bot.UpdateMoney(host[loser], -20);
        }

    }
}
