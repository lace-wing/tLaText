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
        public enum ArgType
        {
            Command,
            Text
        }
        public enum CharType
        {
            Literal,
            Escaper,
            Escapee,
            Formatter,
            Formattee,
            CmdTrigger,
            Command,
            CmdSeparator,
            Parameter,
            ParaSeparator
        }
    }
    public static class SpecT
    {
        public static readonly char[] Escaper = new char[] { '\\' };
        public static readonly char[] CmdTrigger = new char[] { '.', '。' };
        public static readonly char[] Formatter = new char[] { '*' };
        public static readonly char[] SpacePlaceholder = new char[] { '_' };
    }
    public static class EscT
    {
        public static readonly EscapingText LineFeed = new EscapingText(new char[] { 'n', 'r' }, "\n");
        public static readonly EscapingText Tab = new EscapingText(new char[] { 't' }, "\t");

        private static EscapingText[] GetEscTexts()
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
            EscapingText[] escapingTexts = GetEscTexts();
            for (int i = 0; i < escapingTexts.Length; i++)
            {
                if (escapingTexts[i].TryMatch(escapee, out string output))
                {
                    return output;
                }
            }
            return string.Empty;
        }
    }
}
