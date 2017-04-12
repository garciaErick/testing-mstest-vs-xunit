using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Slot : Game
    {
        int[] prizes = { -10, 30, 300, 1337 };
        string[] words = { "Apple", "Pear", "Cherry", "Citron", "Orange", "Banana", "1337" };

        string[] slots;
        string player;
        string host;

        public Slot(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            slots = new string[] { words[bot.Random.Next(0, words.Length)], words[bot.Random.Next(0, words.Length)], 
                                 words[bot.Random.Next(0, words.Length)] };
            player = e.Data.Nick;
            host = bot.GetHost(player);
        }

        public override void Start()
        {
            string temp = "(" + slots[0] + ") (" + slots[1] + ") (" + slots[2] + ")";

            //3x "1337"
            if ((slots[0] == slots[1]) && (slots[1] == slots[2]) && (slots[0] == "1337"))
            {
                temp += " C0nGr47ul47!0nz" + Msg.Bold(player) + " u won " + prizes[3] + "$ - go pwn some n00bs :>";
                bot.UpdateMoney(host, prizes[3]);
            }
            //3 equal
            else if ((slots[0] == slots[1]) && (slots[1] == slots[2]))
            {
                temp += " Congratulations " + Msg.Bold(player) + " you won " + prizes[2] + "$";
                bot.UpdateMoney(host, prizes[2]);
            }
            //0 equal
            else if ((slots[0] != slots[1]) && (slots[1] != slots[2]) && (slots[0] != slots[2]))
            {
                temp += " Sorry " + Msg.Bold(player) + " but you lost " + -prizes[0] + "$";
                bot.UpdateMoney(host, prizes[0]);
            }
            //2 equal
            else
            {
                temp += " Congratulations " + Msg.Bold(player) + " you won " + prizes[1] + "$";
                bot.UpdateMoney(host, prizes[1]);
            }

            chan.SendMessage(temp);
            chan.DisposeGame();
        }

        public override void Update(IrcEventArgs e)
        {
        }

        public override void TimeOut()
        {
        }
    }  
}