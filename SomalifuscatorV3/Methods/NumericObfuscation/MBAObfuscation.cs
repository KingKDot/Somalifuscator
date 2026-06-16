namespace SomalifuscatorV3.Methods.NumericObfuscation
{
    public class MBAObfuscation
    {
        public static readonly Dictionary<string, string> OperationRules = new()
                    {
                        { "+", "((LEFT ^& RIGHT)+(LEFT ^| RIGHT))" },
                        { "-", "(LEFT ^^ -RIGHT)+(2*(LEFT ^& -RIGHT))" },
                        { "&", "((LEFT)&(RIGHT))" },
                        { "||", "((LEFT)||(RIGHT))" },
                        { "<<", "((LEFT)<<(RIGHT))" },
                        { ">>", "((LEFT)>>(RIGHT))" },
                        { "*", "((LEFT)*(RIGHT))" },
                        { "/", "((LEFT)/(RIGHT))" },
                    };

        public static string ApplyMBAObfuscation(string left, string right, string operatorKey, int depth = 1)
        {
            if (!OperationRules.TryGetValue(operatorKey, out string? operation))
            {
                Console.WriteLine("Invalid Operation");
                Console.WriteLine(left);
                Console.WriteLine(right);
                Console.WriteLine(operatorKey);
                Console.ReadKey();
                Environment.Exit(0);
            }

            operation = operation.Replace("LEFT", left).Replace("RIGHT", right);

            if (depth > 1)
            {
                var expressions = ParseBinaryExpressions(operation);
                foreach (var expr in expressions)
                {
                    string? newOp = expr.Operator switch
                    {
                        "&" => ApplyMBAObfuscation(expr.Left, expr.Right, "Band", depth - 1),
                        "|" => ApplyMBAObfuscation(expr.Left, expr.Right, "Bor", depth - 1),
                        "^" => ApplyMBAObfuscation(expr.Left, expr.Right, "Bxor", depth - 1),
                        "+" => ApplyMBAObfuscation(expr.Left, expr.Right, "Plus", depth - 1),
                        "-" => ApplyMBAObfuscation(expr.Left, expr.Right, "Subtract", depth - 1),
                        _ => null
                    };

                    if (newOp != null)
                    {
                        var randomTransformations = new List<string>
                                {
                                    $"(((({newOp})<<1)>>1))",
                                    $"(~~({newOp}))",
                                    $"((({newOp})+0)-0)",
                                    $"(({newOp})&0xFFFFFFFF)"
                                };
                        var randomTransformation = randomTransformations[new Random().Next(randomTransformations.Count)];
                        operation = operation.Replace(expr.FullExpression, $"({randomTransformation})");
                    }
                }
            }

            return operation;
        }

        public static List<BinaryExpression> ParseBinaryExpressions(string input)
        {
            var operators = new[] { "&", "|", "^", "+", "-", "*", "/", "%%", "<<", ">>" };
            var expressions = new List<BinaryExpression>();

            foreach (var op in operators)
            {
                int index = input.IndexOf(op);
                if (index > 0)
                {
                    string left = input[..index].Trim();
                    string right = input[(index + op.Length)..].Trim();
                    expressions.Add(new BinaryExpression(left, right, op, input));
                }
            }

            return expressions;
        }
    }

    public class BinaryExpression
    {
        public string Left { get; }
        public string Right { get; }
        public string Operator { get; }
        public string FullExpression { get; }

        public BinaryExpression(string left, string right, string @operator, string fullExpression)
        {
            Left = left;
            Right = right;
            Operator = @operator;
            FullExpression = fullExpression;
        }
    }
}
