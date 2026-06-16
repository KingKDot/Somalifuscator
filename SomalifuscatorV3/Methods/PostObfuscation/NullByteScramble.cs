using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomalifuscatorV3.Methods.PostObfuscation
{
    public class NullByteScramble
    {
        public static string NullByteScrambleString(string input)
        {
            var nullByteAmmount = new Random().Next(10, 100);

            var lines = input.Split(new[] { '\n' }, StringSplitOptions.None);

            var output = new StringBuilder();

            if (lines.Length > 0)
            {
                output.AppendLine(lines[0]);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (i == lines.Length - 1)
                {
                    var appendingCode = new StringBuilder();
                    appendingCode.Append(MakeRandomString(nullByteAmmount) + line.TrimEnd());
                    appendingCode.Append(new string('\0', nullByteAmmount));
                    output.Append(appendingCode.ToString());
                }
                else if (string.IsNullOrWhiteSpace(line))
                {
                    output.AppendLine();
                }
                else if (line.StartsWith(":") && !line.StartsWith("::"))
                {
                    output.AppendLine(line + "\n");
                }
                else
                {
                    output.AppendLine(MakeRandomString(nullByteAmmount) + line.TrimEnd());
                }
            }

            return output.ToString();
        }

        private static string MakeRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}
