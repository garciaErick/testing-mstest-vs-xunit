using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class ExampleGame : Game
    {
        /* To acutally implement the game you also have to go into
         * Channel.cs -> Region #Events -> OnMessage
         * and "register" the command there:
         * 
         * else if (bot.isCommand(e, "exgame"))
         *    ExampleGame(e);
         *    
         * Now you could start it by saying "[prefix]exgame" in a channel
         * 
         * You should also add the command to the string[] commandlist in Channel.cs
         * Only then it gets printed out by "[prefix]help"
         */

        /*
         * To see how to make an acutal game just look at the games included.
         * For example the code of "Reverse.cs" is fairly easy.
         */

        /// <summary>
        /// Construcor
        /// </summary>
        public ExampleGame(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            //You shouldn't do anything here except initializing variables
        }

        /// <summary>
        /// The game has been constructed and starts now
        /// </summary>
        public override void Start()
        {
             //Send out messages (and start the timer if necessary)
             
             //Your game can also end here (like slot),
             //just don't start a timer and dispose at the end of this method
        }
        
        /// <summary>
        ///  A message was written in the channel the game is running
        /// </summary>
        /// <param name="e"></param>
        public override void Update(IrcEventArgs e)
        {
            //Here you can check all stuff that happends during the game,
            //for example if the message contains the solution for this game (like hangman)
        }

        /// <summary>
        /// The Timer is triggered
        /// </summary>
        public override void TimeOut()
        {
            //Here you can add anything that should happen after the time is over,
            //for example if nobody knows the correct answer within 20 seconds we end the game
        }
    }
}