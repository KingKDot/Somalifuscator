using System;
using System.Collections.Generic;
using System.Linq;

namespace SomalifuscatorV3.Methods.Obfuscation.DynamicObfuscation
{
    public class GlobalVariableHolder
    {
        public struct Variable
        {
            public string Name;
            public string Value;
        }

#pragma warning disable CA2211
        public static List<Variable> Variables = [];
#pragma warning restore CA2211

        public static string TranslateVariable(char character)
        {
            var goodVariables = Variables.Where(x => x.Value.Contains(character)).ToList();

            if (goodVariables.Count == 0)
            {
                return character.ToString();
            }

            var randomVariable = goodVariables[new Random().Next(goodVariables.Count)];

            var allIndices = new List<int>();
            string value = randomVariable.Value;

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == character)
                {
                    allIndices.Add(i);
                }
            }

            int characterIndex = allIndices[new Random().Next(allIndices.Count)];
            int length = 1;

            bool reverse = new Random().Next(0, 2) == 1;

            if (!reverse)
            {
                return $"%{randomVariable.Name}:~{RandomSpaces()}{characterIndex},{RandomSpaces()}{length}%";
            }
            else
            {
                int rightIndex = randomVariable.Value.Length - characterIndex;
                return $"%{randomVariable.Name}:~{RandomSpaces()}-{rightIndex},{RandomSpaces()}{length}%";
            }
        }

        private static string RandomSpaces()
        {
            int spaces = new Random().Next(2, 6);
            return new string(' ', spaces);
        }
    }
}
