using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Reflection;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CommanderLibr.Commands;

namespace CommanderLibr
{
    public class Commander
    {

        Dictionary<string, object> commandDict = new Dictionary<string, object>(); // Loaded commands
        public ConsoleType cType { get;}
        string writeStart = "#";
        public static string argFilePath = Directory.GetCurrentDirectory() + @"\Commands.txt";

        public enum ConsoleType
        {
            CMD,
            OUTSIDE
        }


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
        /// Load all commands in the command list and load the type of console
        /// </summary>
        public Commander(ConsoleType _cType)
        {
            // Set the console type
            cType = _cType;

            // Check for the command arguments file
            if(!File.Exists(argFilePath))
                File.Create(argFilePath);

            // Get all the subclasses of Command
            Command cmd = new Command();
            IEnumerable<Type> bruh = GetDerivedTypesFor(cmd.GetType());

            // Initialize every subclass of Command and add it to the commands list
            foreach(var b in bruh)
            {
                dynamic instance = Activator.CreateInstance(b, new object[] {});
                commandDict.Add(instance.GetType().Name, instance);
            }
        }

        public void Start()
        {
            bool loop = true;

            while(loop)
            {

                // Get command
                string fullCommString = ConRead();
                string commString;
                if (fullCommString.Contains(' '))
                {
                    int firstWhiteSpace = fullCommString.IndexOf(' ', 0);
                    commString = fullCommString.Substring(0, firstWhiteSpace);
                }
                else
                    commString = fullCommString;

                // Find command in commandDict
                var commFound = commandDict.TryGetValue(commString,out dynamic command);
                if (commFound != false)
                {
                    if (fullCommString.Contains(' '))
                    {
                        // Get all arguments
                        int firstWhiteSpace = fullCommString.IndexOf(' ', 0);
                        string[] args = fullCommString
                            .Substring(firstWhiteSpace, fullCommString.Length - commString.Length)
                            .Split(' ')
                            .Where(x => !x.Equals(String.Empty))
                            .ToArray();

                        // Call the command with the arguments
                        command.Call(this,args);
                    }
                    else
                        command.Call(this); // Call the command
                }
                else
                    ConWrite("Command does not exist");
            }
        }

        /// <summary>
        /// Equivalent of Console.Write but with outside console in mind
        /// </summary>
        /// <param name="_string"></param>
        public void ConWrite(string _string)
        {
            if (cType == ConsoleType.CMD)
                Console.Write(_string);
            else
                throw new NotImplementedException(); // TODO: OUTSIDE CONSOLE
        }

        /// <summary>
        /// Equivalent of Console.WriteLine but with outside console in mind
        /// </summary>
        /// <param name="_string"></param>
        public void ConWriteLine(string _string)
        {
            if (cType == ConsoleType.CMD)
                Console.WriteLine(_string);
            else
                throw new NotImplementedException(); // TODO: OUTSIDE CONSOLE
        }

        /// <summary>
        ///  Equivalent of Console.ReadLine but with outside console in mind
        /// </summary>
        /// <returns></returns>
        public string ConRead()
        {
            // Don't accept the string if it's empty
            string _string = String.Empty;
            do
            {
                ConWrite(writeStart + " "); // Character that shows the console is taking input

                if (cType == ConsoleType.CMD)
                    _string = Console.ReadLine();
                else
                    throw new NotImplementedException(); // TODO: OUTSIDE CONSOLE

            } while (string.IsNullOrWhiteSpace(_string));

            return _string;

        }

    }
}
