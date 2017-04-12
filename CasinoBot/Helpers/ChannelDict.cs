using System;
using System.Collections;

namespace CasinoBot
{
    class ChannelDict : DictionaryBase
    {

        public ChannelDict()
        {
        }

        public void Add(string key, Channel chan)
        {
            Dictionary.Add(key, chan);
        }

        public bool Contains(string key)
        {
            return Dictionary.Contains(key);
        }

        public Channel this[String key]
        {
            get
            {
                return ((Channel)Dictionary[key]);
            }
            set
            {
                Dictionary[key] = value;
            }
        }

        public void Remove(string key)
        {
            Dictionary.Remove(key);
        }
    }
}
