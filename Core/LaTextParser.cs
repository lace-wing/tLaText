using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace tLaText.Core
{
    public struct EscapingText
    {
        public char[] input;
        public string output;

        public EscapingText(char[] input, string output)
        {
            this.input = input;
            this.output = output;
        }

        public bool TryMatch(char input, out string output)
        {
            output = string.Empty;
            if (!this.input.Contains(input))
            {
                return false;
            }
            output = this.output;
            return true;
        }
    }
    
    internal class LaTextParser
    {
        public enum CharType
        {
            Start,
            Literal,
            Escaper,
            Escapee,
            Formatter,
            Formattee,
            CmdTrigger,
            Command,
            CmdSeparator,
            Parameter,
            ParaSeparator,
            End
        }
    }
    public static class SpecT
    {
        public static readonly char[] Escaper = new char[] { '\\' };
        public static readonly char[] CmdTrigger = new char[] { '.', '。' };
        public static readonly char[] Formatter = new char[] { '*' };
        public static readonly char[] SpacePlaceholder = new char[] { '_' };

        public enum SpecType
        {
            Escaper,
            CmdTrigger,
            Formatter,
            SpacePlaceholder
        }

        /// <summary>
        /// Get chars belong to the <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>A char[0] if no matching char.</returns>
        public static char[] GetSpecChars(this SpecType type)
        {
            switch (type)
            {
                case SpecType.Escaper:
                    return Escaper;
                case SpecType.CmdTrigger:
                    return CmdTrigger;
                case SpecType.Formatter: 
                    return Formatter;
                case SpecType.SpacePlaceholder:
                    return SpacePlaceholder;
            }
            return new char[0];
        }
        /// <summary>
        /// Matches <paramref name="spec"/> with types of special chars.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool MatchSpecType(this char spec, SpecType type)
        {
            foreach (char c in type.GetSpecChars())
            {
                if (spec == c)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public static class EscT
    {
        public static readonly EscapingText LineFeed = new EscapingText(new char[] { 'n', 'r' }, "\n");
        public static readonly EscapingText Tab = new EscapingText(new char[] { 't' }, "\t");
        public static EscapingText[] Escapables = new EscapingText[] { };

        /// <summary>
        /// Retrives all EscapingTexts in class EscT.
        /// </summary>
        /// <returns></returns>
        public static EscapingText[] GetEscTexts()
        {
            var fields = Array.FindAll(typeof(EscT).GetFields(BindingFlags.Public | BindingFlags.Static), t => t.IsInitOnly && t.DeclaringType == typeof(EscapingText));
            var result = new List<EscapingText>();
            Array.ForEach(fields, field =>
            {
                var value = field.GetValue(null);
                result.Add((EscapingText)value);
            });
            return result.ToArray();
        }
        public static string Escape(char escapee)
        {
            Escapables = Escapables.Length < 1 ? GetEscTexts() : Escapables;
            for (int i = 0; i < Escapables.Length; i++)
            {
                if (Escapables[i].TryMatch(escapee, out string output))
                {
                    return output;
                }
            }
            return string.Empty;
        }
    }
}
