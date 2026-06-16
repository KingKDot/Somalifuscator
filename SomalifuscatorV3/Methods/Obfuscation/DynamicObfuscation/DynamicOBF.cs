using SomalifuscatorV3.GlobalSettings;
using SomalifuscatorV3.Methods.Obfuscation.StaticObfuscation;

namespace SomalifuscatorV3.Methods.Obfuscation.DynamicObfuscation
{
    public class DynamicOBF
    {
        public Func<char, string>[] Methods =
        {
            ObfuscateWithDynamicVar,
            ObfuscateWithStaticVar
        };

        public static string ObfuscateWithDynamicVar(char character)
        {
            if (!Settings.UseVariableSlicing)
            {
                return character.ToString();
            }

            string output = GlobalVariableHolder.TranslateVariable(character);
            if (output == character.ToString())
            {
                return ObfuscateWithStaticVar(character);
            }
            return output;
        }

        public static string ObfuscateWithStaticVar(char character)
        {
            if (!Settings.UseStaticVariableNoise)
            {
                return character.ToString();
            }

            return StaticOBF.ObfuscateStatic(character);
        }

        public static string Obfuscate(char character)
        {
            if (!Settings.UseCharacterObfuscation)
            {
                return character.ToString();
            }

            string result = ObfuscateWithDynamicVar(character);
            if (result == character.ToString())
            {
                result = ObfuscateWithStaticVar(character);
            }
            return result;
        }
    }
}
