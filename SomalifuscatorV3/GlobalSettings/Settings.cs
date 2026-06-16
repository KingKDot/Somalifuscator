using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomalifuscatorV3.GlobalSettings
{
    public static class Settings
    {
        public static int Version = 3;
        public static bool Debug = false;
        public static bool Verbose = false;
        public static bool CheckCommandLengths = true;
        public static bool AntiAnalysis = true;
        public static bool DeadCode = true;
        public static bool RequireWifi = true;
        public static bool RequireAdmin = true;
        public static bool VirtualMachineCheck = true;

        public static bool FormatInput = true;
        public static bool UseBlockObfuscation = true;
        public static bool UseBlockShuffling = true;
        public static bool UseLinePrefixObfuscation = true;
        public static bool UseCharacterObfuscation = true;
        public static bool UseVariableGeneration = true;
        public static bool UseVariableSlicing = true;
        public static bool UseStaticVariableNoise = true;
        public static bool UseNumericObfuscation = true;

        public static void SetAllObfuscation(bool enabled)
        {
            FormatInput = enabled;
            UseBlockObfuscation = enabled;
            UseBlockShuffling = enabled;
            UseLinePrefixObfuscation = enabled;
            UseCharacterObfuscation = enabled;
            UseVariableGeneration = enabled;
            UseVariableSlicing = enabled;
            UseStaticVariableNoise = enabled;
            UseNumericObfuscation = enabled;
        }

        public static void SetAllRuntimeChecks(bool enabled)
        {
            AntiAnalysis = enabled;
            DeadCode = enabled;
            RequireWifi = enabled;
            RequireAdmin = enabled;
            VirtualMachineCheck = enabled;
            CheckCommandLengths = enabled;
        }
    }
}
