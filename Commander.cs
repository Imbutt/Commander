using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Reflection;
using System.Linq;

namespace CommanderLibr
{
    public class Commander
    {
        private string commsPath = @"C:\Users\Andrea\source\repos\ImmagiBoop\ImmagiBoop\CommanderLibr\commands.json";
        List<Command> commands = new List<Command>();

        /// <summary>
        /// Used to initialize all sub command classes
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetDerivedTypesFor(Type baseType)
        {
            var assembly = Assembly.GetExecutingAssembly();

            return assembly.GetTypes()
                .Where(baseType.IsAssignableFrom)
                .Where(t => baseType != t);
        }

        /// <summary>
        /// Load commands
        /// </summary>
        public Commander()
        {
            Command cmd = new Command();
            IEnumerable<Type> bruh = GetDerivedTypesFor(cmd.GetType());
            Console.WriteLine(bruh);
            foreach(var b in bruh)
            {
                //var p = Activator.CreateInstance(b) as t;
                dynamic instance = Activator.CreateInstance(b, new object[] {});
                commands.Add(instance);

            }

            foreach(Command comm in commands)
            {
                Console.WriteLine(comm);
                //Console.WriteLine(comm.commString);
            }
            //jjk

        }

        public enum ConsoleType
        {
            CMD,
            OUTSIDE
        }

        /// <summary>
        /// Can be the default command line program or an outside custom one
        /// </summary>
        public void GetCommand(ConsoleType cType)
        {
            if(cType == ConsoleType.CMD)
                Console.ReadLine();

        }

        public void Start(ConsoleType cType)
        {
            bool loop = true;

            while(loop)
            {
                
                // Get command
                GetCommand(cType);
                



                
            }
        }
    }
}
