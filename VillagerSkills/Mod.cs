using System.IO;
using BepInEx;
using BerryLoaderNS;
using HarmonyLib;

namespace VillagerSkills {
    [BepInDependency("BerryLoader")]
    [BepInPlugin(GUID, Name, Version)]
    public class Mod : BaseUnityPlugin {
        public const string Name = "Villager Skills";
        public const string GUID = "com.maxsch.stacklands.villagerskills";
        public const string Version = "0.0.0";

        private static Harmony harmony;

        public void Awake() {
            Log.Init(Logger);

            harmony = new Harmony(GUID);
            harmony.PatchAll();

            string localizationsPath = Path.Combine(Path.GetDirectoryName(Info.Location), "localization.txt");
            LocAPI.LoadTsvFromFile(localizationsPath);
        }
    }
}
