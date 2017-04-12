using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Dice : Game
    {

        string[] player = new string[2];
        string[] host = new string[2];
        int[] score = new int[2];
        int[] round = new int[2];

        GameState gameState = GameState.Waiting;
        int turn = 0;

        public Dice(Bot bot, Channel chan, IrcEventArgs e)
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
                if (bot.isCommand(e, "join")) //&& !(string.Equals(bot.GetHost(e.Data.Nick), host[0])))
                {
                    player[1] = e.Data.Nick;
                    host[1] = bot.GetHost(e.Data.Nick);
                    gameState = GameState.Running;
                    chan.StartTimer(15);

                    chan.SendMessage("The game starts now : " + player[0] + " vs. " + player[1] + ". You have up to three rolls (1-6), your last roll counts.");
                    chan.SendNotice(player[turn], "It's your turn! Use " + Msg.Bold(bot.Prefix + "roll") + " to roll the dice or "
                        + Msg.Bold(bot.Prefix + "stop") + " to end your turn.");
                }
            }
            //Game running
            else if ((e.Data.Nick == player[0] && turn == 0) || (e.Data.Nick == player[1] && turn == 1))
            {
                if (bot.isCommand(e, "roll"))
                {
                    score[turn] = bot.Random.Next(1, 7);
                    round[turn]++;
                    if (round[turn] == 3)
                    {
                        chan.SendNotice(player[turn], "You rolled a " + Msg.Bold(score[turn]) + ". This was your last roll.");
                        if (turn < 1)
                            Next();
                        else
                            Over();
                    }
                    else
                    {
                        chan.SendNotice(player[turn], "You rolled a " + Msg.Bold(score[turn]) + 
                            ". You have " + (3 - round[turn]) + " attempt(s) left -> " + bot.Prefix + "roll/stop");
                        chan.StartTimer(15);
                    }
                }
                else if (bot.isCommand(e, "stop"))
                {
                    if (turn < 1)
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
            chan.SendNotice(player[turn], "It's your turn! Use " + Msg.Bold(bot.Prefix + "roll") + " to roll the dice or "
                + Msg.Bold(bot.Prefix + "stop") + " to end your turn.");
            chan.StartTimer(15);
        }

        private void Over()
        {
            chan.SendMessage(player[0] + ": " + Msg.Bold(score[0]) + " vs. " + player[1] + ": " + Msg.Bold(score[1]));

            if (score[0] > score[1])
                Winner(0);
            else if (score[0] < score[1])
                Winner(1);
            else if (score[0] <= 3)
            {
                chan.SendMessage("Both players lose 20$");
                bot.UpdateMoney(host[0], -20);
                bot.UpdateMoney(host[1], -20);
            }
            else
            {
                chan.SendMessage("Both players win 10$");
                bot.UpdateMoney(host[0], 10);
                bot.UpdateMoney(host[1], 10);
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

