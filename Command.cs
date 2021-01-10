using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CommanderLibr
{
    public class Command
    {
        public string[] ExistingArgs { get; private set; }
        public string Name { get; private set; }
        public Commander Cmd { get; set; }

        public Command() {  }
        public Command(Commander cmd)
        {
            Cmd = cmd;
        }

        public void SetCommName(string commName)
        {
            Name = commName;
        }

        public void SetExistingArgs(string[] existingArgs)
        {
            ExistingArgs = existingArgs;
        }

        // Not used as it is now working as intented, ex: if the user wants to input a path for a file 
        // the command is not run as it does not recognize it as an argument
        public bool IsArgumentArrayCorrect(string[] args)
        {
            foreach (string arg in args)
            {
                if (!ExistingArgs.Contains(arg))
                {
                    Cmd.ConWriteLine($"Argument ({arg}) does not exist in the command, " +
                        $"type {Name} --help for more info");
                    return false;
                }
            }
            return true;
        }
    }
}
