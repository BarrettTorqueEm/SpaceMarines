
/*
    Audit Log

    -DW 6/27/2020
        - #1.1 Reformatted string output (Multiple cases)

    -MWG 6/29/2020
        - #2.1 Implimented ConsoleUtility Class
*/

using MiniConsole;
using MiniConsole.Stubs;
using MiniConsole.Utility;
using UnityEngine;
using cc = MiniConsole.ConsoleController;

public class Stub_Application : CommandStub {
    public override void Construct () {
        @namespace = "app";

        commands = new Command[] {
            new Command ("path", DataPath, "Displays or opens the application's data path.",
                new string[] {
                    "path",
                    "path " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("persistentPath", PersistentDataPath, "Displays or opens the application's persistent data path.",
                new string[] {
                    "persistentPath",
                    "persistentPath " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("info", Info, "Displays application info",
                new string[] {
                    "info",
                    "info " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("logPath", LogPath, "Displays or opens the console log path.",
                new string[] {
                    "logPath",
                    "logPath " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                )
            };
    }
    public static void DataPath (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                //execute function
                // #1.1
                // #2.1
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"Application Data Path: {Application.dataPath}");
                return;
            case 1:
                //check keyword
                switch (cmd.KeywordUsed) {
                    case Keyword.Any:
                    case Keyword.None:
                        //break out for more filtering
                        break;
                    case Keyword.Open:
                        ConsoleUtility.LogLine (ConsoleMessageFormat.Status, "Opening data folder...");
                        System.Diagnostics.Process.Start (Application.dataPath);
                        return;
                    default: //all other recognized keywords
                        ConsoleUtility.LogFailedKeywordSupportError (cmd.name, args[0]);
                        return;
                }
                goto default;
            default:
                //any other argument configuration
                ConsoleUtility.LogArgumentError (cmd.name, args.Length);
                return;
        }
    }
    public static void PersistentDataPath (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                //execute function
                // #1.1
                // #2.1
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"Application Persistent Data Path: {Application.persistentDataPath}");
                return;
            case 1:
                //check keyword
                switch (cmd.KeywordUsed) {
                    case Keyword.Any:
                    case Keyword.None:
                        //break out for more filtering
                        break;
                    case Keyword.Open:
                        // #2.1
                        ConsoleUtility.LogLine (ConsoleMessageFormat.Status, "Opening persistent data folder...");
                        System.Diagnostics.Process.Start (Application.persistentDataPath);
                        return;
                    default: //all other recognized keywords
                        ConsoleUtility.LogFailedKeywordSupportError (cmd.name, args[0]);
                        return;
                }
                goto default;
            default:
                //any other argument configuration
                ConsoleUtility.LogArgumentError (cmd.name, args.Length);
                return;
        }
    }
    public static void LogPath (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                //execute function
                // #2.1
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"Application Log Path: {Application.consoleLogPath}");
                return;
            case 1:
                //check keyword
                switch (cmd.KeywordUsed) {
                    case Keyword.Any:
                    case Keyword.None:
                        //break out for more filtering
                        break;
                    case Keyword.Open:
                    // #2.1
                        ConsoleUtility.LogLine (ConsoleMessageFormat.Status, "Opening log file...");
                        System.Diagnostics.Process.Start (Application.consoleLogPath);
                        return;
                    default: //all other recognized keywords
                        ConsoleUtility.LogFailedKeywordSupportError (cmd.name, args[0]);
                        return;
                }
                goto default;
            default:
                //any other argument configuration
                ConsoleUtility.LogArgumentError (cmd.name, args.Length);
                return;
        }
    }
    public static void Info (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                //execute function
                // #1.1
                // #2.1
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, "Application Information:");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Platform: {Application.platform.ToString ()}");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Integrity Status: {CheckAppIntegrity ()}");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Data Path: {Application.dataPath}");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Persistent Data Path: {Application.persistentDataPath}");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Log Path: {Application.consoleLogPath}");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Version: {Application.version}");
                ConsoleUtility.LogToConsoleStatic (ConsoleMessageFormat.Query, $"{Spacing.TAB}Running: Unity {Application.unityVersion}");
                return;
            case 1:
                //check keyword
                switch (cmd.KeywordUsed) {
                    case Keyword.Any:
                    case Keyword.None:
                        //break out for more filtering
                        break;
                    default: //all other recognized keywords
                        ConsoleUtility.LogFailedKeywordSupportError (cmd.name, args[0]);
                        return;
                }
                goto default;
            default:
                //any other argument configuration
                ConsoleUtility.LogArgumentError (cmd.name, args.Length);
                return;
        }
    }
    /// <summary>
    /// Checks the <see cref="Application.genuine"/> property and returns a string based on the status.
    /// </summary>
    private static string CheckAppIntegrity () {
        if (Application.genuineCheckAvailable) {
            if (Application.genuine) {
                return "Unmodified";
            }
            else {
                return "Modified";
            }
        }
        else {
            return "Unknown";
        }
    }
}
