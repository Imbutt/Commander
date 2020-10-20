using System;
using System.Collections.Generic;
using System.Text;
using CommanderLibr;

namespace CommanderLibr.Commands
{
    class commandTemplate : Command
    {
        public string commName { get; set; }
        public commandTemplate()
        {
            commName = this.GetType().Name;
        }


        public void Call(Commander cmd)
        {

        }
        public void Call(Commander cmd, string[] args)
        {

        }

    }
}
