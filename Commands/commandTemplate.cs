using System;
using System.Collections.Generic;
using System.Linq;
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
        public commandTemplate()
        {
            SetCommName(this.GetType().Name);
            string[] existingArgs = new List<string>() 
            {
                // List of arguments
                

            }.ToArray();
            SetExistingArgs(existingArgs);
        }

        
        public void Call(Commander cmd)
        {
            // do stuff    
        }

        public void Call(Commander cmd, string[] args)
        {
            if(IsArgumentArrayCorrect(args))
            {
                // do stuff
            }
        }
    }
}
