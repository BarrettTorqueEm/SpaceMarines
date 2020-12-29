
using MiniConsole;
using MiniConsole.Stubs;
using MiniConsole.Utility;
using UnityEngine;
using cc = MiniConsole.ConsoleController;

/*
    Audit Log

    -DW 6/27/2020
        -#1.1 Modified commands description output
        -#1.2 Refactored string output for graphical based info
        -#1.3 Added more info to retrieve about graphics hardware

    -MWG 6/29/2020
        -#2.1 Implimented ConsoleUtility Class
*/

public class Stub_GraphicsInfo : CommandStub {
    public override void Construct () {
        @namespace = "graphics";

        commands = new Command[] {
            // #1.1 
            new Command ("info", GetGraphicsInfo, "Displays graphics card and graphical software info",
            new string[] {
                "info",
                "info " + Keywords.AsFormattedString(Keyword.Any)
            },
            new int[] {0,1}
            )
            };
    }
    #region Command Handlers
    /// <summary>
    /// (<see cref="CommandHandler"/>) Runs a sample method.
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    public static void GetGraphicsInfo (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                // DEFAULT COMMAND TO EXECUTE
                // #1.2
                // #2.1
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, "Graphics Info:");
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, $"{Spacing.TAB}Device vendor: {SystemInfo.graphicsDeviceVendor}"); // Get graphic card's vendor (NVIDIA, AMD, Intel, etc)
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, $"{Spacing.TAB}Graphic memory: {SystemInfo.graphicsMemorySize.ToString ("###,###,###,##0 MB")}"); // Get graphic card memory

                // #1.3
                // #2.1
                ConsoleUtility.LogLine(ConsoleMessageFormat.Query, $"{Spacing.TAB}Supports multithreading: {SystemInfo.graphicsMultiThreaded}");
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, $"{Spacing.TAB}Device name: {SystemInfo.graphicsDeviceName}"); // Get graphic card name (GTX 1060, RTX 2070, etc)
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, $"{Spacing.TAB}Device version: {SystemInfo.graphicsDeviceVersion}"); // Gets graphics device version number
                ConsoleUtility.LogLine (ConsoleMessageFormat.Query, $"{Spacing.TAB}Shader model: {SystemInfo.graphicsShaderLevel.ToString ()}"); // Get shader model version of card
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
    #endregion
}
