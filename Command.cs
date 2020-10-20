using System;
using System.Collections.Generic;
using System.Text;

namespace CommanderLibr
{
    public class Command : Attribute
    {

        public Command() { }

        public void Call(Commander cmd)
        {
            cmd.ConWriteLine("defualt command");
        }
        public void Call(Commander cmd,string[] args)
        {
            cmd.ConWriteLine("default command");
            foreach(var b in args)
            {
                cmd.ConWriteLine(b);
            }
        }
    }
}
