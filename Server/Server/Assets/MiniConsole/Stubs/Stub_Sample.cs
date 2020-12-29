
/*
    Audit Log

    -MWG 6/29/2020
        -#1.1 Implimented ConsoleUtility Class   
*/

using MiniConsole;
using MiniConsole.Stubs;
using MiniConsole.Utility;
using cc = MiniConsole.ConsoleController;

public class Stub_Sample : CommandStub {
    public override void Construct () {
        @namespace = "sample";

        commands = new Command[] {
            new Command ("command", SampleMethod, "Runs a sample command.",
            new string[] {
                "command",
                "command " + Keywords.AsFormattedString(Keyword.Any)
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
    public static void SampleMethod (Command cmd, string[] args) {
        switch (args.Length) {
            case 0:
                //#1.1
                //execute function
                ConsoleUtility.LogLine (ConsoleMessageFormat.Message, "Sample method log!");
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
