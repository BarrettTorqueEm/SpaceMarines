//Copyright Jordan TV Williams 2019
//Copyright JWIndie 2019

/*
    Audit Log

    -DW 6/27/2020
        -#1.1 Added catch exception to give further detail as to why streamwriter failed. 

    -MWG 6/29/2020
        -#2.1 Implimented ConsoleUtility Class   
*/

using MiniConsole.Library;
using MiniConsole.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;



namespace MiniConsole
{
    /// <summary>
    /// Command class.
    /// <para>By default, all commands support the Any, Info, and Syntax <see cref="Keyword"/>s.</para>
    /// </summary>
    public struct Command
    {
        /// <summary>
        /// Name of the command.
        /// </summary>
        public string name;
        /// <summary>
        /// Delegate to invoke the target method.
        /// </summary>
        public CommandHandler handler;
        /// <summary>
        /// Command information and/or description.
        /// </summary>
        public string description;
        /// <summary>
        /// Command syntax options.
        /// </summary>
        public string[] overloads;
        /// <summary>
        /// Returns <see langword="true"/> if deprecated. 
        /// <para>if <see langword="true"/>, the command might not be available in later versions of MiniConsole.</para>
        /// </summary>
        public bool deprecated;
        /// <summary>
        /// Arguemtn mask.
        /// </summary>
        public IntMask argumentMask;
        /// <summary>
        /// The last <see cref="Keyword"/> used for this command as determined by the <see cref="CommandInterpreter"/>.
        /// </summary>
        public Keyword KeywordUsed { get; set; }
        /// <summary>
        /// What keywords does the command support
        /// </summary>
        public readonly KeywordMask keywordSupport;
        /// <summary>
        /// Does the command support unlimited arguments?
        /// </summary>
        public readonly bool paramsArguments;
        /// <summary>
        /// Base constructor for a command.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        /// <param name="description"></param>
        /// <param name="overloads"></param>
        /// <param name="deprecated"></param>
        /// <param name="validArgumentLengths"></param>
        /// <param name="defaultValue"></param>
        public Command(string name, CommandHandler handler, string description, string[] overloads, int[] validArgumentLengths)
        {
            this.name = name;
            this.handler = handler;
            this.description = description;
            this.overloads = overloads;
            deprecated = false;

            argumentMask = new IntMask(validArgumentLengths);
            keywordSupport = new KeywordMask(KeywordMask.VerifyKeywordSupport());

            KeywordUsed = Keyword.Unknown;
            paramsArguments = false;
        }
        /// <summary>
        /// Base constructor for a command.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="handler"></param>
        /// <param name="description"></param>
        /// <param name="overloads"></param>
        /// <param name="deprecated"></param>
        /// <param name="validArgumentLengths"></param>
        /// <param name="defaultValue"></param>
        public Command(string name, CommandHandler handler, string description, string[] overloads, int[] validArgumentLengths, CommandOptions options)
        {
            this.name = name;
            this.handler = handler;
            this.description = description;
            this.overloads = overloads;
            deprecated = options.deprecated;

            argumentMask = new IntMask(validArgumentLengths);

            //Prevent null reference exception errors aon this array
            if (options.supportedKeywords == null)
            {
                options.supportedKeywords = new Keyword[0];
                //Debug.Log ("Created keyword[] to prevent null reference error");
            }

            keywordSupport = new KeywordMask(KeywordMask.VerifyKeywordSupport(options.supportedKeywords));

            KeywordUsed = Keyword.Unknown;
            paramsArguments = options.paramsArguments;
        }
    }
    /// <summary>
    /// Delegate void for all MiniConsole commands.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="args"></param>
    public delegate void CommandHandler(Command command, string[] args);
    /// <summary>
    /// Console keywords.
    /// </summary>
    public enum Keyword
    {
        None,
        Unknown,
        Any,
        Complete,
        Default,
        Info,
        Partial,
        Syntax,
        Value,
        Open
    }
    /// <summary>
    /// Determines the appearance of messaage and how it should be logged to the <see cref="LogHandler"/>.
    /// </summary>
    public enum ConsoleMessageFormat
    {
        Help,
        Message,
        Status,
        Query,
        Error,
        Deprecated
    }
    public static class LogHandler
    {
        /// <summary>
        /// Should the log file be opened when the application quits?
        /// </summary>
        [SerializeField] public static bool openLogFileOnQuit = false;
        /// <summary>
        /// Returns <see langword="true"/> if the last line logged was an empty line, new line, or return of some sort.
        /// </summary>
        public static bool LastLineWasAReturn
        {
            get
            {
                if (lastLine.Contains("\n") || lastLine == "") return true;
                return false;
            }
        }
        /// <summary>
        /// The last line that was written to the log file.
        /// </summary>
        [SerializeField] private static string lastLine;
        /// <summary>
        /// The internally storeed <see cref="StreamWriter"/> for wtiting to the log file.
        /// </summary>
        [SerializeField] private static StreamWriter writer;
        /// <summary>
        /// The filename for the log file. The base file location is <see cref="Application.persistentDataPath"/>.
        /// </summary>
        public static string LogPath { get; private set; } = "/Log.txt";
        /// <summary>
        /// Sets the log path in the <see cref="Application.persistentDataPath"/> directory.
        /// </summary>
        /// <param name="s"></param>
        public static void SetLogPath(string s)
        {
            if (Directory.Exists($"{Application.persistentDataPath}/s"))
            {
                Directory.CreateDirectory($"{Application.persistentDataPath}/s");
            }
            LogPath = s;
        }
        /// <summary>
        /// Is the <see cref="StreamWriter"/> currently open on the log file?
        /// </summary>
        public static bool Opened { get; private set; }
        /// <summary>
        /// Creates a new <see cref="StreamWriter"/> at the <see cref="LogPath"/>. Will overwrite the existing file. The <see cref="Stream"/> remains open until <see cref="CloseLog()"/> is called.
        /// </summary>
        public static void OpenLog()
        {
            //create the streamWriter
            writer = new StreamWriter(Application.persistentDataPath + LogPath);
            writer.AutoFlush = true;

            //write a few lines to the console.
            Log("Running MiniConsole " + Version.currentVersion + "\n");
        }
        /// <summary>
        /// Creates a new <see cref="StreamWriter"/> at the <see cref="LogPath"/>. Will overwrite the existing file. The <see cref="Stream"/> remains open until <see cref="CloseLog()"/> is called.
        /// </summary>
        public static void OpenLog(string path)
        {
            //create the streamWriter
            SetLogPath(path);
            writer = new StreamWriter(Application.persistentDataPath + path);
            writer.AutoFlush = true;

            //write a few lines to the console.
            Log("Running MiniConsole " + Version.currentVersion + "\n");
        }
        /// <summary>
        /// Creates a new <see cref="StreamWriter"/> at the <see cref="LogPath"/>. If a file already exists, it will append the original file. The <see cref="Stream"/> remains open until <see cref="CloseLog()"/> is called.
        /// </summary>
        public static void ContinueLog()
        {
            //search for the log file at the pathname
            if (File.Exists(Application.persistentDataPath + LogPath))
            {
                try
                {
                    StreamReader r = new StreamReader(Application.persistentDataPath + LogPath);
                    StringBuilder s = new StringBuilder(r.ReadToEnd());
                    r.Dispose();

                    //open a new log manually
                    writer = new StreamWriter(Application.persistentDataPath + LogPath);
                    writer.Write(s.ToString());
                }
                // #1.1
                catch (Exception ex)
                {
                    //throws error about file sharing protocol even though the previous writer was closed and gives insight as to what happened in console
                    Debug.LogWarning($"Error writing out stream: {ex.Message}");
                    //open a new log file
                    OpenLog();
                }
            }
            else
            {
                //if no file found, log an error and open a new log
                Debug.LogError(ConsoleError.MCx0006.message);
                OpenLog();
            }
        }
        /// <summary>
        /// Closes the current <see cref="Stream"/>  at the <see cref="LogPath"/>.
        /// </summary>
        public static void CloseLog(bool alertTermination = true)
        {
            if (writer == null)
            {
                Debug.LogError(ConsoleError.MCx0005.message);
            }
            else
            {
                if (alertTermination)
                {
                    if (!LastLineWasAReturn) LogSpace();
                    Log("Terminating application...");
                }
                writer.Flush();
                writer.Close();
                writer.Dispose();
                writer = null;

                if (openLogFileOnQuit) System.Diagnostics.Process.Start(Application.persistentDataPath + LogPath);
            }
        }
        /// <summary>
        /// Writes a line to the <see cref="StreamWriter"/> if one exists. Lines are prefixed with the <see cref="System.DateTime.Now"/>.
        /// </summary>
        public static void Log(string line)
        {
            if (writer == null)
            {
                Debug.LogError(ConsoleError.MCx0005.message);
            }
            else
            {
                //write line
                writer.WriteLine(lastLine = "[" + System.DateTime.Now.ToString() + "] " + line);
            }
        }
        /// <summary>
        /// Writes lines to the <see cref="StreamWriter"/> if one exists. Lines are prefixed with the <see cref="System.DateTime.Now"/>.
        /// </summary>
        public static void Log(params string[] lines)
        {
            if (writer == null)
            {
                Debug.LogError(ConsoleError.MCx0005.message);
            }
            else
            {
                //write lines
                //create a stringBuilder
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < lines.Length; i++)
                {
                    //append on thee last line; else appendLine
                    if (i == lines.Length - 1)
                    {
                        s.Append(lastLine = "[" + System.DateTime.Now.ToString() + "] " + lines[i]);
                    }
                    else
                    {
                        s.AppendLine("[" + System.DateTime.Now.ToString() + "] " + lines[i]);
                    }
                }
                //push to the log file
                writer.WriteLine(s.ToString());
            }
        }
        /// <summary>
        /// Writes a formatted block of lines to the <see cref="StreamWriter"/> if one exists. Only the first line is prefixed with the <see cref="System.DateTime.Now"/>. The rest are indented.
        /// </summary>
        public static void LogBlock(params string[] lines)
        {
            if (writer == null)
            {
                Debug.LogError(ConsoleError.MCx0005.message);
            }
            else
            {
                //write lines
                //create a stringBuilder
                StringBuilder s = new StringBuilder();
                s.AppendLine();
                s.AppendLine("[" + System.DateTime.Now.ToString() + "] " + lines[0]);
                for (int i = 1; i < lines.Length; i++)
                {
                    s.AppendLine("    " + lines[i]);
                }
                lastLine = "\n";
                //push to the log file
                writer.WriteLine(s.ToString());
            }
        }
        /// <summary>
        /// Writes spaces to the <see cref="StreamWriter"/> if one exists. Spaces are not prefixed.
        /// </summary>
        /// <param name="lines"></param>
        public static void LogSpace(int lines = 1)
        {
            if (lines < 1) return; //skip if we give a negative line number. Nice try!
            if (writer == null)
            {
                Debug.LogError(ConsoleError.MCx0005.message);
            }
            else
            {
                //filter process by # of spaces
                if (lines == 1)
                {
                    writer.WriteLine();
                }
                else
                {
                    StringBuilder s = new StringBuilder();
                    for (int i = 0; i < lines; i++)
                    {
                        s.Append("\n");
                    }
                    lastLine = "\n";
                    writer.Write(s.ToString());
                }
            }
        }
    }
    /// <summary>
    /// Handles spacing values for the console logs.
    /// </summary>
    public static class Spacing
    {
        public static readonly string TAB = "    ";
        public static readonly string HALF_TAB = "  ";
        /// <summary>
        /// Returns the current indent value as a string.
        /// </summary>
        public static string INDENT
        {
            get
            {
                if (indentIndexer == null)
                {
                    indentIndexer = new StringBuilder();
                    SetIndent(indentIndex);
                }
                return indentIndexer.ToString();
            }
        }
        /// <summary>
        /// Internal strring builder for storing the indent value.
        /// </summary>
        private static StringBuilder indentIndexer = new StringBuilder();
        /// <summary>
        /// The internally stored value of the index.
        /// </summary>
        private static int indentIndex = 0;
        /// <summary>
        /// Returns the string value of a indent of size <paramref name="i"/> without modifying the internally stored indent value.
        /// </summary>
        /// <param name="i"></param>
        public static string GetIndentOf(int i)
        {
            StringBuilder s = new StringBuilder();
            for (int n = 0; n < i; n++) s.Append(TAB);
            return s.ToString();
        }
        /// <summary>
        /// Sets the indent value.
        /// </summary>
        /// <param name="i"></param>
        public static void SetIndent(int i)
        {
            indentIndex = i;
            indentIndexer.Clear();
            for (int n = 0; n < i; n++) indentIndexer.Append(TAB);
        }
        /// <summary>
        /// Modifies the indent value. Clamped automatically to a minimum value of 0.
        /// </summary>
        /// <param name="i"></param>
        public static void AddIndent(int i)
        {
            indentIndex += i;
            if (indentIndex < 0)
            {
                indentIndex = 0;
                indentIndexer.Clear();
            }
            else
            {
                for (int n = 0; n < i; n++) indentIndexer.Remove(indentIndexer.Length - TAB.Length, TAB.Length);
            }
        }
    }
    /// <summary>
    /// Basic version info.
    /// </summary>
    public static class Version
    {
        public const string currentVersion = "4";
        public const string author = "Jordan T V Williams - JW Indie";
    }
    /// <summary>
    /// Keyword information.
    /// </summary>
    public static class Keywords
    {
        /// <summary>
        /// Returns the list of keywords.
        /// </summary>
        public static string[] List
        {
            get
            {
                return new string[] {
                    Any.value,
                    Complete.value,
                    Default.value,
                    Info.value,
                    Open.value,
                    Partial.value,
                    Syntax.value,
                    Value.value
            };
            }
        }
        /// <summary>
        /// Attempts to parse the string as a keyword. Returns the keyword.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Keyword Parse(string s)
        {
            //put the string in lowercase format and trim the whitespace before parsing.
            s = s.ToLower();
            s.Trim();
            //try parse
            switch (s)
            {
                case Any.value: return Keyword.Any;
                case Complete.value: return Keyword.Complete;
                case Default.value: return Keyword.Default;
                case Info.value: return Keyword.Info;
                case Partial.value: return Keyword.Partial;
                case Syntax.value: return Keyword.Syntax;
                case Value.value: return Keyword.Value;
                case Open.value: return Keyword.Open;
            }
            //if no matches found, return unknown
            return Keyword.Unknown;
        }
        /// <summary>
        /// Returns <paramref name="keyword"/> as a color formatted string.
        /// </summary>
        public static string AsFormattedString(Keyword keyword)
        {
            if (keyword == Keyword.Unknown) return "";

            string prefix = "<color=#" + Colors.Keyword.value.Substring(2) + ">";
            string value = "";
            switch (keyword)
            {
                case Keyword.Any: value = Any.value; break;
                case Keyword.Complete: value = Complete.value; break;
                case Keyword.Default: value = Default.value; break;
                case Keyword.Info: value = Info.value; break;
                case Keyword.Partial: value = Partial.value; break;
                case Keyword.Syntax: value = Syntax.value; break;
                case Keyword.Value: value = Value.value; break;
                case Keyword.Open: value = Open.value; break;
            }
            return prefix + value + "</color>";
        }
        public static class Any
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$keyword";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Refers to any of the valid keywords.";
        }
        public static class Complete
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$complete";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Runs a command as complete.";
        }
        public static class Default
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$default";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Refers to the command's default value (based on command return type).";
        }
        public static class Info
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$info";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Tells the console to provide info about a command.";
        }
        public static class Open
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$open";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Opens a file or document.";
        }
        public static class Partial
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$partial";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Runs a command as partial.";
        }
        public static class Syntax
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$overload";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Tells the console to list the command's overloads.";
        }
        public static class Value
        {
            /// <summary>
            /// The string value of the keyword.
            /// </summary>
            public const string value = "$value";
            /// <summary>
            /// Returns the description of the keyword.
            /// </summary>
            public const string Description = "Tells the console to provide the command's value.";
        }
    }
    /// <summary>
    /// List of console errors.
    /// </summary>
    public static class ConsoleError
    {
        #region ErrorCode Class
        public class ErrorCode
        {
            public readonly string message;
            public ErrorCode(string m)
            {
                message = m;
            }

        }
        #endregion
        /// <summary>
        ///  Failed to find the ConsoleController in scene.
        /// </summary>
        public static ErrorCode MCx0001 = new ErrorCode("MCx0001: Failed to find a ConsoleController instance in the scene");
        /// <summary>
        /// Too many ConsoleControllers in scene.
        /// </summary>
        public static ErrorCode MCx0002 = new ErrorCode("MCx0002: More than 1 ConsoleController instances in the scene");
        /// <summary>
        /// Tried to reinitialize the ReferenceManager after Init() was run.
        /// </summary>
        public static ErrorCode MCx0003 = new ErrorCode("MCx0003: Attempt to run RenferenceManager.Init() more than once");
        /// <summary>
        /// Null reference.
        /// </summary>
        public static ErrorCode MCx0004 = new ErrorCode("MCx0004: Null refereence");
        /// <summary>
        /// Unassigned <see cref="StreamWriter"/> on a <see cref="LogHandler"/> method call.
        /// </summary>
        public static ErrorCode MCx0005 = new ErrorCode("MCx0005: Unassigned StreamWriter on LogHandler method call");
        /// <summary>
        /// Could not find the Log.txt file at <see cref="LogHandler.LogPath"/>.
        /// </summary>
        public static ErrorCode MCx0006 = new ErrorCode("MCx0006: Log file not found at path");
        /// <summary>
        /// Tried to reference a <see cref="CommandHandler"/> that was <see langword="null"/>.
        /// </summary>
        public static ErrorCode MCx0007 = new ErrorCode("MCx0007: Null nommand handler");
        /// <summary>
        /// Could not find <see cref="Command"/> after parsing.
        /// </summary>
        public static ErrorCode MCx0008 = new ErrorCode("MCx0008: Failed to find command");
        /// <summary>
        /// <see cref="Keyword"/> not recognized.
        /// </summary>
        public static ErrorCode MCx0009 = new ErrorCode("MCx0009: Keyword not recognized");
        /// <summary>
        /// Call when there are an incorrect number of arguments for a command.
        /// </summary>
        public static ErrorCode MCx0010 = new ErrorCode("MCx0010: Arguement error");
        /// <summary>
        /// Unknown <see cref="CommandInterpreter"/> parsing error.
        /// </summary>
        public static ErrorCode MCx0011 = new ErrorCode("MCx0011: Unknown parsing error");
    }
    public struct CommandOptions
    {
        /// <summary>
        /// Should the command be treated as obsolete?
        /// </summary>
        public bool deprecated;
        /// <summary>
        /// Does the command support infinite arguments?
        /// </summary>
        public bool paramsArguments;
        /// <summary>
        /// Keywords the command can support.
        /// </summary>
        public Keyword[] supportedKeywords;
    }
}
namespace MiniConsole.Library
{
    /// <summary>
    /// Color informaation.
    /// </summary>
    public static class Colors
    {
        public static class Error
        {
            public const string value = "0xF10501";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Help
        {
            public const string value = "0x909090";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Keyword
        {
            public const string value = "0x5ca8ff";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Mystic
        {
            public const string value = "0xA776f7";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Deprecated
        {
            public const string value = "0xA62F13";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Status
        {
            public const string value = "0x5eff8c";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Syntax
        {
            public const string value = "0x5ca8ff";
            public static Color color = Commands.HexToColor(value);
        }
        public static class Query
        {
            public const string value = "0xffeb68";
            public static Color color = Commands.HexToColor(value);
        }
    }

    /// <summary>
    /// Handles and logs console errors.
    /// </summary>
    public static class ErrorHandler
    {
        //#2.1
        /// <summary>
        /// Logs an error to the console.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="target"></param>
        public static void LogError(ConsoleError.ErrorCode error, MonoBehaviour target, bool newLine = false)
        {
            LogHandler.Log(error.message + " @ " + target.name + ((newLine) ? "\n" : ""));
            if (ConsoleController.Instance) ConsoleUtility.LogLine(ConsoleMessageFormat.Error, error.message + " @ " + target.name + ((newLine) ? "\n" : ""));
        }
        //#2.1
        /// <summary>
        /// Logs an error to the console.
        /// </summary>
        /// <param name="error"></param>
        public static void LogError(ConsoleError.ErrorCode error, string m, bool newLine = false)
        {
            LogHandler.Log(error.message + ": " + m + ((newLine) ? "\n" : ""));
            if (ConsoleController.Instance) ConsoleUtility.LogLine(ConsoleMessageFormat.Error, error.message + ": " + m + ((newLine) ? "\n" : ""));
        }
        //#2.1
        /// <summary>
        /// Logs an error to the console.
        /// </summary>
        /// <param name="error"></param>
        public static void LogError(ConsoleError.ErrorCode error, bool newLine = false)
        {
            LogHandler.Log(error.message + ((newLine) ? "\n" : ""));
            if (ConsoleController.Instance) ConsoleUtility.LogLine(ConsoleMessageFormat.Error, error.message + ((newLine) ? "\n" : ""));
        }
    }

    /// <summary>
    /// Handles console logging.
    /// </summary>

    public static class Commands
    {
        /// <summary>
        /// Converts hexadecimal colors to Unity Color.
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexToColor(string hex, bool as255 = false)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            if (as255)
            {
                return new Color(r * 255, g * 255, b * 255, a * 255);
            }
            return new Color(r, g, b, a);
        }
    }
    /// <summary>
    /// Structure for checking the bit values of an integer. Similar to Unity's <see cref="LayerMask"/>.
    /// </summary>
    public struct IntMask
    {
        private int mask;
        /// <summary>
        /// Returns true if the bit at index <paramref name="i"/> is a 1;
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool Compare(int i)
        {
            if (i >= 16) return false;
            //return true if the bit at the comparison index is 1
            return (mask & (1 << i)) != 0;
        }
        /// <summary>
        /// Adds the <paramref name="values"/> to the mask.
        /// </summary>
        /// <param name="values"></param>
        private void SetMask(int[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] >= 0 && values[i] < 15)
                {
                    mask += (1 << values[i]);
                }
            }
        }
        /// <summary>
        /// Returns all comparisons of the mask at once.
        /// </summary>
        /// <returns></returns>
        public bool[] GetEvaluations()
        {
            var values = new bool[32];
            //iterate through the bits of the int mask and set a bool true if the bit is 1
            for (int i = 0; i < 32; i++)
            {
                values[i] = (mask & (1 << i)) != 0;
            }
            return values;
        }
        /// <summary>
        /// Creates an argument mask from an array of <see cref="int"/>s.
        /// </summary>
        /// <param name="validArgumentLengths"></param>
        public IntMask(int[] validArgumentLengths)
        {
            mask = 0;
            SetMask(validArgumentLengths.Distinct().ToArray());
        }
    }
    /// <summary>
    /// <see cref="Keyword"/> wrapper for an <see cref="IntMask"/>.
    /// </summary>
    public struct KeywordMask
    {
        private IntMask mask;
        /// <summary>
        /// Returns true if the keyword is included in the mask.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool Compare(Keyword query)
        {
            return mask.Compare((int)query);
        }
        public KeywordMask(params Keyword[] keywords)
        {
            var ints = new List<int>(3);

            //create an int array and cast each keyword as an int
            for (int i = 0; i < keywords.Length; i++)
            {
                ints.Add((int)keywords[i]);
            }
            //construct a new int mask
            mask = new IntMask(ints.ToArray());
        }
        /// <summary>
        /// Return all comaparisons of the mask at once.
        /// </summary>
        /// <returns></returns>
        public bool[] GetEvaluations()
        {
            return mask.GetEvaluations();
        }

        /// <summary>
        /// Cleans a <see cref="Keyword"/> array and prevents duplicates, which would break the <see cref="KeywordMask"/>.
        /// </summary>
        /// <returns></returns>
        public static Keyword[] VerifyKeywordSupport()
        {
            return new Keyword[] {
                Keyword.Any,
                Keyword.Info,
                Keyword.Syntax
            };
        }
        public static Keyword[] VerifyKeywordSupport(params Keyword[] keywords)
        {
            Keyword[] k = new Keyword[keywords.Length + 3];

            k[0] = Keyword.Any;
            k[1] = Keyword.Info;
            k[2] = Keyword.Syntax;

            for (int i = 0; i < k.Length; i++)
            {
                if (i >= 3)
                {
                    if (keywords[i - 3] == Keyword.None || keywords[i - 3] == Keyword.Unknown)
                    {
                        k[i] = Keyword.Any;
                    }
                    else
                    {
                        k[i] = keywords[i - 3];
                    }
                }
            }

            //clean the array and prevent duplicates
            return k.Distinct().ToArray();
        }
    }
    /// <summary>
    /// Holds the name, description and # of overloads for a command.
    /// </summary>
    public struct CommandMeta
    {
        public readonly string name, description, @namespace;
        public readonly int overloads;
        public readonly bool baseCommand;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="overloads"></param>
        public CommandMeta(string name, string description, int overloads)
        {
            this.name = name;
            this.description = description;
            this.overloads = overloads;

            if (name.Contains('.'))
            {
                //name contains a periodindicating a namespace
                string[] args = name.Split('.');
                @namespace = args[0];
                baseCommand = false;
            }
            else
            {
                @namespace = "";
                baseCommand = true;
            }
        }
    }
}