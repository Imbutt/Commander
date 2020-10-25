using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CommanderLibr;

namespace CommanderLibr.Commands
{
    /// <summary>
    /// This class has to go inside the Commands folder in order to be read by the Commander and work properly
    /// </summary>
    class commandTemplate : Command
    {
        public commandTemplate(Commander cmd) : base(cmd)
        {
            SetCommName(this.GetType().Name);
            string[] existingArgs = new string[]
            {
                // List of arguments

            };

            SetExistingArgs(existingArgs);
        }

        
        public void Call()
        {
            // do stuff    
        }

        public void Call(string[] args)
        {
            if(IsArgumentArrayCorrect(args))
            {
                // do stuff
            }
        }
    }
}
