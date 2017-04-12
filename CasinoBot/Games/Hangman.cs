using Meebey.SmartIrc4net;

namespace CasinoBot
{
    class Hangman : Game
    {
        int lifes = 10;
        int time = 60;
        int prize = 30;

        string solution;
        bool[] correct;
        char[] chars;
        int count = 0;

        public Hangman(Bot bot, Channel chan, IrcEventArgs e)
            : base(bot, chan)
        {
            solution = bot.RandomWord;
            chars = solution.ToCharArray();
            correct = new bool[solution.Length];
        }

        public override void Start()
        {
            chan.StartTimer(time);
            chan.SendMessage("You have " + time + " seconds to find the following word: " + Msg.Bold(GenerateWord()) +
                " but don't forget, you have only " + lifes + " lifes.");
        }

        public override void Update(IrcEventArgs e)
        {
            if (e.Data.Message.Length == 1)
            {
                bool changed = false;

                //check each letter of the solution
                for (int i = 0; i < solution.Length; i++)
                {
                    if (chars[i] == char.Parse(e.Data.Message) && correct[i] == false)
                    {
                        correct[i] = true;
                        changed = true;
                        count++;
                    }
                }

                if (changed) //correct letter
                {
                    if (count == solution.Length) //final letter
                        Over(e.Data.Nick);
                    else //any letter
                        chan.SendMessage(Msg.Bold(e.Data.Message) + " is correct! " + Msg.Bold(GenerateWord()) + " Lifes left: " + lifes);
                }
                else
                {
                    lifes--;
                    if (lifes == 0)
                    {
                        chan.SendMessage("Game over! " + Msg.Bold(solution) + " would have been the solution.");
                        chan.DisposeGame();
                    }
                    else
                        chan.SendMessage(Msg.Bold(e.Data.Message) + " is wrong or was already guessed. Lifes left: " + lifes);
                }
            }

            else if (e.Data.Message == solution)
                Over(e.Data.Nick);
        }

        public override void TimeOut()
        {
            chan.SendMessage("Time is over! The word we looked for was " + Msg.Bold(solution));
            chan.DisposeGame();
        }

        private void Over(string nick)
        {
            int earned = bot.UpdateMoney(bot.GetHost(nick), prize + lifes + solution.Length, int.Parse(chan.CurrentTime), time);
            chan.SendMessage(nick + " has found the solution: " + Msg.Bold(solution) + " and wins " + earned + "$");
            chan.DisposeGame();
        }

        private string GenerateWord()
        {
           
            string word = "";

            for (int i = 0; i < solution.Length; i++)
            {
                if (correct[i])
                    word += chars[i];
                else
                    word += "_";
            }

            return word;
        }
    }
}