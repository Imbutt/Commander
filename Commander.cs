using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Reflection;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CommanderLibr.Commands;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CommanderLibr
{
    public class Commander
    {

        Dictionary<string, object> commandDict = new Dictionary<string, object>(); // Loaded commands
        public ConsoleType cType { get;}
        string writeStart = "#";
        public string HelpFilePath 
        { 
            get => Directory.GetCurrentDirectory() + @"\help.txt";
        }
        public string TempHelpFilePath
        {
            get => Directory.GetCurrentDirectory() + @"\tempHelp.txt";
        }

        public enum ConsoleType
        {
            CMD,
            OUTSIDE
        }

        public enum MessType
        {
            DEBUG,
            ERROR,
            WARNING
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

            // Get all the subclasses of Command
            Command cmd = new Command();
            IEnumerable<Type> bruh = GetDerivedTypesFor(cmd.GetType());

            // Initialize every subclass of Command and add it to the commands list
            foreach (var b in bruh)
            {
                dynamic instance = Activator.CreateInstance(b, this);
                commandDict.Add(instance.GetType().Name, instance);
            }

#if (DEBUG)

            // If the solution config is set to DEBUG run this code
            ConWriteLine("DEBUG mode is active",MessType.DEBUG);

            // Get the help.txt from the repos to the bin folder
            string reposHelpFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName
                + "/Commander/help.txt";
            if (File.Exists(reposHelpFilePath))
            {
                if (File.Exists(HelpFilePath))
                {
                    // If the help.txt inside the bin is different from the repos one substitute it
                    bool areEqual = File.ReadLines(reposHelpFilePath).SequenceEqual(
                        File.ReadLines(HelpFilePath));
                    if (!areEqual)
                    {
                        File.Delete(HelpFilePath);
                        File.Copy(reposHelpFilePath, HelpFilePath);
                        ConWriteLine("Replaced the bin help.txt with the updated repos one", MessType.DEBUG);
                        ConWriteLine($"{reposHelpFilePath} to {HelpFilePath}", MessType.DEBUG);
                    }
                }
                else
                {
                    File.Copy(reposHelpFilePath, HelpFilePath);
                    ConWriteLine("Copied the repos help.txt inside the bin folder", MessType.DEBUG);
                }
            }
            else
            {
                if (!File.Exists(HelpFilePath))
                {
                    ConWriteLine("Could not find the repos help.txt neither the bin one, Creating an error help.txt file", MessType.ERROR);
                    using (StreamWriter sw = File.CreateText(HelpFilePath))
                    {
                        sw.WriteLine("This help.txt file has been automatically generated because it was not found" +
                            "when the program was started");
                    }
                }
                else
                {
                    ConWriteLine("Repos help.txt file not found, maybe it got accidentally deleted? Creating empty one",MessType.WARNING);
                    File.Create(reposHelpFilePath);
                }
            }
#endif

            // If help.txt isn't found create an error help.txt
            if (!File.Exists(HelpFilePath))
            {
                ConWriteLine("help.txt not found, Creating error help.txt file", MessType.ERROR);
                using (StreamWriter sw = File.CreateText(HelpFilePath))
                {
                    sw.WriteLine("This help.txt file has been automatically generated because it was not found" +
                        "when the program was started");
                }
            }
            else
            {
                // Check if all commands and arguments are inside the help.txt file

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
                        command.Call(args);
                    }
                    else
                        command.Call(); // Call the command
                }
                else
                    ConWrite("Command does not exist");
            }
        }
        
        /// <summary>
        /// Creates a template help.txt file getting all the commands and their arguments 
        /// </summary>
        /// <param name="path"> Path to output the file, could also be another name like tempHelp.txt </param>
        public void CreateHelpFile(string path)
        {
            if(commandDict.Count != 0)
            {
                List<object> commands = commandDict.Values.ToList();
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (dynamic comm in commands)
                    {
                        sw.WriteLine($"{comm.Name}:");
                        sw.WriteLine("\t" + $"args:");
                        foreach(string arg in comm.ExistingArgs)
                        {
                            sw.WriteLine("\t\t" + $"-{arg}:");
                        }
                    }
                }
            }
        }


        public void CheckHelpFile(string path)
        {
            // TODO: Finish function
            if (commandDict.Count != 0)
            {
                List<object> commands = commandDict.Values.ToList();
                string text = File.ReadAllText(path);
                foreach (dynamic comm in commands)
                {
                    Regex r = new Regex($"{comm.Name}:");
                    var b = r.Match(text).Index;
                }
            }
        }


        /// <summary>
        /// Equivalent of Console.Write but with outside console in mind
        /// </summary>
        /// <param name="_string"> String to write to console </param>
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
        /// <param name="_string"> String to write to console with endline </param>
        public void ConWriteLine(string _string)
        {
            if (cType == ConsoleType.CMD)
                Console.WriteLine(_string);
            else
                throw new NotImplementedException(); // TODO: OUTSIDE CONSOLE
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_string"> String to write to console </param>
        /// <param name="mt"> Type colored message before the string </param>
        public void ConWriteLine(string _string, MessType mt)
        {
            if (cType == ConsoleType.CMD)
            {
                ConsoleColor cmdColor;
                string messString;

                // Get the first thing and its color
                switch(mt)
                {
                    case MessType.DEBUG:
                        cmdColor = ConsoleColor.Cyan;
                        messString = "DEBUG: ";
                        break;
                    case MessType.ERROR:
                        cmdColor = ConsoleColor.Red;
                        messString = "ERROR: ";
                        break;
                    case MessType.WARNING:
                        cmdColor = ConsoleColor.DarkYellow;
                        messString = "WARNING: ";
                        break;
                    default:
                        // This should not happen
                        cmdColor = ConsoleColor.White;
                        messString = "WHAT: ";
                        break;
                }
                
                // Type it
                Console.ForegroundColor = cmdColor;
                Console.Write(messString);
                Console.ResetColor();   // RESET THE COLOR

                Console.WriteLine(_string);
            }

            else
                throw new NotImplementedException(); // TODO: OUTSIDE CONSOLE
        }

        /// <summary>
        ///  Equivalent of Console.ReadLine but with outside console in mind
        /// </summary>
        /// <returns> Returns the equivalent string of Console.ReadLine </returns>
        public string ConRead()
        {
            // Don't accept the string if it's empty
            string _string = String.Empty;


            do
            {
                if (cType == ConsoleType.CMD)
                {
                    
                    ConWrite(writeStart + " "); // Character that shows the console is taking input
                    _string = Console.ReadLine();
                }
                else
                    throw new NotImplementedException(); // TODO: OUTSIDE CONSOLE

            } while (string.IsNullOrWhiteSpace(_string));

            return _string;
        }

        public string ConReadCheckInput(string[] acceptedInputs)
        {
            string input;
            do
            {
                input = ConRead();
            }
            while (!acceptedInputs.Contains(input));

            return input;
        }

        public bool ConReadBool()
        {
            string[] positiveInputs = new string[] { "y", "yes", "1"};
            string[] negativeInputs = new string[] { "n", "no", "0" };
            string[] acceptedInputs = positiveInputs.Concat(negativeInputs).ToArray();

            string input = ConReadCheckInput(acceptedInputs);
            if (input.Equals(positiveInputs))
                return true;
            else
                return false;
        }


    }
}
