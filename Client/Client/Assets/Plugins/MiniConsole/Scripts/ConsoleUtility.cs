// Writen by: Michael W. Girard
// Owner: Jordan TV Williams
// Date: 6/29/2020

using MiniConsole.Library;
using MiniConsole.Stubs;
using System.Linq;
using UnityEngine;

namespace MiniConsole.Utility
{
    public static class ConsoleUtility
    {

        /// <summary>
        /// Logs a formatted message to the <see cref="console"/> and, if <paramref name="logExternal"/> is <see langword="true"/>, to the <see cref="LogHandler"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="message"></param>
        /// <param name="logExternal"></param>
        public static void LogLine(ConsoleMessageFormat format, string message, bool logExternal = false)
        {
            switch (format)
            {
                case ConsoleMessageFormat.Status:
                    BufferMessage("<color=#" + Colors.Status.value.Substring(2) + ">" + message + "</color>");
                    if (logExternal) LogHandler.Log("Status: " + message);
                    break;

                case ConsoleMessageFormat.Message:
                    BufferMessage("<color=#FFF>" + message + "</color>");
                    if (logExternal) LogHandler.Log("Message: " + message);
                    break;

                case ConsoleMessageFormat.Query:
                    BufferMessage("<color=#" + Colors.Query.value.Substring(2) + ">" + message + "</color>");
                    if (logExternal) LogHandler.Log("Query: " + message);
                    break;

                case ConsoleMessageFormat.Help:
                    BufferMessage("<color=#" + Colors.Help.value.Substring(2) + ">" + message + "</color>");
                    break;

                case ConsoleMessageFormat.Error:
                    BufferMessage("<color=#" + Colors.Error.value.Substring(2) + ">" + message + "</color>");
                    if (logExternal) LogHandler.Log(message);
                    break;

                case ConsoleMessageFormat.Deprecated:
                    BufferMessage("<color=#" + Colors.Deprecated.value.Substring(2) + ">" + message + "</color>");
                    break;
            }
            UpdateConsoleDisplay();
        }
        /// <summary>
        /// Logs formatted messages to the <see cref="console"/> and, if <paramref name="logExternal"/> is <see langword="true"/>, to the <see cref="LogHandler"/>.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="messages"></param>
        /// <param name="logExternal"></param>
        public static void LogLine(ConsoleMessageFormat format, string[] messages, bool logExternal = false)
        {
            string color = "FFF";
            switch (format)
            {
                case ConsoleMessageFormat.Status:
                    color = Colors.Status.value.Substring(2);
                    if (logExternal) LogHandler.Log("Status: " + messages);
                    break;

                case ConsoleMessageFormat.Message:
                    if (logExternal) LogHandler.Log("Message: " + messages);
                    break;

                case ConsoleMessageFormat.Query:
                    color = Colors.Query.value.Substring(2);
                    if (logExternal) LogHandler.Log("Query: " + messages);
                    break;

                case ConsoleMessageFormat.Help:
                    color = Colors.Help.value.Substring(2);
                    break;

                case ConsoleMessageFormat.Error:
                    color = Colors.Error.value.Substring(2);
                    if (logExternal) LogHandler.Log(messages);
                    break;

                case ConsoleMessageFormat.Deprecated:
                    color = Colors.Deprecated.value.Substring(2);
                    break;
            }

            //for loop to add the lines and then add them to the buffer
            for (int i = 0; i < messages.Length; i++)
            {
                messages[i] = "<color=#" + color + ">" + messages + "</color>";
            }

            //send all the strings to the messaageBuffer
            BufferMessage(messages);

            UpdateConsoleDisplay();
        }
        /// <summary>
        /// Logs a formatted message to the <see cref="ConsoleController.console"/> and, if <paramref name="logExternal"/> is <see langword="true"/>, to the <see cref="LogHandler"/>. 
        /// <para>Functions as a <see langword="static"/> wrapper for <see cref="ConsoleController.LogLine(ConsoleMessageFormat, string, bool)"/>.</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="message"></param>
        /// <param name="logExternal"></param>
        public static void LogToConsoleStatic(ConsoleMessageFormat format, string message, bool LogExternal = false)
        {
            try
            {
               LogLine(format, message, LogExternal);
            }
            catch
            {
                ErrorHandler.LogError(ConsoleError.MCx0001);
            }
        }
        /// <summary>
        /// Logs a formatted message to the <see cref="ConsoleController.console"/> and, if <paramref name="logExternal"/> is <see langword="true"/>, to the <see cref="LogHandler"/>. 
        /// <para>Functions as a <see langword="static"/> wrapper for <see cref="ConsoleController.LogLine(ConsoleMessageFormat, string, bool)"/>.</para>
        /// </summary>
        /// <param name="format"></param>
        /// <param name="message"></param>
        /// <param name="logExternal"></param>
        public static void LogToConsoleStatic(ConsoleMessageFormat format, string[] messages, bool LogExternal = false)
        {
            try
            {
                LogLine(format, messages, LogExternal);
            }
            catch
            {
                ErrorHandler.LogError(ConsoleError.MCx0001);
            }
        }

        /// <summary>
        /// Logs a single whitespace line.
        /// </summary>
        public static void LogSpace()
        {
            LogLine(ConsoleMessageFormat.Message, "");
        }

        
        //-MWG Need to adjust Logic
        /// <summary>
        /// If <see langword="true"/>, the console will always remain open, else the console can be toggled.
        /// </summary>
        /// <param name="state"></param>
        public static void ToggleLock(bool state)
        {
            if (state)
            { //true
                ConsoleController.Instance.forceOpen = ConsoleController.Instance.open = state;
                ConsoleController.Instance.ToggleVisibilityInternal();
            }
            else
            { //false
                ConsoleController.Instance.forceOpen = state;
            }
        }
        /// <summary>
        /// Toggles the visibility of the console GUI. Skips function if <see cref="forceOpen"/> is <see langword="true"/>.
        /// </summary>
        public static void ToggleVisibility()
        {
            if (ConsoleController.Instance.forceOpen) return;

            ConsoleController.Instance.open = !ConsoleController.Instance.open;
            ConsoleController.Instance.ToggleVisibilityInternal();
        }
        /// <summary>
        /// Toggle sthe visibility of the console GUI. Skips function if <see cref="forceOpen"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="state"></param>
        public static void ToggleVisibility(bool state)
        {
            if (ConsoleController.Instance.forceOpen) return;
            ConsoleController.Instance.open = state;
            ConsoleController.Instance.ToggleVisibilityInternal();
        }
        
        /// <summary>
        /// Sets the <see cref="messageBufferIndex"/>.
        /// </summary>
        public static void SetMessageBufferIndex(float f)
        {
            ConsoleController.Instance.messageBufferIndex = (int)Mathf.Lerp(0, ConsoleController.Instance.messages - ConsoleController.displayBufferSize, f);

            //update the display to reflect changes
            UpdateConsoleDisplay();
        }
        /// <summary>
        /// Prints a messge to the console. Will also log to the console log file if <paramref name="LogExternal"/> is set to <see langword="true"/>.
        /// </summary>
        /// <param name="message"></param>
        public static void Say(string message, bool LogExternal = false)
        {
            LogLine(ConsoleMessageFormat.Message, message);
            if (LogExternal) LogHandler.Log("User logged message: " + message);
        }
        /// <summary>
        /// Clears the entire message buffer
        /// </summary>
        public static void ClearMessageBuffer()
        {
            ConsoleController.Instance.messages = 0;
            ConsoleController.Instance.messageBuffer = new string[ConsoleController.Instance.messageBuffer.Length];
            UpdateConsoleDisplay();
            UpdateScrollBarSize();
            UpdateScrollBarValue();
            ConsoleController.Instance.console.text = "";
        }
        /// <summary>
        /// Loads all the commands stubs into the command library.
        /// </summary>
        public static void LoadCommandStubs()
        {
            ConsoleController.Instance.stubs = ConsoleController.Instance.GetComponents<CommandStub>();
            LogHandler.Log("Loading " + ConsoleController.Instance.stubs.Length + " command stubs...");

            //sort the stubs alphabetically
            var list = ConsoleController.Instance.stubs.ToList();
            list.Sort((x, y) => string.Compare(x.Namespace, y.Namespace));
            ConsoleController.Instance.stubs = list.ToArray();

            //load each stubs commands
            for (int i = 0; i < ConsoleController.Instance.stubs.Length; i++)
            {
                if (ConsoleController.Instance.stubs[i].active)
                {
                    ConsoleController.Instance.stubs[i].Construct();
                    ConsoleController.Instance.stubs[i].RegisterCommands();
                }
            }
        }
        /// <summary>
        /// Includes or removes stubs from the command library.
        /// </summary>
        /// <param name="stubIndex"></param>
        /// <param name="activeState"></param>
        public static void IncludeStub(int stubIndex, bool activeState)
        {
            if (ConsoleController.Instance.stubs == null || ConsoleController.Instance.stubs.Length < 1)
            {
                LogLine(ConsoleMessageFormat.Error, "No stubs found.");
                return;
            }
            else
            {
                ConsoleController.Instance.stubs[stubIndex].active = activeState;
            }
            //recompile afterwards
            CommandInterpreter.RecompileCommandLibrary();
        }
        /// <summary>
        /// Update the text value of the <see cref="console"/> based on the <see cref="displayBufferSize"/>.
        /// </summary>
        public static void UpdateConsoleDisplay()
        {
            //index 0 is the newest message,  but we will have to add them in reverse to create the proper scrolling effect
            //iterate from there in up to the display size and create a string value for that.
            var s = new System.Text.StringBuilder();

            for (int i = ConsoleController.Instance.messageBufferIndex + ConsoleController.displayBufferSize - 1; i >= ConsoleController.Instance.messageBufferIndex; i--)
            {
                //prevent writing values that throw error. Not sure why they do but this fixes it. I'm sure there's a logical reason.
                if (i > ConsoleController.Instance.messages - 1) continue;

                //dont add empty strings...waste of programming resources
                if (ConsoleController.Instance.messageBuffer[i] != "" && ConsoleController.Instance.messageBuffer[i].Length > 0)
                {
                    //prevent extra whitespace with append on the last line and appendLine everywhere else
                    if (i == ConsoleController.Instance.messageBufferIndex)
                    {
                        s.Append(ConsoleController.Instance.messageBuffer[i]);
                    }
                    else
                    {
                        s.AppendLine(ConsoleController.Instance.messageBuffer[i]);
                    }
                }

                //pass that string value to the console text value
                ConsoleController.Instance.console.text = s.ToString();
            }
        }
        /// <summary>
        /// Pushes <paramref name="input"/> to the <see cref="inputBuffer"/> for indexing and input history.
        /// </summary>
        /// <param name="input"></param>
        public static void BufferInput(string input)
        {
            //shift every value in the buffer up by 1 (index 0 becomes index 1)
            //then insert the new string into index 0
            ShiftBuffer(ref ConsoleController.Instance.inputBuffer, 1);
            ConsoleController.Instance.inputBuffer[0] = input;

            if (ConsoleController.Instance.inputs < ConsoleController.Instance.inputBuffer.Length) ConsoleController.Instance.inputs++;
        }
        /// <summary>
        /// Pushes <paramref name="message"/> to the <see cref="messageBuffer"/> for indexing and history.
        /// <para>Additionally formats the console and scrollbar.</para>
        /// </summary>
        /// <param name="message"></param>
        public static void BufferMessage(params string[] message)
        {
            //shift every value in the buffer up by 1 (index 0 becomes index 1)
            //then insert the new string into index 0
            ShiftBuffer(ref ConsoleController.Instance.messageBuffer, message.Length);
            for (int i = 0; i < message.Length; i++)
            {
                ConsoleController.Instance.messageBuffer[i] = message[i];
            }

            //record the increase in viewable messages (mesasges that have actual characters) but clamp it to the size of the buffer
            ConsoleController.Instance.messages += message.Length;
            if (ConsoleController.Instance.messages > ConsoleController.Instance.messageBuffer.Length) ConsoleController.Instance.messages = ConsoleController.Instance.messageBuffer.Length;

            //update the scroll bar if we are under the max messages size
            if (ConsoleController.Instance.messages != ConsoleController.Instance.messageBuffer.Length)
            {
                UpdateScrollBarSize();
            }
        }
        /// <summary>
        /// Shifts the values of a string buffer up by <paramref name="n"/> indices.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="n"></param>
        public static void ShiftBuffer(ref string[] buffer, int n)
        {
            for (int i = buffer.Length - 1; i > n - 1; i--)
            {
                buffer[i] = buffer[i - n];
            }
        }
        /// <summary>
        /// Updates the size of the scrollbar to represent the number of non-empty messages visible.
        /// </summary>
        public static void UpdateScrollBarSize()
        {
            ConsoleController.Instance.scroll.gameObject.SetActive(ConsoleController.Instance.messages > ConsoleController.displayBufferSize);
            ConsoleController.Instance.scroll.size = (float)ConsoleController.displayBufferSize / ConsoleController.Instance.messages;
        }
        /// <summary>
        /// Updates the position of the scrollBar.
        /// </summary>
        public static void UpdateScrollBarValue()
        {
            ConsoleController.Instance.scroll.SetValueWithoutNotify((float)ConsoleController.Instance.messageBufferIndex / (ConsoleController.Instance.messages - ConsoleController.displayBufferSize));
        }
        /// <summary>
        /// Logs a series of help messages to the console.
        /// </summary>
        public static void DisplayHelpDialogue()
        {
            ConsoleUtility.LogSpace();
            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, "Mini Console Help");
            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + "Use the command <i>keywords</i> to list all usable keywords.");
            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + "Use the command <i>commands</i> to list all usable commands.");
            ConsoleUtility.LogSpace();
            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + "Note: All keywords must be prefaced with a $ symbol and must be the first argument of a command.");
            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + "Note: All commands are case sensitive.");
        }
        
        /// <summary>
        /// Logs a formatted message to the console when a command lacks support for a submitted keyword.
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="keywordAttempt"></param>
        public static void LogFailedKeywordSupportError(string commandName, string keywordAttempt)
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Error, commandName + " does not support the keyword: <i>" + keywordAttempt + "</i>");
        }
        public static void LogArgumentError(string commandName, int argumentsEntered)
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Error, commandName + " does not have an overload that takes " + argumentsEntered + " arguments.");
        }
        /// <summary>
        /// Logs a syntax error to the console.
        /// </summary>
        /// <param name="syntax"></param>
        public static void LogSyntaxError(string syntax)
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Error, "Syntax error on " + syntax);
        }
        /// <summary>
        /// Logs a syntax error to the console with syntax suggestions.
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="syntaxSuggestions"></param>
        public static void LogSyntaxError(string syntax, string[] syntaxSuggestions)
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Error, "Syntax error on " + syntax);
            ConsoleUtility.LogLine(ConsoleMessageFormat.Help, "Did you mean:");
            for (int i = 0; i < syntaxSuggestions.Length; i++)
            {
                ConsoleUtility.LogLine(ConsoleMessageFormat.Help, Spacing.TAB + "<i>" + syntaxSuggestions[i] + "</i>");
            }
        }
        /// <summary>
        /// Logs a argument range error. 
        /// <para>Note: Does not handle any logic.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void LogArgumentOutOfRangeError(string commandName, int argumentIndex, int value, int min, int max)
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Error, "Argument [" + argumentIndex + "] out of range:  must be between " + min + " and " + max + ".");
        }
        /// <summary>
        /// Logs a argument range error.
        /// <para>Note: Does not handle any logic.</para>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void LogArgumentOutOfRangeError(string commandName, int argumentIndex, float value, float min, float max)
        {
            ConsoleUtility.LogLine(ConsoleMessageFormat.Error, "Argument [" + argumentIndex + "] out of range:  must be between " + min + " and " + max + ".");
        }
    }
}

