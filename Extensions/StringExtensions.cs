using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Extensions
{
    public static class StringExtensions
    {
        public static string Conform(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (c == 'A' || c == 'à' || c == 'â' || c == 'Ä' || c == 'Ã' || c == 'Å') { sb.Append('a'); }
                else if (c == 'B') { sb.Append('b'); }
                else if (c == 'C' || c == 'ç') { sb.Append('c'); }
                else if (c == 'D') { sb.Append('d'); }
                else if (c == 'E' || c == 'é' || c == 'è' || c == 'ê' || c == 'ë' || c == '€') { sb.Append('e'); }
                else if (c == 'F') { sb.Append('f'); }
                else if (c == 'G') { sb.Append('g'); }
                else if (c == 'H') { sb.Append('h'); }
                else if (c == 'I' || c == 'ï' || c == 'î') { sb.Append('i'); }
                else if (c == 'J') { sb.Append('j'); }
                else if (c == 'K') { sb.Append('k'); }
                else if (c == 'L') { sb.Append('l'); }
                else if (c == 'M') { sb.Append('m'); }
                else if (c == 'N') { sb.Append('n'); }
                else if (c == 'O') { sb.Append('o'); }
                else if (c == 'P') { sb.Append('p'); }
                else if (c == 'Q') { sb.Append('q'); }
                else if (c == 'R') { sb.Append('r'); }
                else if (c == 'S') { sb.Append('s'); }
                else if (c == 'T') { sb.Append('t'); }
                else if (c == 'U') { sb.Append('u'); }
                else if (c == 'V') { sb.Append('v'); }
                else if (c == 'W') { sb.Append('w'); }
                else if (c == 'X') { sb.Append('x'); }
                else if (c == 'Y') { sb.Append('y'); }
                else if (c == 'Z') { sb.Append('z'); }
                else if (c == ' ' || c == '_' || c == '\'' || c == '`' || c == '~' || c == '&' || c == '"' || c == '{' || c == '(' || c == '[' || c == '|' || c == '\\' || c == '/' || c == ':' || c == ';' || c == '?' || c == ',' || c == '.' || c == '!' || c == '<' || c == '>' || c == '^' || c == '@' || c == ')' || c == ']' || c == '°' || c == '=' || c == '}' || c == '+') { sb.Append('-'); }
                else if (c == '#') { sb.Append("sharp"); }
                else { sb.Append(c); }

            }
            for (int i = 1; i < sb.Length; i++)
            {
                if (sb[i - 1] == '-' && sb[i] == '-')
                {
                    sb.Remove(i, 1);
                    i--;
                }
            }
            return sb.ToString().Trim('-');
        }
    }
}
