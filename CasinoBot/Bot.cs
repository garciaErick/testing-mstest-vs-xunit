#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Meebey.SmartIrc4net;

#endregion

namespace CasinoBot
{
    class Bot
    {   

        #region Fields

        Main main;
        IrcClient irc;
        Thread listen;
        ChannelDict chans;
        Hashtable flood = new Hashtable();
        Random rnd;
        string listener = "";
        const int startmoney = 500;

        //from Main
        Hashtable users;
        List<string> wordlist;
        string[] perform;
        string prefix;
        string pwd;
        bool qnet;
        bool raw;
        
        #endregion

        #region Properties

        public Hashtable Users
        {
            get { return users; }
        }

        public Random Random
        {
            get { return rnd; }
        }

        public string Prefix
        {
            get { return prefix; }
        }

        public string RandomWord
        {
           get
            {
                string word;
                do
                {
                    word = wordlist[rnd.Next(0, wordlist.Count)].TrimEnd();
                }  while (word.Length < 6);
                return word;
            }
        }

        public bool IsConnected
        {
            get { return irc.IsConnected; }
        }

        public int Startmoney
        {
            get { return startmoney; }
        }

        #endregion

        #region Initialize

        public Bot(Main main)
        {
            this.main = main;

            irc = new IrcClient();
            listen = new Thread(new ThreadStart(irc.Listen));

            rnd = new Random(DateTime.Now.Millisecond);
            chans = new ChannelDict();
        }

        #endregion

        #region Connect/Disconnect

        public void Connect(string server, int port, string channels, string[] botnames, string name, string prefix, int delay, bool qnet,
            string pwd, bool raw, string[] perform, Hashtable users, List<string> wordlist)
        {
            this.prefix = prefix;
            this.qnet = qnet;
            this.pwd = pwd;
            this.raw = raw;
            this.perform = perform;
            this.users = users;
            this.wordlist = wordlist;

            irc.Encoding = System.Text.Encoding.UTF8;
            irc.SendDelay = delay;

            //required by GetHost()
            irc.ActiveChannelSyncing = true;

            irc.OnRawMessage += new IrcEventHandler(OnRawMessage);
            irc.OnQueryMessage += new IrcEventHandler(OnQueryMessage);
            irc.OnChannelMessage += new IrcEventHandler(OnChannelMessage);

            irc.OnError += new Meebey.SmartIrc4net.ErrorEventHandler(OnError);
            irc.OnJoin += new JoinEventHandler(OnJoin);
            irc.OnPart += new PartEventHandler(OnPart);
            irc.OnKick += new KickEventHandler(OnKick);
            irc.OnDisconnected += new EventHandler(OnDisconnected);
            
            try
            {
                irc.Connect(server, port);
            }
            catch (ConnectionException f)
            {
                Log("<ERROR> " + f.Message);
#if DEBUG
                Log("<EXCEPTION> " + f.StackTrace);
#endif
            }

            try
            {
                irc.Login(botnames, name);
                irc.RfcJoin(channels);

                listen.Start();
                OnConnected();
            }
            catch (ConnectionException)
            {
            }
            catch (Exception f)
            {
                Log("<ERROR> " + f.Message);
#if DEBUG
                Log("<EXCEPTION> " + f.StackTrace);
#endif
            }
        }

        public void Disconnect()
        {
            if (irc.IsConnected == true)
                irc.Disconnect();
        }

        #endregion


        #region Event Handlers

        /// <summary>
        /// ANY Message recieved
        /// </summary>
        private void OnRawMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Message != null && e.Data.Channel == null && e.Data.Nick == null) //Server-Msg
                //antispam ;)
                if (!(e.Data.Type == ReceiveType.Motd) && !(e.Data.Type == ReceiveType.Who) && !(e.Data.Type == ReceiveType.BanList)
                    && !(e.Data.Message == irc.Address))
                    InvokeLog("<Server> " + e.Data.Message);
        }

        /// <summary>
        /// All Messages recieved in a Channel
        /// </summary>
        private void OnChannelMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Message.Contains(irc.Nickname))
                InvokeLog("<Highlight - " + e.Data.Nick + "> " + e.Data.Message);
            else
                InvokeLog("<" + e.Data.Channel + " - " + e.Data.Nick + "> " + e.Data.Message);

            if (!(flood.Contains(e.Data.Nick)) || (long)flood[e.Data.Nick] <= DateTime.Now.Ticks)
            {
                chans[e.Data.Channel].OnMessage(sender, e); //forward to Channel
                flood[e.Data.Nick] = DateTime.Now.Ticks + 10000000;
            }     
        }

        /// <summary>
        /// All Messages recieved in a Query
        /// </summary>
        private void OnQueryMessage(object sender, IrcEventArgs e)
        {
            InvokeLog("<Query - " + e.Data.Nick + "> " + e.Data.Message);
            QueryCommands(e);
        }

        private void OnJoin(object sender, IrcEventArgs e)
        {
            if (irc.IsMe(e.Data.Nick))
            {
                if (chans.Contains(e.Data.Channel))
                    chans[e.Data.Channel] = new Channel(this, e.Data.Channel);
                else
                    chans.Add(e.Data.Channel, new Channel(this, e.Data.Channel));

                InvokeLog("<System> Joined " + e.Data.Channel);
            }
            else
            {
                string hostmask = GetHost(e.Data.Nick);
                if (users.ContainsKey(hostmask))
                {
                    SendNotice(e.Data.Nick, "Welcome back to " + e.Data.Channel + ", you currently have "
                        + users[hostmask] + "$! For a list of all available commands  type " + prefix + "help :)");
                }
                else
                {
                    SendNotice(e.Data.Nick, "Welcome to " + e.Data.Channel + " I'm the casino-bot, come and play with me :D " +
                    "Each player starts out with 500$. For a list of all available commands type " + prefix + "help");
                }
            }
        }

        private void OnPart(object sender, IrcEventArgs e)
        {
            if (irc.IsMe(e.Data.Nick))
            {
                chans.Remove(e.Data.Channel);
                InvokeLog("<System> Parted " + e.Data.Channel);
            }
        }

        private void OnKick(object sender, IrcEventArgs e)
        {
            if (irc.IsMe(e.Data.Nick))
            {
                chans.Remove(e.Data.Channel);
                InvokeLog("<System> Kicked from " + e.Data.Channel);
            }
        }

        private void OnConnected()
        {
            InvokeLog("<System> Connected");

            foreach (string s in perform)
                irc.WriteLine(s);
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            chans.Clear();
            listen.Abort();
            Log("<System> Disconnected");
        }

        private void OnError(object sender, Meebey.SmartIrc4net.ErrorEventArgs e)
        {
            InvokeLog("<ERROR> " + e.ErrorMessage);
            Disconnect();
        }

        #endregion

        #region Query Commands

        /// <summary>
        /// Triggered by OnQueryMessage
        /// </summary>
        /// <param name="e"></param>
        public void QueryCommands(IrcEventArgs e)
        {
            string[] msgArray = e.Data.MessageArray;
            string host;

            if (msgArray[0] == (prefix + pwd) && msgArray.Length >= 3)
            {
                switch (msgArray[1])
                {
                    case "join":
                        if (msgArray.Length == 4)
                            irc.RfcJoin(msgArray[2], msgArray[3]); //if channelmode +k
                        else
                            irc.RfcJoin(msgArray[2]);
                        listener = e.Data.Nick;
                        break;

                    case "part":
                        irc.RfcPart(msgArray[2]);
                        listener = e.Data.Nick;
                        break;

                    case "msg":
                        if (msgArray.Length >= 4)
                            SendMessage(msgArray[2], e.Data.Message.Substring(e.Data.Message.IndexOf(msgArray[3])));
                        else
                            SendNotice(e.Data.Nick, "Invalid syntax. Use \"msg <channel> <message>\"");
                        break;

                    case "amsg":
                        foreach (DictionaryEntry d in chans)
                            SendMessage(d.Key.ToString(), e.Data.Message.Substring(e.Data.Message.IndexOf(msgArray[2])));
                        break;

                    case "whois":
                        host = GetHost(msgArray[2]);
                        if (host != null)
                            SendNotice(e.Data.Nick, msgArray[2] + " is [" + host + "] and has " + users[host] + "$.");
                        else
                            SendNotice(e.Data.Nick, "Nick not found.");
                        break;

                    case "setmoney":
                        if (msgArray.Length >= 4)
                        {
                            if (GetHost(msgArray[2]) != null)
                            {
                                host = GetHost(msgArray[2]);
                                if (users.ContainsKey(host))
                                {
                                    SendNotice(e.Data.Nick, "Money for " + msgArray[2] + "[" + host + "] changed from " +
                                        users[host] + "$ to " + msgArray[3] + "$");
                                    users[host] = msgArray[3];
                                }
                                else
                                {
                                    SendNotice(e.Data.Nick, "User " + msgArray[2] + "[" + host + "] added with " + msgArray[3] + "$");
                                    users.Add(host, msgArray[3]);
                                }
                            }
                            else
                                SendNotice(e.Data.Nick, "Nick not found");
                        }
                        else
                            SendNotice(e.Data.Nick, "Invalid syntax. Use \"setmoney <nick> <value>\"");
                        break;

                    default:
                        if (raw)
                        {
                            irc.WriteLine(e.Data.Message.Substring(e.Data.Message.IndexOf(msgArray[1])));
                            listener = e.Data.Nick;
                        }
                        else
                            SendNotice(e.Data.Nick, "Unknown command (raw is disabled). Valid commands are " + prefix + "join/part/msg/amsg/whois/setmoney");
                        break;
                }
            }
        }

        #endregion


        #region Simple Methods

        /// <summary>
        /// If the message starts with the given command
        /// </summary>
        /// <param name="e"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool isCommand(IrcEventArgs e, string command)
        {
            return (e.Data.MessageArray[0].ToLower() == prefix + command);
        }

        public void SendNotice(string nick, string message)
        {
            irc.SendMessage(SendType.Notice, nick, message);
        }

        public void SendMessage(string target, string message)
        {
            irc.SendMessage(SendType.Action, target, message);
        }

        public void SendRaw(string message)
        {
            irc.WriteLine(message);
        }

        public void Log(string text)
        {
            main.Log(text);
        }

        public void InvokeLog(string text)
        {
            //notify the result if someone used a query command
            if (!(listener.Equals("")) && (text.StartsWith("<Server>") || text.StartsWith("<System>")))
            {
                SendNotice(listener, text);
                listener = "";
            }

            main.InvokeLog(text);
        }

        #endregion

        #region Complex Methods

        /// <summary>
        /// Changes the players money based on the formula: v - [v * (x1/x2)] + x
        /// </summary>
        public int UpdateMoney(string hostmask, int v, int x1, int x2)
        {
            v += Random.Next(0, 6) - (v * x1 / x2);

            UpdateMoney(hostmask, v);
            return v;
        }

        /// <summary>
        /// Changes the players money by the given value
        /// (positive = win, negative = loss)
        /// </summary>
        public void UpdateMoney(string hostmask, int value)
        {
            //existing or new player
            if (users.ContainsKey(hostmask))
                users[hostmask] = Convert.ToInt32(users[hostmask]) + value;
            else
                users.Add(hostmask, startmoney + value);
        }

        /// <summary>
        /// Returns the hostmask to identify given player
        /// (requires ActiveChannelSyncing)
        /// </summary>
        public string GetHost(string nick)
        {
            if (qnet == true) //usermode +x
                return irc.GetIrcUser(nick).Host;  //returns xyz.users.quakenet.org
            else
            {
                if (irc.GetIrcUser(nick) != null)
                {
                    string host = irc.GetIrcUser(nick).Host;
                    return irc.GetIrcUser(nick).Ident.Replace("~", "") + "-" + host.Substring(host.IndexOf('.') + 1); //returns smith.dip.t-dailin.net
                }
                else
                    return null;
            }
        }


        #endregion

    }
}
