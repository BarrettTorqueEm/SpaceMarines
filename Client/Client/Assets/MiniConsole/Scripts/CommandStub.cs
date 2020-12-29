//Copyright Jordan TV Williams 2019
//Copyright JWIndie 2019

using MiniConsole.Library;
using System.Linq;
using UnityEngine;

namespace MiniConsole.Stubs {
    public class CommandStub : MonoBehaviour {
        /// <summary>
        /// Determines if the commands from this stub shoulc be included when the Command Library is recompiled.
        /// </summary>
        public bool active = true;
        /// <summary>
        /// The namespace of the command. Used to group commands.
        /// </summary>
        protected string @namespace = "stub";
        /// <summary>
        /// The list of commands to add to the library upon stub loading.
        /// </summary>
        protected Command[] commands;
        /// <summary>
        /// Returns the namespace of the <see cref=" CommandStub"/>.
        /// </summary>
        public string Namespace {
            get {
                return @namespace;
            }
        }
        /// <summary>
        /// Registers commands to the command library under a <paramref name="namespace"/>. By default the namespace is 'Stub'.
        /// </summary>
        public void RegisterCommands () {
            //ensure all stubs have a namespace
            if (@namespace.Length < 1) @namespace = "stub";

            //only add commands if there are commands to add
            if (commands == null) {
                //create command [] if null
                commands = new Command[0];
            }
            else if (commands.Length > 0) {
                //format command with the namespace

                //sort the commands by name
                var list = commands.ToList ();
                list.Sort ((x, y) => string.Compare (x.name, y.name));
                commands = list.ToArray ();

                //iterate through each command
                for (int i = 0 ; i < commands.Length ; i++) {

                    //add the namespace to the command name
                    commands[i].name = @namespace + "." + commands[i].name; //format name

                    //add the namespace prefix to each override
                    for (int o = 0 ; o < commands[i].overloads.Length ; o++) {
                        commands[i].overloads[o] = @namespace + "." + commands[i].overloads[o]; //format each overload
                    }

                    //finally register the command to the library
                    CommandInterpreter.RegisterCommand (commands[i]);
                }
            }
        }
        /// <summary>
        /// Method called on all CommandStubs refore registration.
        /// </summary>
        public virtual void Construct () {

        }
    }
}
