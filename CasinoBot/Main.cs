#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;

#endregion

namespace CasinoBot
{
    public partial class Main : Form
    {

        #region Fields

        Bot bot;

        List<string> wordlist = new List<string>(); // no array because lenght of wordlist is unknown before runtime
        Hashtable users = new Hashtable(); //key:<str>hostmask | value:<int>money

        #endregion

        #region Form Events

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadConfig();
            LoadFiles();
            Thread.CurrentThread.Name = "Main";
            //button1_Click(null, null); //quickstart :)
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bot != null)
                bot.Disconnect();

            SaveConfig();
            SaveFiles();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
            {
                bot = new Bot(this);

                string[] botnames = { txtNick.Text, txtAlt.Text };
                string[] perform = txtPerform.Text.Split('\n');

                bot.Connect(txtServer.Text, Convert.ToInt32(txtPort.Text), txtChannel.Text, botnames, txtReal.Text, txtPrefix.Text,
                    Convert.ToInt32(txtDelay.Text), rdoQNet.Checked, txtPass.Text , chkRaw.Checked, perform, users, wordlist);

                btnConnect.Text = "Disconnect";
                btnConfig.Enabled = false;
            }
            else
            {
                if (bot != null && bot.IsConnected == true)
                    bot.Disconnect();

                btnConnect.Text = "Connect";
                btnConfig.Enabled = true;
            }

        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            if (btnConfig.Text == "Config")
            {
                btnConnect.Visible = false;
                tabControl1.Enabled = true;
                Size = new Size(Size.Width, Size.Height + 350);
                btnConfig.Text = "Save and Close";
            }
            else
            {
                Size = new Size(Size.Width, Size.Height - 350);
                tabControl1.Enabled = false;
                btnConnect.Visible = true;
                btnConfig.Text = "Config";
            }
        }

        private void lblFilter_Click(object sender, EventArgs e)
        {
            txtImportant.Visible = !txtImportant.Visible;
            txtLog.Visible = !txtLog.Visible;

            if (lblFilter.Text == "Important")
                lblFilter.Text = "All";
            else
                lblFilter.Text = "Important";
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (bot != null && bot.IsConnected)
                {
                    bot.SendRaw(txtInput.Text);
                    txtInput.Text = "";
                }
                else
                    Log("<System> You are not connected!");
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                ShowInTaskbar = false;
            else
                ShowInTaskbar = true;
        }

        #endregion

        #region I/O

        void LoadFiles()
        {
            //generating word-array from file, slow startup but faster runtime
            try
            {
                string line;
                StreamReader sr = new StreamReader("wordlist.txt");

                while ((line = sr.ReadLine()) != null)
                    wordlist.Add(line);

                sr = new StreamReader("perform.txt");
                while ((line = sr.ReadLine()) != null)
                {
                    string[] lines = line.Split('\n');
                    foreach (string s in lines)
                        txtPerform.AppendText(line + "\n");
                }
            }
            catch (IOException e)
            {
                Log("<System> " + e.Message);
            }
        }

        void SaveFiles()
        {
            string[] perform = txtPerform.Text.Split('\n');

            try
            {
                StreamWriter sw = new StreamWriter("perform.txt");
                sw.Write(txtPerform.Text);
                sw.Close();
            }
            catch (IOException e)
            {
                Log("<System> " + e.Message);
            }
        }

        void SaveConfig()
        {
            try
            {
                XmlWriterSettings o = new XmlWriterSettings();
                o.Indent = true;
                o.IndentChars = "   ";

                XmlWriter w = XmlWriter.Create("config.xml", o);
                w.WriteStartDocument();
                w.WriteStartElement("Config");
                //Connection
                w.WriteStartElement("Connection");
                w.WriteElementString("Server", txtServer.Text);
                w.WriteElementString("Port", txtPort.Text);
                w.WriteElementString("Channel", txtChannel.Text);
                w.WriteEndElement();
                //Bot
                w.WriteStartElement("Bot");
                w.WriteElementString("Nick", txtNick.Text);
                w.WriteElementString("Alternative", txtAlt.Text);
                w.WriteElementString("Ident", txtReal.Text);
                w.WriteEndElement();
                //Settings
                w.WriteStartElement("Settings");
                w.WriteElementString("Prefix", txtPrefix.Text);
                w.WriteElementString("Delay", txtDelay.Text);
                if (rdoStandard.Checked == true)
                    w.WriteElementString("Hostmask", "0");
                else
                    w.WriteElementString("Hostmask", "1");
                w.WriteElementString("Masterpwd", txtPass.Text);
                if (chkRaw.Checked == false)
                    w.WriteElementString("Raw", "0");
                else
                    w.WriteElementString("Raw", "1");
                w.WriteEndElement();
                //Users
                w.WriteStartElement("Users");
                foreach (DictionaryEntry de in users)
                {
                    w.WriteElementString("User", de.Key.ToString() + "=" + de.Value.ToString());
                }
                w.WriteEndElement();
                w.WriteEndElement();
                w.Flush();
                w.Close();
            }
            catch (IOException e)
            {
                Log("<System> " + e.Message);
            }
        }

        void LoadConfig()
        {
            try
            {
                XmlReader r = XmlReader.Create("config.xml");
                string[] temp = new string[1];
                while (r.Read())
                {
                    switch (r.Name)
                    {
                        case "Server":
                            txtServer.Text = r.ReadInnerXml();
                            break;
                        case "Port":
                            txtPort.Text = r.ReadInnerXml();
                            break;
                        case "Channel":
                            txtChannel.Text = r.ReadInnerXml();
                            break;
                        case "Nick":
                            txtNick.Text = r.ReadInnerXml();
                            break;
                        case "Alternative":
                            txtAlt.Text = r.ReadInnerXml();
                            break;
                        case "Ident":
                            txtReal.Text = r.ReadInnerXml();
                            break;
                        case "Prefix":
                            txtPrefix.Text = r.ReadInnerXml();
                            break;
                        case "Delay":
                            txtDelay.Text = r.ReadInnerXml();
                            break;
                        case "Hostmask":
                            if (r.ReadInnerXml() == "0")
                                rdoStandard.Checked = true;
                            else
                                rdoQNet.Checked = true;
                            break;
                        case "Masterpwd":
                            txtPass.Text = r.ReadInnerXml();
                            break;
                        case "Raw":
                            if (r.ReadInnerXml() == "0")
                                chkRaw.Checked = false;
                            else
                                chkRaw.Checked = true;
                            break;
                        case "User":
                            temp = r.ReadInnerXml().Split('=');
                            users.Add(temp[0], temp[1]);
                            break;
                    }
                }
                r.Close();
            }
            catch (IOException e)
            {
                Log("<System> " + e.Message);
            }
        }

        #endregion

        #region Public Methods

        public void Log(string text)
        {
            txtLog.Text = "<" + DateTime.Now.Hour + ":" + DateTime.Now.Minute + "> " + text + "\r\n" + txtLog.Text;

            if (text.StartsWith("<Server>") || text.StartsWith("<System>") || text.StartsWith("<ERROR>")  || text.StartsWith("<Query -") || text.StartsWith("<Highlight -"))
                txtImportant.Text = "<" + DateTime.Now.ToString("HH:mm") + "> " + text + "\r\n" + txtImportant.Text;
        }

        public void InvokeLog(string text)
        {

            MethodInvoker TextUpdate = delegate
            {
                Log(text);
            };
            Invoke(TextUpdate);
        }

        #endregion

    }
}
