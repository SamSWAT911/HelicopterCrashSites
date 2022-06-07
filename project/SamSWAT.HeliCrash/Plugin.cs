using BepInEx;
using BepInEx.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace SamSWAT.HeliCrash
{
    [BepInPlugin("com.SamSWAT.HeliCrash", "SamSWAT.HeliCrash", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static string Directory;
        public static HeliCrashLocations heliCrashSites;
        internal static ConfigEntry<int> HeliCrashChance;
        
        private void Awake()
        {
            Directory = Path.Combine(BepInEx.Paths.PluginPath, "SamSWAT.HeliCrash/").Replace("\\", "/");
            new HeliCrashPatch().Enable();
            var json = new StreamReader(Directory + "HeliCrashLocations.json").ReadToEnd();
            heliCrashSites = JsonConvert.DeserializeObject<HeliCrashLocations>(json);

            HeliCrashChance = Config.Bind(
                "Main Settings",
                "Helicopter crash site chance",
                10,
                new ConfigDescription("Chance of helicopter crash site appearance in percentages",
                new AcceptableValueRange<int>(0, 100)));
        }
    }
}
