using System.Text;
using SomalifuscatorV3.GlobalSettings;
using SomalifuscatorV3.Obfuscation;
using Spectre.Console;

namespace SomalifuscatorV3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var executionTimer = new ExecutionTimer();
            executionTimer.Start();

            try
            {
                var config = ParseCommandLineArgs(args);
                DisplayConfiguration(config);
                ProcessFile(config);
            }
            catch (FileNotFoundException ex)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] The file was not found. {ex.Message}");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An error occurred:[/] {ex.Message}");
                Environment.Exit(1);
            }

            executionTimer.Stop();
            DisplayExecutionTime(executionTimer.GetElapsedTime());
        }

        private struct ObfuscationConfig
        {
            public string InputFile { get; set; }
            public string OutputFile { get; set; }

            public ObfuscationConfig(string inputFile, string outputFile)
            {
                InputFile = inputFile;
                OutputFile = outputFile;
            }
        }

        private class ExecutionTimer
        {
            private DateTime _startTime;
            private DateTime _endTime;

            public void Start() => _startTime = DateTime.Now;
            public void Stop() => _endTime = DateTime.Now;
            public TimeSpan GetElapsedTime() => _endTime - _startTime;
        }

        private static ObfuscationConfig ParseCommandLineArgs(string[] args)
        {
            string? inputFile = null;
            string? outputFile = null;

            for (int i = 0; i < args.Length; i++)
            {
                string loweredArg = args[i].ToLowerInvariant();

                switch (loweredArg)
                {
                    case "--debug":
                        Settings.Debug = true;
                        break;

                    case "--no-debug":
                        Settings.Debug = false;
                        break;

                    case "--verbose":
                        Settings.Verbose = true;
                        break;

                    case "--no-verbose":
                        Settings.Verbose = false;
                        break;

                    case "--all-runtime-checks":
                        Settings.SetAllRuntimeChecks(true);
                        break;

                    case "--no-all-runtime-checks":
                    case "--disable-all-runtime-checks":
                        Settings.SetAllRuntimeChecks(false);
                        break;

                    case "--check-command-lengths":
                        Settings.CheckCommandLengths = true;
                        break;

                    case "--no-check-command-lengths":
                        Settings.CheckCommandLengths = false;
                        break;

                    case "--anti-analysis":
                        Settings.AntiAnalysis = true;
                        break;

                    case "--no-anti-analysis":
                        Settings.AntiAnalysis = false;
                        break;

                    case "--dead-code":
                        Settings.DeadCode = true;
                        break;

                    case "--no-dead-code":
                        Settings.DeadCode = false;
                        break;

                    case "--require-wifi":
                        Settings.RequireWifi = true;
                        break;

                    case "--no-require-wifi":
                        Settings.RequireWifi = false;
                        break;

                    case "--require-admin":
                        Settings.RequireAdmin = true;
                        break;

                    case "--no-require-admin":
                        Settings.RequireAdmin = false;
                        break;

                    case "--vm-check":
                    case "--virtual-machine-check":
                        Settings.VirtualMachineCheck = true;
                        break;

                    case "--no-vm-check":
                    case "--no-virtual-machine-check":
                        Settings.VirtualMachineCheck = false;
                        break;

                    case "--all-obf":
                    case "--all-obfuscation":
                        Settings.SetAllObfuscation(true);
                        break;

                    case "--no-all-obf":
                    case "--no-all-obfuscation":
                    case "--disable-all-obfuscation":
                        Settings.SetAllObfuscation(false);
                        break;

                    case "--format-input":
                        Settings.FormatInput = true;
                        break;

                    case "--no-format-input":
                    case "--preserve-input-format":
                        Settings.FormatInput = false;
                        break;

                    case "--block-obf":
                    case "--block-obfuscation":
                    case "--enable-block-obfuscation":
                        Settings.UseBlockObfuscation = true;
                        break;

                    case "--no-block-obf":
                    case "--no-block-obfuscation":
                    case "--disable-block-obfuscation":
                        Settings.UseBlockObfuscation = false;
                        break;

                    case "--shuffle-blocks":
                    case "--block-shuffling":
                        Settings.UseBlockShuffling = true;
                        break;

                    case "--no-shuffle-blocks":
                    case "--no-block-shuffling":
                        Settings.UseBlockShuffling = false;
                        break;

                    case "--line-prefix-obf":
                    case "--line-prefix-obfuscation":
                        Settings.UseLinePrefixObfuscation = true;
                        break;

                    case "--no-line-prefix-obf":
                    case "--no-line-prefix-obfuscation":
                        Settings.UseLinePrefixObfuscation = false;
                        break;

                    case "--char-obf":
                    case "--character-obf":
                    case "--character-obfuscation":
                        Settings.UseCharacterObfuscation = true;
                        break;

                    case "--no-char-obf":
                    case "--no-character-obf":
                    case "--no-character-obfuscation":
                        Settings.UseCharacterObfuscation = false;
                        break;

                    case "--variable-generation":
                    case "--var-generation":
                    case "--generated-vars":
                        Settings.UseVariableGeneration = true;
                        break;

                    case "--no-variable-generation":
                    case "--no-var-generation":
                    case "--no-generated-vars":
                        Settings.UseVariableGeneration = false;
                        break;

                    case "--variable-slicing":
                    case "--var-slicing":
                    case "--slice-vars":
                        Settings.UseVariableSlicing = true;
                        break;

                    case "--no-variable-slicing":
                    case "--no-var-slicing":
                    case "--no-slice-vars":
                        Settings.UseVariableSlicing = false;
                        break;

                    case "--static-var-noise":
                    case "--static-variable-noise":
                        Settings.UseStaticVariableNoise = true;
                        break;

                    case "--no-static-var-noise":
                    case "--no-static-variable-noise":
                        Settings.UseStaticVariableNoise = false;
                        break;

                    case "--numeric-obf":
                    case "--numeric-obfuscation":
                        Settings.UseNumericObfuscation = true;
                        break;

                    case "--no-numeric-obf":
                    case "--no-numeric-obfuscation":
                        Settings.UseNumericObfuscation = false;
                        break;

                    case "--input":
                    case "-i":
                        if (i + 1 < args.Length)
                        {
                            inputFile = args[++i];
                        }
                        break;

                    case "--output":
                    case "-o":
                        if (i + 1 < args.Length)
                        {
                            outputFile = args[++i];
                        }
                        break;

                    case "--help":
                    case "-h":
                        DisplayHelp();
                        Environment.Exit(0);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(inputFile))
                throw new ArgumentException("Input file is required. Use -i or --input <file>.");

            if (string.IsNullOrWhiteSpace(outputFile))
                throw new ArgumentException("Output file is required. Use -o or --output <file>.");

            return new ObfuscationConfig(inputFile, outputFile);
        }

        private static void DisplayConfiguration(ObfuscationConfig config)
        {
            AnsiConsole.MarkupLine($"[cyan]Input File:[/] {config.InputFile}");
            AnsiConsole.MarkupLine($"[cyan]Output File:[/] {config.OutputFile}");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]Runtime Settings:[/]");
            DisplaySetting("Debug", Settings.Debug);
            DisplaySetting("Verbose", Settings.Verbose);
            DisplaySetting("Check Command Lengths", Settings.CheckCommandLengths);
            DisplaySetting("Anti Analysis", Settings.AntiAnalysis);
            DisplaySetting("Dead Code", Settings.DeadCode);
            DisplaySetting("Require Wifi", Settings.RequireWifi);
            DisplaySetting("Require Admin", Settings.RequireAdmin);
            DisplaySetting("Virtual Machine Check", Settings.VirtualMachineCheck);
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]Obfuscation Settings:[/]");
            DisplaySetting("Format Input", Settings.FormatInput);
            DisplaySetting("Block Obfuscation", Settings.UseBlockObfuscation);
            DisplaySetting("Block Shuffling", Settings.UseBlockShuffling);
            DisplaySetting("Line Prefix Obfuscation", Settings.UseLinePrefixObfuscation);
            DisplaySetting("Character Obfuscation", Settings.UseCharacterObfuscation);
            DisplaySetting("Generated Variables", Settings.UseVariableGeneration);
            DisplaySetting("Variable Slicing", Settings.UseVariableSlicing);
            DisplaySetting("Static Variable Noise", Settings.UseStaticVariableNoise);
            DisplaySetting("Numeric Obfuscation", Settings.UseNumericObfuscation);
            AnsiConsole.WriteLine();
        }

        private static void DisplaySetting(string label, bool enabled)
        {
            AnsiConsole.MarkupLine($"  [cyan]{label}:[/] [{(enabled ? "green" : "red")}]{(enabled ? "Enabled" : "Disabled")}[/]");
        }

        private static void ProcessFile(ObfuscationConfig config)
        {
            if (!File.Exists(config.InputFile))
                throw new FileNotFoundException($"Input file '{config.InputFile}' does not exist.");

            string fileContent = File.ReadAllText(config.InputFile);
            var obfuscator = new Obfuscator(fileContent);

            string obfuscatedCode = obfuscator.Obfuscate();
            obfuscatedCode = NormalizeLineEndings(obfuscatedCode);

            File.WriteAllBytes(config.OutputFile, Encoding.UTF8.GetBytes(obfuscatedCode));
            AnsiConsole.MarkupLine($"[green]Obfuscated file written to:[/] {config.OutputFile}");
        }

        private static string NormalizeLineEndings(string code)
        {
            return code.Replace("\n\n", "\n");
        }

        private static void DisplayExecutionTime(TimeSpan elapsed)
        {
            AnsiConsole.MarkupLine($"\n[yellow]Obfuscation completed in:[/] " +
                $"[bold green]{elapsed.Hours:D2}[/]h " +
                $"[bold green]{elapsed.Minutes:D2}[/]m " +
                $"[bold green]{elapsed.Seconds:D2}[/]s " +
                $"[bold green]{elapsed.Milliseconds:D3}[/]ms");
        }

        private static void DisplayHelp()
        {
            AnsiConsole.MarkupLine("[bold cyan]SomalifuscatorV3[/] - Batch File Obfuscation Tool");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]Usage:[/]");
            AnsiConsole.WriteLine("  SomalifuscatorV3.exe [options]");
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[yellow]Options:[/]");
            AnsiConsole.MarkupLine("  [green]-i, --input <file>[/]                  Input file path");
            AnsiConsole.MarkupLine("  [green]-o, --output <file>[/]                 Output file path");
            AnsiConsole.MarkupLine("  [green]--debug | --no-debug[/]                Toggle debug mode");
            AnsiConsole.MarkupLine("  [green]--verbose | --no-verbose[/]            Toggle verbose output");
            AnsiConsole.MarkupLine("  [green]--all-runtime-checks | --no-all-runtime-checks[/] Toggle runtime checks");
            AnsiConsole.MarkupLine("  [green]--check-command-lengths | --no-check-command-lengths[/] Toggle command length checks");
            AnsiConsole.MarkupLine("  [green]--anti-analysis | --no-anti-analysis[/] Toggle anti-analysis checks");
            AnsiConsole.MarkupLine("  [green]--dead-code | --no-dead-code[/]        Toggle dead code generation");
            AnsiConsole.MarkupLine("  [green]--require-wifi | --no-require-wifi[/]  Toggle wifi requirement");
            AnsiConsole.MarkupLine("  [green]--require-admin | --no-require-admin[/] Toggle admin requirement");
            AnsiConsole.MarkupLine("  [green]--vm-check | --no-vm-check[/]          Toggle virtual machine check");
            AnsiConsole.MarkupLine("  [green]--all-obf | --no-all-obf[/]            Toggle every obfuscation setting");
            AnsiConsole.MarkupLine("  [green]--format-input | --no-format-input[/]  Toggle trimming comments/blank lines");
            AnsiConsole.MarkupLine("  [green]--block-obf | --no-block-obf[/]        Toggle goto block obfuscation");
            AnsiConsole.MarkupLine("  [green]--shuffle-blocks | --no-shuffle-blocks[/] Toggle block shuffling");
            AnsiConsole.MarkupLine("  [green]--line-prefix-obf | --no-line-prefix-obf[/] Toggle random line prefixes");
            AnsiConsole.MarkupLine("  [green]--char-obf | --no-char-obf[/]          Toggle per-character obfuscation");
            AnsiConsole.MarkupLine("  [green]--variable-generation | --no-variable-generation[/] Toggle generated variable blocks");
            AnsiConsole.MarkupLine("  [green]--variable-slicing | --no-variable-slicing[/] Toggle %var:~index,length% slicing");
            AnsiConsole.MarkupLine("  [green]--static-var-noise | --no-static-var-noise[/] Toggle static fake-variable noise");
            AnsiConsole.MarkupLine("  [green]--numeric-obf | --no-numeric-obf[/]    Toggle numeric pointer obfuscation");
            AnsiConsole.MarkupLine("  [green]-h, --help[/]                          Show this help message");
        }
    }
}
