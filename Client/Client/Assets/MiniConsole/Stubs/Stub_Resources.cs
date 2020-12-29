using System;
using MiniConsole;
using MiniConsole.Stubs;
using MiniConsole.Utility;
using UnityEngine;
using cc = MiniConsole.ConsoleController;

/*

    Audit Log

    -DW 7/15/2020
        -#1.1 Reformatted process to obtain systems total memory usage
*/



public class Stub_Resources : CommandStub {

    public override void Construct () {
        @namespace = "resources";

        commands = new Command[] {
            // #1.1 
            new Command ("usage", GetUsedResources, "Displays amount of resources being used by application.",
            new string[] {
            "info",
            "info " + Keywords.AsFormattedString (Keyword.Any)
            },
            new int[] { 0, 1 }
            )
        };
    }
    #region Command Handlers
    /// <summary>
    /// (<see cref="CommandHandler"/>) Runs a sample method.
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    public static void GetUsedResources (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                // DEFAULT COMMAND TO EXECUTE
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, "Currently Using:");
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, $"{Spacing.TAB}RAM available: {GetRemainingMemory()} of {SystemInfo.systemMemorySize}MB used.");
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

    // #1.1
    // Get total memory usage on runtime
    private static string GetRemainingMemory () {
        int usedMem = unchecked((int)GC.GetTotalMemory(true));
        int totalMemory = SystemInfo.systemMemorySize;

        int answer = totalMemory - usedMem;
        return Math.Abs(answer).ToString();
    }
    #endregion
}