//Copyright Jordan TV Williams 2019
//Copyright JWIndie 2019
/*
    Audit Log

    -JW 6/7/2020
        -Spelling fixes

    -MWG 6/29/2020
        -#1.1 Implimented ConsoleUtility Class
        --NOTE: Might not need all of the ConsoleController checks???
*/

using System.Collections.Generic;
using MiniConsole.Utility;

namespace MiniConsole.Library
{
    /// <summary>
    /// Parses strings into commands to run delegates for methods.
    /// </summary>
    public static class CommandInterpreter
    {
        /// <summary>
        /// Dictionary of commands to parse against.
        /// <para>Note: Cannot be serialized and must reconstructed after every recompile.</para>
        /// </summary>
        private static Dictionary<string, Command> commandLibrary = new Dictionary<string, Command>();
        private static List<CommandMeta> libraryMeta = new List<CommandMeta>();
        /// <summary>
        /// Registers a command to <see cref="commandLibrary"/>.
        /// </summary>
        /// <param name="cmd"></param>
        public static void RegisterCommand(Command cmd)
        {
            commandLibrary.Add(cmd.name, cmd);
            //add a command meta for listing commands
            libraryMeta.Add(new CommandMeta(cmd.name, cmd.description, cmd.overloads.Length));
            //if (ConsoleController.Instance) ConsoleController.Instance.LogLine (ConsoleMessageFormat.Query, "Added new command: " + cmd.name);
        }
        /// <summary>
        /// Tries to parse a string into an executable command call. Returns <see langword="false"/> if there is no command in the <see cref="commandLibrary"/> that matches.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static void TryParse(string s)
        {
            var cc = ConsoleController.Instance;
            //first take the string and prepare it 
            //then create a command reference and check the commandLibrary for matches to the command name
            var args = s.Trim(' ').Split(' ');
            Command cmd;
            try
            {
                //search the commandLibrary
                if (commandLibrary.TryGetValue(args[0], out cmd))
                {
                    //if we find aa match, make sure the delegate is functional and not null
                    if (cmd.handler == null)
                    {
                        //log null handler error
                        ErrorHandler.LogError(ConsoleError.MCx0007);
                    }
                    else
                    {
                        //resize the argument array to exclude the command name
                        args = args.GetRange(1, args.Length - 1);

                        //perform basic keyword handling only if args were given
                        if (args.Length > 0)
                        {

                            //check if the first char of the keyword argument is a $ 
                            if (args[0][0] == '$')
                            {

                                //parse the argument as lowercase into a keyword
                                cmd.KeywordUsed = Keywords.Parse(args[0].ToLower());

                                //check keyword and handle it
                                //only any, info, and syntax, and unknown can be handled by the interpreter
                                switch (cmd.KeywordUsed)
                                {

                                    case Keyword.Any: //list supported keywords
                                        var eval = cmd.keywordSupport.GetEvaluations();

                                        //#1.1 
                                        ConsoleUtility.LogLine(ConsoleMessageFormat.Help, cmd.name + " supports the following keywords:");
                                        if (eval[2]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Any));
                                        if (eval[3]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Complete));
                                        if (eval[4]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Default));
                                        if (eval[5]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Info));
                                        if (eval[6]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Partial));
                                        if (eval[7]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Syntax));
                                        if (eval[8]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Value));
                                        if (eval[9]) ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Open));
                                        return;

                                    case Keyword.Info:
                                        ConsoleUtility.LogLine(ConsoleMessageFormat.Help, cmd.name + ": <i>" + cmd.description + "</i>");
                                        return;

                                    case Keyword.Syntax:
                                        ConsoleUtility.LogLine(ConsoleMessageFormat.Help, cmd.name + " overloads:");
                                        //log each overload.
                                        for (int i = 0; i < cmd.overloads.Length; i++)
                                        {
                                            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + "<i>" + cmd.overloads[i] + "</i>");
                                        }
                                        return;
                                    case Keyword.Unknown:
                                        ConsoleUtility.LogLine(ConsoleMessageFormat.Error, "Unknown keyword: <i>" + args[0] + "</i>");
                                        return;
                                }
                            }
                            else
                            {
                                //not a keyword
                                cmd.KeywordUsed = Keyword.None;
                            }
                        }

                        //if no keyword handling was required, check the argument length against the mask.
                        if (cmd.argumentMask.Compare(args.Length) || cmd.paramsArguments)
                        {
                            //call the method
                            cmd.handler.Invoke(cmd, args);
                        }
                        else
                        {
                            //#1.1
                            //log an argument error
                            ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, "Use the <i>" + Keywords.Syntax.value + "</i> keyword to list the overloads for a command");
                        }
                    }
                }
                else
                {
                    //throw missing command error
                    ErrorHandler.LogError(ConsoleError.MCx0008, s);
                }
            }
            catch
            {
                //come uknown parsing error. Log a fatty
                ErrorHandler.LogError(ConsoleError.MCx0011, "Could not parse command.");
            }
        }
        /// <summary>
        /// Returns a range of arguments staring at index and extening to count.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string[] GetRange(this string[] s, int index, int count)
        {
            string[] arrayOut = new string[count];

            for (int i = index; i <= count; i++)
            {
                if (i < s.Length)
                {
                    arrayOut[i - index] = s[i];
                }
                else
                {
                    arrayOut[i] = "";
                }
            }
            return arrayOut;
        }
        /// <summary>
        /// Clears every command from the command library. 
        /// <para>NOTE: Removes Every command, including base commands. 
        /// Requires a recompiliation of the library for continiued use after this function is called.</para>
        /// </summary>
        public static void ClearCommandLibrary()
        {
            LogHandler.Log("Cleared command library");
            commandLibrary.Clear();
            libraryMeta.Clear();
        }
        //#1.1
        /// <summary>
        /// Recompiles the <see cref="CommandInterpreter"/>'s internal library of commands.
        /// </summary>
        public static void RecompileCommandLibrary()
        {
            if (!LogHandler.LastLineWasAReturn) LogHandler.LogSpace();
            LogHandler.Log("Recompiling command library...");
            ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Recompiling command library...");
            //get the base commands
            var baseCommands = ConsoleController.baseCommands;

            //clear the library
            ClearCommandLibrary();

            //repopulate it 
            for (int i = 0; i < baseCommands.Length; i++)
            {
                RegisterCommand(baseCommands[i]);
            }
            ConsoleUtility.LoadCommandStubs();
            ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Finished.");
            LogHandler.Log("Finished.\n");
        }
        //#1.1
        /// <summary>
        /// Lists all <see cref="Command"/>s by name to the <see cref="ConsoleController"/> message log with its number of overrides.
        /// </summary>
        public static void ListAllCommands()
        {
            //cache the constroller refernce to reduce instance verification
            var cc = ConsoleController.Instance;

            //check if the instance exists, else return and log an error
            if (cc)
            {
                //log prefix message
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, "MiniConsole " + Version.currentVersion + " supports " + commandLibrary.Count + " commands:");

                Spacing.SetIndent(1);
                string lastNamespace = "";
                for (int i = 0; i < libraryMeta.Count; i++)
                {
                    //check the meta for a namespace
                    //of one exists compare to the last namespace
                    //if different add a visual break
                    if (libraryMeta[i].baseCommand == false)
                    {
                        if (libraryMeta[i].@namespace != lastNamespace)
                        {
                            lastNamespace = libraryMeta[i].@namespace;
                            ConsoleUtility.LogSpace();
                        }
                    }
                    ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + libraryMeta[i].name + " (" + libraryMeta[i].overloads + " overloads): <i>" + libraryMeta[i].description + "</i>");
                }
                Spacing.SetIndent(0);
            }
            else
            {
                //cc is null
                ErrorHandler.LogError(ConsoleError.MCx0001);
            }
        }
        /// <summary>
        /// Lists all recognized keywords.
        /// </summary>
        public static void ListAllKeywords()
        {
            var cc = ConsoleController.Instance;

            if (cc)
            {
                //#1.1
                //log prefix message
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, "MiniConsole " + Version.currentVersion + " supports the following keywords: ");

                Spacing.SetIndent(1);
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Any) + ": <i>" + Keywords.Any.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Complete) + ": <i>" + Keywords.Complete.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Default) + ": <i>" + Keywords.Default.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Info) + ": <i>" + Keywords.Info.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Partial) + ": <i>" + Keywords.Partial.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Syntax) + ": <i>" + Keywords.Syntax.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Value) + ": <i>" + Keywords.Value.Description + "</i>");
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + Keywords.AsFormattedString(Keyword.Open) + ": <i>" + Keywords.Open.Description + "</i>");
            }
            else
            {
                //cc is null
                ErrorHandler.LogError(ConsoleError.MCx0001);
            }
        }
    }
}