using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace CommanderLibr
{
    public class Command : Attribute
    {
        public string[] ExistingArgs { get; private set; }
        public string CommName { get; private set; }
        public Commander Cmd { get; set; }

        public Command() {  }
        public Command(Commander cmd)
        {
            Cmd = cmd;
        }

        public void SetCommName(string _CommName)
        {
            CommName = _CommName;
        }

        public void SetExistingArgs(string[] existingArgs)
        {
            ExistingArgs = existingArgs;
        }

        public bool IsArgumentArrayCorrect(string[] args)
        {
            foreach (string arg in args)
            {
                if (!ExistingArgs.Contains(arg))
                {
                    Cmd.ConWriteLine($"Argument ({arg}) does not exist in the command, " +
                        $"type {CommName} --help for more info");
                    return false;
                }
            }
            return true;
        }
    }
}
