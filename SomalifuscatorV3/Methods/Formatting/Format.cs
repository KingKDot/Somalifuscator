using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomalifuscatorV3.Methods.Formatting
{
    public class Format
    {

        public static string FormatCode(string code)
        {
            var splitCode = code.Split("\n");

            var builder = new StringBuilder();

            foreach (var line in splitCode)
            {
                var trimmedLine = line.Trim();

                if (trimmedLine == "" || trimmedLine.StartsWith("::") || trimmedLine.StartsWith("REM"))
                {
                    continue;
                }

                builder.Append(trimmedLine + "\n");
            }

            return builder.ToString();
        }
    }
}
