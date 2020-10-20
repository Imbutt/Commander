using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommanderLibr
{
    public class Command : Attribute
    {
        public string[] ExistingArgs { get; private set; }
        public string CommName { get; private set; }

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
                    return true;
            }
            return false;
        }
    }
}
