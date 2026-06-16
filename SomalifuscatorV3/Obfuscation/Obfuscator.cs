using SomalifuscatorV3.GlobalSettings;
using SomalifuscatorV3.Methods.Formatting;
using SomalifuscatorV3.Methods.WalkThroughObfuscation;

namespace SomalifuscatorV3.Obfuscation
{
    public class Obfuscator
    {
        private string _file_content;

        public Obfuscator(string file_contents)
        {
            _file_content = file_contents;
        }

        public string Obfuscate()
        {
            string formattedLine = Settings.FormatInput ? Format.FormatCode(_file_content) : _file_content;

            Walk walker = new Walk();
            string obfuscated = walker.WalkThrough(formattedLine.Split('\n').ToList());

            return obfuscated;
        }
    }
}
