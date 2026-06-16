using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomalifuscatorV3.GlobalSettings;

namespace SomalifuscatorV3.Methods.Generators
{
    public class VariableGenerator
    {
        public static (string command, string firstRandomVar, string secondRandomVar) GenerateVariable()
        {
            var random = new Random();
            var length = random.Next(15, 25);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789+-*/()";
            const string charsSafe = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            var builder = new StringBuilder();

            var firstRandomVar = new StringBuilder();
            firstRandomVar.Append(charsSafe[random.Next(charsSafe.Length)]);
            for (var i = 1; i < length; i++)
            {
                firstRandomVar.Append(chars[random.Next(chars.Length)]);
            }

            var secondRandomVar = new StringBuilder();
            secondRandomVar.Append(charsSafe[random.Next(charsSafe.Length)]);
            for (var i = 1; i < length; i++)
            {
                secondRandomVar.Append(chars[random.Next(chars.Length)]);
            }

            builder.AppendLine($"set {firstRandomVar.ToString()}={secondRandomVar.ToString()}");

            GlobalConfig.AddOrUpdateVariable(firstRandomVar.ToString(), secondRandomVar.ToString());

            return (builder.ToString(), firstRandomVar.ToString(), secondRandomVar.ToString());
        }
    }
}
