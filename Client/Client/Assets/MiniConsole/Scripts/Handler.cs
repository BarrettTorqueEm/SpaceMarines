using System.Collections;
using System.Collections.Generic;
using MiniConsole.Utility;
using MiniConsole.Library;
using UnityEngine;

namespace MiniConsole
{
    public static class Handler
    {
        /// <summary>
        /// (<see cref="CommandHandler"/>) Prints a messge to the console. Will also log to the console log file if <paramref name="LogExternal"/> is set to <see langword="true"/>.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void Say(Command cmd, string[] args)
        {
            if (args.Length > 1)
            {
                if (bool.TryParse(args[args.Length - 1], out bool logExternal))
                {
                    //parsed to bool
                    ConsoleUtility.Say(string.Join(" ", args, 0, args.Length - 1), logExternal);
                }
                else
                {
                    ConsoleUtility.Say(string.Join(" ", args), false);
                }
            }
            //prevent empty strings from logging
            else if (args.Length == 1)
            {
                if (args[0].Length > 0)
                {
                    ConsoleUtility.Say(string.Join(" ", args), false);
                }
            }
        }

        public static void InteractLogFile(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    //execute function
                    System.Diagnostics.Process.Start(LogHandler.LogPath);
                    return;
                case 1:
                    //check keyword
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Open:
                            System.Diagnostics.Process.Start(LogHandler.LogPath);
                            break;
                        case Keyword.Value:
                            ConsoleUtility.LogLine(ConsoleMessageFormat.Query, $"Log file saved @ {LogHandler.LogPath}");
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    goto default;
                default:
                    //any other argument configuration
                    Debug.Log("Parser arg error");
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// (<see cref="CommandHandler"/>) Lists all commands to the console.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void ListCommands(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    //execute function
                    CommandInterpreter.ListAllCommands();
                    return;
                case 1:
                    //check keyword
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    goto default;
                default:
                    //any other argument configuration
                    Debug.Log("Parser arg error");
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// (<see cref="CommandHandler"/>) Lists all recognized keywords.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void ListKeywords(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    //execute function
                    CommandInterpreter.ListAllKeywords();
                    return;
                case 1:
                    //check keyword
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    goto default;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        public static void ClearMessageBuffer(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    if (ConsoleController.Instance)
                    {
                        ConsoleUtility.ClearMessageBuffer();
                    }
                    else
                    {
                        ErrorHandler.LogError(ConsoleError.MCx0001);
                    }
                    return;
                case 1:
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    return;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// (<see cref="CommandHandler"/>) Recompiles the <see cref="CommandInterpreter.commandLibrary"/>.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void Recompile(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    CommandInterpreter.RecompileCommandLibrary();
                    return;
                case 1:
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    return;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// (<see cref="CommandHandler"/>) Quits the application or editor.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void Quit(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    Application.Quit();
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    return;
                case 1:
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    return;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// Activates or deactivates stubs. Recompiles the command library afterwards.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void IncludeStub(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        case Keyword.Value:
                            //print out the number of stubs
                            if (ConsoleController.Instance.stubs == null)
                            {
                                ConsoleUtility.LogLine(ConsoleMessageFormat.Query, "The stub[] is null.");
                            }
                            else
                            {
                                ConsoleUtility.LogLine(ConsoleMessageFormat.Query, "There are " + ConsoleController.Instance.stubs.Length + " stubs loaded.");
                            }
                            return;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    return;
                case 2:
                    int index;
                    bool state;
                    bool failedOnIntParse, failedOnBoolParse;
                    //parse values
                    failedOnIntParse = !int.TryParse(args[0], out index);
                    failedOnBoolParse = !bool.TryParse(args[1], out state);


                    if (failedOnIntParse || failedOnBoolParse)
                    {
                        //check to see if the int failed but we used the keyword $complete
                        if (failedOnIntParse && cmd.KeywordUsed == Keyword.Complete && !failedOnBoolParse)
                        {
                            for (int i = 0; i < ConsoleController.Instance.stubs.Length; i++)
                            {
                                ConsoleController.Instance.stubs[i].active = state;
                            }
                            ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "All stubs have been " + ((state == true) ? "enabled" : "disabled"));
                            CommandInterpreter.RecompileCommandLibrary();
                        }
                        else
                        {
                            //log syntax error
                            ConsoleUtility.LogSyntaxError(string.Join(" ", args),
                                new string[] {
                                "include <int> <bool>"
                                }
                                );
                        }
                    }
                    else
                    {
                        //successful parse
                        if (index < 0 || index >= ConsoleController.Instance.stubs.Length)
                        {
                            //log index out of range error
                            ConsoleUtility.LogArgumentOutOfRangeError(cmd.name, 0, index, 0, ConsoleController.Instance.stubs.Length);
                            return;
                        }
                        //set the active state of the stub and recompile
                        ConsoleController.Instance.stubs[index].active = state;
                        ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Stub " + index + " has been " + ((state == true) ? "enabled" : "disabled"));
                        CommandInterpreter.RecompileCommandLibrary();
                    }
                    return;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// (<see cref="CommandHandler"/>) Log help dialogue to the console.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void DisplayHelpDialogue(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    //execute function
                    ConsoleUtility.DisplayHelpDialogue();
                    return;
                case 1:
                    //check keyword
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    goto default;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
        /// <summary>
        /// Prints the current Mini Console version to the console.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        public static void GetCredits(Command cmd, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    //execute function
                    ConsoleUtility.LogLine(ConsoleMessageFormat.Query, "Current Mini Console version: " + Version.currentVersion);
                    ConsoleUtility.LogLine(ConsoleMessageFormat.Query, Spacing.HALF_TAB + "Written by: " + Version.author);
                    return;
                case 1:
                    //check keyword
                    switch (cmd.KeywordUsed)
                    {
                        case Keyword.Any:
                        case Keyword.None:
                            //break out for more filtering
                            break;
                        default: //all other recognized keywords
                            ConsoleUtility.LogFailedKeywordSupportError(cmd.name, args[0]);
                            return;
                    }
                    goto default;
                default:
                    //any other argument configuration
                    ConsoleUtility.LogArgumentError(cmd.name, args.Length);
                    return;
            }
        }
    }
}

