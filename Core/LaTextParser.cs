using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    internal class LaTextParser
    {
        internal enum Parsing
        {
            /// <summary>
            /// Parsing starts.
            /// </summary>
            TextStart,
            /// <summary>
            /// Text is a literal.
            /// </summary>
            Literal,
            /// <summary>
            /// Text is a whitespace.
            /// </summary>
            Space,
            /// <summary>
            /// Text is a separator for literals.
            /// </summary>
            LitSeparator,
            /// <summary>
            /// Text is a trigger of commands.
            /// </summary>
            CommandTrigger,
            /// <summary>
            /// Text is command.
            /// </summary>
            Command,
            /// <summary>
            /// Text is a separator for commands.
            /// </summary>
            CmdSeparator,
            /// <summary>
            /// Text is a parameter.
            /// </summary>
            Parameter,
            /// <summary>
            /// Text is a separator for parameters.
            /// </summary>
            ParaSeparator,
            /// <summary>
            /// Text is a escaper.
            /// </summary>
            Escaper,
            /// <summary>
            /// Text is escaped.
            /// </summary>
            Escapee,
            /// <summary>
            /// Text is an inline formatter.
            /// </summary>
            Formatter,
            /// <summary>
            /// Text is formatted.
            /// </summary>
            Formattee,
            /// <summary>
            /// Parsing ends.
            /// </summary>
            TextEnd
        }
        private static Parsing[] LeagalNextInputStates(Parsing state)
        {
            switch (state)
            {
                default: 
                    return new Parsing[] { Parsing.TextEnd };
                case Parsing.TextStart:
                    return new Parsing[] { Parsing.Literal, Parsing.Space, Parsing.CommandTrigger, Parsing.Escaper, Parsing.Formatter, Parsing.TextEnd };
                case Parsing.Literal:
                    return new Parsing[] { Parsing.Literal, Parsing.Space, Parsing.Escaper, Parsing.Formatter, Parsing.TextEnd };
                case Parsing.Space:
                    return new Parsing[] { Parsing.Literal, Parsing.CommandTrigger, Parsing.Escaper, Parsing.Formatter, Parsing.TextEnd };
                case Parsing.CommandTrigger:
                    return new Parsing[] { Parsing.Command };
                case Parsing.Command:
                    return new Parsing[] { Parsing.Space, Parsing.CmdSeparator, Parsing.TextEnd };
                case Parsing.CmdSeparator:
                    return new Parsing[] { Parsing.Parameter };
                case Parsing.Parameter:
                    return new Parsing[] { Parsing.Space, Parsing.ParaSeparator, Parsing.TextEnd };
                case Parsing.ParaSeparator:
                    return new Parsing[] { Parsing.Parameter };
                case Parsing.Escaper:
                    return new Parsing[] { Parsing.Escapee };
                case Parsing.Formatter:
                    return new Parsing[] { Parsing.Literal, Parsing.Space };
            }
        }
        private static Parsing NextState(Parsing previous, Parsing current)
        {

            return Parsing.TextEnd;
        }
    }
}
