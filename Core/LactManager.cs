using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal class LactManager
    {
        public enum ArgType
        {
            None,
            Command,
            Text
        }

        public static Dictionary<string, LaAction> Actions { get; private set; }
        /// <summary>
        /// Finds and adds LaActions into the dictionary Actions.
        /// </summary>
        private void SetupActionDict()
        {
            Array.ForEach(Array.FindAll(Assembly.GetExecutingAssembly().GetTypes(), type => !type.IsAbstract && type.IsSubclassOf(typeof(LaAction))), t =>
            {
                var action = (LaAction)Activator.CreateInstance(t);
                Actions.Add(action.Trigger, action);
            });
        }

        /// <summary>
        /// Get type of the <paramref name="arg"/>.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public ArgType GetArgType(string arg)
        {
            if (arg == null || arg == string.Empty || arg.Length < 1)
            {
                return ArgType.None;
            }
            if (arg[0].MatchSpecType(SpecT.SpecType.CmdTrigger))
            {
                return ArgType.Command;
            }
            return ArgType.Text;
        }
    }
}