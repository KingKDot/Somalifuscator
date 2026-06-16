using System;
using System.Collections.Generic;

namespace SomalifuscatorV3.Methods.Obfuscation.StaticObfuscation.EnvVar
{
    public class EnvVar
    {
        private static readonly Dictionary<string, (string, string)> locationMap = new Dictionary<string, (string, string)>
                    {
                        { "ALLUSERSPROFILE", ("both", @"C:\ProgramData") },
                        { "APPDATA", ("right", @"\AppData\Roaming") },
                        { "CommonProgramFiles", ("both", @"C:\Program Files\Common Files") },
                        { "CommonProgramFiles(x86)", ("both", @"C:\Program Files (x86)\Common Files") },
                        { "CommonProgramW6432", ("both", @"C:\Program Files\Common Files") },
                        { "ComSpec", ("both", @"C:\Windows\system32\cmd.exe") },
                        { "DriverData", ("both", @"C:\Windows\System32\Drivers\DriverData") },
                        { "HOMEDRIVE", ("both", @"C:") },
                        { "HOMEPATH", ("left", @"\Users\") },
                        { "LOCALAPPDATA", ("right", @"\AppData\Local") },
                        { "OS", ("both", @"Windows_NT") },
                        { "ProgramData", ("both", @"C:\ProgramData") },
                        { "ProgramFiles", ("both", @"C:\Program Files") },
                        { "ProgramFiles(x86)", ("both", @"C:\Program Files (x86)") },
                        { "ProgramW6432", ("both", @"C:\Program Files") },
                        { "PUBLIC", ("both", @"C:\Users\Public") },
                        { "SESSIONNAME", ("both", @"Console") },
                        { "SystemDrive", ("both", @"C:") },
                        { "SystemRoot", ("both", @"C:\Windows") },
                        { "TEMP", ("right", @"\AppData\Local\Temp") },
                        { "TMP", ("right", @"\AppData\Local\Temp") },
                        { "USERPROFILE", ("left", @"C:\Users\") },
                        { "windir", ("both", @"C:\Windows") },
                        { "__APPDIR__", ("both", @"C:\Windows\system32\") },
                        { "__CD__", ("left", @"C:\Users\") },
                    };

        public static string TranslateVariable(char character)
        {
            var goodVariables = locationMap.Where(x => x.Value.Item2.Contains(character)).ToList();

            if (goodVariables.Count == 0)
            {
                return character.ToString();
            }

            var randomVariable = goodVariables[new Random().Next(goodVariables.Count)];

            var allIndices = new List<int>();
            string path = randomVariable.Value.Item2;

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == character)
                {
                    allIndices.Add(i);
                }
            }

            int characterIndex = allIndices[new Random().Next(allIndices.Count)];
            int length = 1;

            switch (randomVariable.Value.Item1)
            {
                case "both":
                    bool reverse = new Random().Next(0, 2) == 1;
                    if (!reverse)
                    {
                        return $"%{randomVariable.Key}:~{RandomSpaces()}{characterIndex},{RandomSpaces()}{length}%";
                    }
                    else
                    {
                        int rightIndexBoth = randomVariable.Value.Item2.Length - characterIndex;
                        return $"%{randomVariable.Key}:~{RandomSpaces()}-{rightIndexBoth},{RandomSpaces()}{length}%";
                    }

                case "left":
                    return $"%{randomVariable.Key}:~{RandomSpaces()}{characterIndex},{RandomSpaces()}{length}%";

                case "right":
                    int rightIndex = randomVariable.Value.Item2.Length - characterIndex;
                    return $"%{randomVariable.Key}:~{RandomSpaces()}-{rightIndex},{RandomSpaces()}{length}%";

                default:
                    return character.ToString();
            }
        }

        private static string RandomSpaces()
        {
            int spaces = new Random().Next(2, 6);
            return new string(' ', spaces);
        }
    }
}
