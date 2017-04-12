#region Using

using System;
using System.Collections;
using System.Timers;
using System.Text;
using Meebey.SmartIrc4net;

#endregion

namespace CasinoBot
{
    class Channel
    {
        //Used to output all commands in help()
        string[] commandlist = { "blackjack", "bomb", "dice", "hangman", "luckylotto", "omgword", "reverse", "slot", "money", "credits" };

        #region Fields

        readonly Bot bot;
        readonly string name;

        Game game;
        Timer timer;
        long startTime = 0;
        long triggerTime = 0;
        long floodTime = 0;

        #endregion

        #region Properties

        public String Name
        {
            get { return name; }
        }

        public long StartTime
        {
            get { return startTime; }
        }

        public string CurrentTime
        {
            get { return Convert.ToString((DateTime.Now.Ticks - startTime) / 10000000); }
        }

        #endregion

        #region Initialize

        public Channel(Bot bot, string name)
        {
            this.bot = bot;

            this.name = name;
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnTick);
        }

        #endregion


        #region Events

        /// <summary>
        /// Any message recieved in this channel
        /// </summary>
        public void OnMessage(object sender, IrcEventArgs e)
        {
            if (bot.isCommand(e, "help"))
                Help(e);

            else if (game == null && System.DateTime.Now.Ticks >= floodTime)
            {
                if (bot.isCommand(e, "blackjack"))
                    game = new Blackjack(bot, this, e);

                else if (bot.isCommand(e, "bomb"))
                    game = new Bomb(bot, this, e);

                else if (bot.isCommand(e, "dice"))
                    game = new Dice(bot, this, e);

                else if (bot.isCommand(e, "hangman"))
                    game = new Hangman(bot, this, e);

                else if (bot.isCommand(e, "luckylotto"))
                    game = new Luckylotto(bot, this, e);

                else if (bot.isCommand(e, "omgword"))
                    game = new Omgword(bot, this, e);

                else if (bot.isCommand(e, "reverse"))
                    game = new Reverse(bot, this, e);

                else if (bot.isCommand(e, "slot"))
                    game = new Slot(bot, this, e);

                else if (bot.isCommand(e, "money"))
                    Money(e);

                else if (bot.isCommand(e, "credits"))
                    Credits(e);

                /* Add your own game here - like this: */
                //else if (bot.isCommand(e, "exgame"))
                    //ExampleGame(e);

                if (game != null) 
                    //not instant like slot
                    game.Start();
            }

            else if (game != null)
                game.Update(e);
        }

        private void OnTick(object source, ElapsedEventArgs e)
        {
            if ((game != null) && (System.DateTime.Now.Ticks >= triggerTime))
            {
                game.TimeOut();
                timer.Enabled = false;
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Posts the Credits
        /// </summary>
        /// <param name="e"></param>
        private void Credits(IrcEventArgs e)
        {
            //PLEASE do not change or remove existing text. Feel free to add yourself though, if you have contributed :)
            bot.SendMessage(e.Data.Channel, "This bot was written by Marius Musch <http://mariuz.org> in C# " +
                "using the C# IRC libary \"SmartIrc4net\" by Mirco Bauer <http://meebey.net>.");
        }

        /// <summary>
        /// Sends all commands to the asking nick
        /// </summary>
        private void Help(IrcEventArgs e)
        {
            //building a list of all commands using the right prefix
            string tmp = "";
            for (int i = 0; i < commandlist.Length; i++)
                tmp += bot.Prefix + commandlist[i] + " ";

            SendNotice(e.Data.Nick, "I currently know the following commands: " + Msg.Bold(tmp));
        }

        /// <summary>
        /// Announces the current wealth of the given nick
        /// </summary>
        private void Money(IrcEventArgs e)
        {
            string hostmask = bot.GetHost(e.Data.Nick);
            if (bot.Users.ContainsKey(hostmask)) //existing player
            {
                bot.SendMessage(e.Data.Channel, "Hey " + Msg.Bold(e.Data.Nick) + " you have " +
                    Msg.Bold(bot.Users[hostmask].ToString()) + "$ - come and play with me!");
            }
            else //new player
            {
                bot.SendMessage(e.Data.Channel, "Hey " + Msg.Bold(e.Data.Nick) + ", each new player starts with " +
                    Msg.Bold(bot.Startmoney.ToString()) + "$ - give it a try and play with me!");
            }
        }

        #endregion


        #region Game Methods

        /// <summary>
        /// Ends the game
        /// </summary>
        public void DisposeGame()
        {
            game = null;
            StartFlood(2);
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        /// <param name="length">Time in seconds</param>
        public void StartTimer(long length)
        {
            startTime = DateTime.Now.Ticks;
            triggerTime = DateTime.Now.Ticks + (length * 10000000);
            timer.Enabled = true;
        }

        /// <summary>
        /// No games can be started in this channel for x seconds
        /// </summary>
        /// <param name="lenght">Time in seconds</param>
        public void StartFlood(int lenght)
        {
            floodTime = DateTime.Now.Ticks + (lenght * 10000000);
        }

        /// <summary>
        /// Sends the message prefixed with the name of the current game
        /// </summary>
        public void SendMessage(string message)
        {
            bot.SendMessage(name, Msg.Frame(game.ToString().Substring(game.ToString().IndexOf('.') + 1)) + " " + message);
        }

        /// <summary>
        /// Sends a notice to the given nick
        /// </summary>
        public void SendNotice(string nick, string message)
        {
            bot.SendNotice(nick, message);
        }

        #endregion

    }
}
