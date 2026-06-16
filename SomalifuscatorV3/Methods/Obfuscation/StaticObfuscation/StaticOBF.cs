using SomalifuscatorV3.Methods.Obfuscation.StaticObfuscation.EnvVar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SomalifuscatorV3.Methods.Obfuscation.StaticObfuscation
{
    internal class StaticOBF
    {
        private static string GenerateRandomString(int length)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789(){}-+*/";
            var random = new Random();

            var result = new char[length];
            result[0] = letters[random.Next(letters.Length)];

            for (int i = 1; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }

            return new string(result);
        }

        private static string ObfuscateStaticRandomVarStuff(char character)
        {
            int length = new Random().Next(6, 12);

            bool addBack = new Random().Next(0, 2) == 0;

            if (addBack)
                return $"%{GenerateRandomString(length)}%{character}%{GenerateRandomString(length)}%";
            else
                return $"%{GenerateRandomString(length)}%{character}";
        }

        private static string ObfuscateStaticEnvVar(char character)
        {
            return EnvVar.EnvVar.TranslateVariable(character);
        }

        public static string ObfuscateStatic(char character)
        {
            var methods = new Func<char, string>[]
            {
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
                ObfuscateStaticRandomVarStuff,
            };

            var outPut = methods[new Random().Next(0, methods.Length)](character);

            if (outPut == character.ToString())
                outPut = ObfuscateStaticRandomVarStuff(character);
            return outPut;
        }
    }
}
