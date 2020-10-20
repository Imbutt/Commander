using System;
using System.Collections.Generic;
using System.Text;
using CommanderLibr;

namespace CommanderLibr.Commands
{
    class getpixel : Command
    {
        public string commName { get; set; }
        public getpixel()
        {
            commName = this.GetType().Name;
        }


        public void Call(Commander cmd)
        {
            cmd.ConWriteLine("ee");
        }
        
        public void Call(Commander cmd, string[] args)
        {
            foreach(var b in args)
            {
                cmd.ConWriteLine(b);
            }
        }





    }
}
