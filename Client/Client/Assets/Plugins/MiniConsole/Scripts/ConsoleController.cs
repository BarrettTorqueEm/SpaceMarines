//Copyright Jordan TV Williams 2019
//Copyright JWIndie 2019

/*

    Audit Log

    -MWG 6/27/2020
        -#1.1 Added default keys to get rid of Unity warning
        -#1.2 Added false opition to ensure console doesnt open on start.

    -JW 6/27/2020
        -#2.1 Added [HideInInspector] attributes to several privatee fields
        -#2.2 Added if UNITY_EDITOR directive for non build/runtime code
        -#2.3 Added base command for log file interaction

        JUST adding some lines

    -DW 6/27/2020
        - Fixed dating on Jordan's audit entries (above)+

    -MWG 6/29/2020
        -#3.1 Moved multiple functions to ConsoleUtility.cs (Too many to label) 

    -MWG 12/28/2020
        -#4.1 Spacing updated. No label.
*/

using UnityEngine;
using UnityEngine.UI;
using MiniConsole.Library;
using MiniConsole.Stubs;
using MiniConsole.Utility;
using System.Linq;

namespace MiniConsole
{
    [DisallowMultipleComponent]
    /// <summary>
    /// Controls the GUI console.
    /// </summary>
    public sealed class ConsoleController : MonoBehaviour
    {
        /// <summary>
        /// Time interval in seconds between <see cref="ScrollInputBufffer(int)"/> calls while holding the arrow keys.
        /// </summary>
        private const float autoScrollInterval = .2f;
        /// <summary>
        /// The number of lines of the <see cref="messageBuffer"/> to display on the screen at a single time.
        /// </summary>
        public static int displayBufferSize = 25;
        /// <summary>
        /// Internal <see langword="static"/> reference to a <see cref="ConsoleController"/> instance.
        /// </summary>
        private static ConsoleController instance;


        /// <summary>
        /// Returns the current <see cref="ConsoleController"/> instancce in the scene. Returns <see langword="null"/> if no <see cref="ConsoleController"/> was found the scene.
        /// </summary>
        public static ConsoleController Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<ConsoleController>();
                return instance;
            }
        }
        /// <summary>
        /// Returns <see langword="true"/> if the <see cref="ConsoleController"/> is open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return IsOpen;
            }
        }

        [Header("Keyboard Inputs")]

        // #1.1
        [SerializeField] private KeyCode TOGGLE_KEY = KeyCode.Tab;
        [SerializeField] private KeyCode CLOSE_KEY = KeyCode.Escape;

        [Header("General Settings")]
        [Range(1, 5)] [SerializeField] private int linesPerScroll = 3;
        [SerializeField] private bool openOnStart = false;
        [SerializeField] public bool forceOpen;
        [SerializeField] private bool openLogFileOnQuit = false;
        [SerializeField] public bool open = false;
        [SerializeField] private InputField commandInput;
        [SerializeField] public Text console;
        [SerializeField] public Scrollbar scroll;

        //hidden
        //#2.1
        [HideInInspector] [SerializeField] public string[] messageBuffer;
        [HideInInspector] [SerializeField] public string[] inputBuffer;
        [HideInInspector] [SerializeField] private bool wasCompiling;
        [HideInInspector] [SerializeField] private float compileTime;
        [HideInInspector] [SerializeField] public int messages;
        [HideInInspector] [SerializeField] public int inputs;
        [HideInInspector] [SerializeField] private int inputBufferIndex = -1;
        [HideInInspector] [SerializeField] public int messageBufferIndex = 0;
        [HideInInspector] [SerializeField] private float autoScrollTime = 0;
        [HideInInspector] [SerializeField] public CommandStub[] stubs;


        public static readonly Command[] baseCommands = new Command[] {
            new Command ("clear", Handler.ClearMessageBuffer, "Clears the console messages and message history.",
                new string[] {
                    "clear",
                    "clear " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("credits", Handler.GetCredits, "View Mini Console credits and version info.",
                new string[] {
                    "credits",
                    "credits " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("commands", Handler.ListCommands, "Lists all recognized commands",
                new string[] {
                    "commands",
                    "commands " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("help", Handler.DisplayHelpDialogue, "Displays help messages to the console.",
                new string[] {
                    "help",
                    "help " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int [] {0,1}
                ),
            new Command ("keywords", Handler.ListKeywords, "Lists all recognized keywords",
                new string[] {
                    "keywords",
                    "keywords " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command("compile", Handler.Recompile, "Recompiles the command library.",
                new string[] {
                    "compile",
                    "compile " + Keywords.AsFormattedString(Keyword.Any)
                },
                new int[] {0,1}
                ),
            new Command ("say", Handler.Say, "Prints a message to the console. Can print to the log file as well.",
                new string[] {
                    "say " + Keywords.AsFormattedString(Keyword.Any),
                    "say <string>",
                    "say <string> <bool>"
                },
                new int[] {1,2},
                new CommandOptions {paramsArguments = true}
                ),
            new Command ("stubs", Handler.IncludeStub, "Enables or disables stubs by index. Forces a command library recompile.",
                new string[] {
                    "stubs "  + Keywords.AsFormattedString(Keyword.Any),
                    "stubs <int> <bool>"
                },
                new int [] {1,2},
                new CommandOptions {
                    supportedKeywords = new Keyword[] {
                        Keyword.Value,
                        Keyword.Complete
                    }
                }
                ),
            new Command ("quit", Handler.Quit, "Quits the application.",
                new string[] {
                    "quit",
                    "quit " + Keywords.AsFormattedString (Keyword.Any)
                },
                new int[] {0,1}
                ),
            //#2.3
            //new Command ("log",InteractLogFile, "Interacts with the log file",
            //    new string[] {
            //        "log "  + Keywords.AsFormattedString(Keyword.Any),   
            //    },
            //    new int[] {1},
            //    new CommandOptions {
            //        supportedKeywords = new Keyword[] {
            //            Keyword.Open,
            //            Keyword.Value
            //        }
            //    }
            //    ),
        };

        private void Start()
        {
            LogHandler.OpenLog();
            LogHandler.Log("Setting up MiniConsole Runtime...");

            LogHandler.openLogFileOnQuit = openLogFileOnQuit;

            //set buffer sizes
            messageBuffer = new string[500];
            inputBuffer = new string[50];

            //search through all children and get references
            foreach (Transform t in transform)
            {
                if (t.name == "$Input") commandInput = t.GetComponent<InputField>();
                if (t.name == "$ConsoleView") console = t.GetComponent<Text>();
                if (t.name == "$Scroll") scroll = t.GetComponent<Scrollbar>();
            }

            //set the scroll bar size and value
            ConsoleUtility.UpdateScrollBarSize();
            ConsoleUtility.UpdateScrollBarValue();

            // #1.2
            //set console visibility
            open = forceOpen || openOnStart ? true : false;
            //if (forceOpen || openOnStart) open = true;

            ToggleVisibilityInternal();

            if (baseCommands.Length > 0)
            {

                LogHandler.Log("Loading base commands...");
                for (int i = 0; i < baseCommands.Length; i++)
                {
                    CommandInterpreter.RegisterCommand(baseCommands[i]);
                }
                LogHandler.Log("Loaded " + baseCommands.Length + " base commands.");
            }

            //load any stubs
            ConsoleUtility.LoadCommandStubs();

            ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Successfully started MiniConsole " + Version.currentVersion + "!");

            LogHandler.Log("Done.\n");

        }
        public void Update()
        {
            #region Inputs
            //toggling
            if (!forceOpen)
            {
                if (Input.GetKeyDown(TOGGLE_KEY)) ConsoleUtility.ToggleVisibility();
                if (Input.GetKeyDown(CLOSE_KEY)) ConsoleUtility.ToggleVisibility(false);
            }
            if (open)
            {
                //force the input field to be active
                commandInput.ActivateInputField();

                //check for inputs
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    ScrollInputBufffer(1);
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (Time.time - autoScrollTime >= autoScrollInterval) ScrollInputBufffer(1);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ScrollInputBufffer(-1);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (Time.time - autoScrollTime >= autoScrollInterval) ScrollInputBufffer(-1);
                }

                //check for messaage scrolling
                if (messages > displayBufferSize)
                {
                    int scrollDelta = (int)Input.mouseScrollDelta.y;
                    if (scrollDelta != 0)
                    {
                        messageBufferIndex += scrollDelta * linesPerScroll;

                        //clamp the buffer index to >= 0
                        messageBufferIndex = Mathf.Clamp(messageBufferIndex, 0, messages - displayBufferSize);

                        //update the display and the scroll bar
                        ConsoleUtility.UpdateConsoleDisplay();
                        ConsoleUtility.UpdateScrollBarValue();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return) && commandInput.text != "")
                {
                    //get the input and record it to the command buffer if the command line is new
                    if (inputBuffer[0] != commandInput.text) ConsoleUtility.BufferInput(commandInput.text);

                    //send the text to the parser
                    CommandInterpreter.TryParse(commandInput.text);
                    messageBufferIndex = 0;

                    ////update the console output
                    ConsoleUtility.UpdateConsoleDisplay();
                    ConsoleUtility.UpdateScrollBarValue();
                    //clear the command input text field and ensure it still has focus
                    commandInput.text = "";
                    inputBufferIndex = -1;
                }
            }
            #endregion
#if UNITY_EDITOR
            //display a series of messages if source code was updated during runtime
            //on start compiling, get some info and store it
            if (UnityEditor.EditorApplication.isCompiling && !wasCompiling)
            {
                ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Recompiling changes to the source code. Hold on...");

                compileTime = Time.time;
                wasCompiling = true;

                //prevent LogHandler StreamWriter compiler issues
                //Close the stream
                if (!LogHandler.LastLineWasAReturn) LogHandler.LogSpace();
                LogHandler.Log("Started source code recompile...");

                //dont open the log on recompile
                bool openState = LogHandler.openLogFileOnQuit;
                LogHandler.openLogFileOnQuit = false;
                LogHandler.CloseLog(false);
                LogHandler.openLogFileOnQuit = openState;
            }
            //on finish compiling
            if (!UnityEditor.EditorApplication.isCompiling && wasCompiling)
            {
                instance = this;
                wasCompiling = false;
                compileTime = (Time.time - compileTime);

                ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Finished! (" + compileTime.ToString() + ") seconds");

                //pick up the log from where we left off
                LogHandler.ContinueLog();
                LogHandler.Log("Finished compile in " + compileTime.ToString("###0.0###") + " seconds");

                //recompile the command library because dictiionaries do not serialize
                CommandInterpreter.RecompileCommandLibrary();
            }
#endif
        }
        private void ScrollInputBufffer(int i)
        {
            if (messages == 0) return;
            if (i == 1)
            { //scroll up
                autoScrollTime = Time.time;
                //move up in the array
                if (inputBufferIndex + 1 == inputBuffer.Length) return;

                if (inputBufferIndex + 1 < inputs && inputBuffer[inputBufferIndex + 1] != "" && inputBuffer[inputBufferIndex + 1].Length > 0)
                {
                    inputBufferIndex++;
                    commandInput.text = inputBuffer[inputBufferIndex];
                    commandInput.caretPosition = commandInput.text.Length;
                }
            }
            else if (i == -1)
            {
                autoScrollTime = Time.time;
                //move down in the array
                if (inputBufferIndex - 1 == -2) return;

                inputBufferIndex--;
                if (inputBufferIndex >= 0 && inputBuffer[inputBufferIndex] != "" && inputBuffer[inputBufferIndex + 1].Length > 0)
                {
                    commandInput.text = inputBuffer[inputBufferIndex];
                    commandInput.caretPosition = commandInput.text.Length;
                }
                if (inputBufferIndex == -1)
                {
                    //clear the text 
                    commandInput.text = "";
                    //commandInput.textComponent.text = "";
                    commandInput.caretPosition = 0;
                }
            }
        }
        /// <summary>
        /// Called when the Application quits.
        /// </summary>
        private void OnApplicationQuit()
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Status, "Quitting application...");
            LogHandler.CloseLog();
        }

        /// <summary>
        /// Internal methods controlling console visibility. Ignores <see cref="forceOpen"/>.
        /// </summary>
        public void ToggleVisibilityInternal()
        {
            GetComponent<Image>().enabled = open;
            try
            {
                scroll.gameObject.SetActive(open);
                console.gameObject.SetActive(open);
                commandInput.gameObject.SetActive(open);
            }
            catch
            {
                //throwing error because this method called before start, so lets grab the references manually
                //search through all children and get references
                foreach (Transform t in transform)
                {
                    if (t.name == "$Input") commandInput = t.GetComponent<InputField>();
                    if (t.name == "$ConsoleView") console = t.GetComponent<Text>();
                    if (t.name == "$Scroll") scroll = t.GetComponent<Scrollbar>();
                }
            }
            if (open == false) commandInput.text = "";
        }

        //#2.2
#if UNITY_EDITOR
        /// <summary>
        /// Handle editor value updates.
        /// </summary>
        private void OnValidate()
        {
            //if the user forces the open state, update the console visibility
            if (forceOpen)
            {
                open = true;
                ToggleVisibilityInternal();
            }
            LogHandler.openLogFileOnQuit = openLogFileOnQuit;
        }
#endif
    }
}
