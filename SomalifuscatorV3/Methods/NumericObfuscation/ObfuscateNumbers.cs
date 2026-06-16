using SomalifuscatorV3.GlobalSettings;
using System.Diagnostics;

namespace SomalifuscatorV3.Methods.NumericObfuscation
{
    public class ObfuscateNumbers
    {
        public static string ObfuscateNumber(string number)
        {
            if (!Settings.UseNumericObfuscation || !IsDecimalNumber(number))
            {
                return number;
            }

            return AddOrSubtractRandomEQ(number);
        }

        private static bool IsDecimalNumber(string number)
        {
            return int.TryParse(number, out _);
        }

        private static string AddOrSubtractRandomEQ(string numberToObf)
        {
            Debug.WriteLine($"Obfuscating number: {numberToObf}");

            var random = new Random();
            var number1 = random.Next(1, 10000);
            var signs = new[] { "+", "-" };
            var sign1 = signs[random.Next(0, 2)];
            var oppositeSign1 = sign1 == "+" ? "-" : "+";

            var finalNumber = $"{numberToObf} {sign1} {number1}";
            var outFinal = EvaluateExpression(finalNumber);

            return MBAObfuscation.ApplyMBAObfuscation(outFinal.ToString(), number1.ToString(), oppositeSign1);
        }

        private static int EvaluateExpression(string expression)
        {
            var data = expression.Split(' ');
            var result = int.Parse(data[0]);
            var sign = data[1];
            var number = int.Parse(data[2]);

            if (sign == "+")
            {
                result += number;
            }
            else if (sign == "-")
            {
                result -= number;
            }

            return result;
        }
    }
}
