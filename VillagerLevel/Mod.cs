using BepInEx;
using HarmonyLib;

namespace VillagerLevel {
    [BepInPlugin(GUID, Name, Version)]
    public class Mod : BaseUnityPlugin {
        public const string Name = "Villager Level";
        public const string GUID = "com.maxsch.stacklands.villagerlevel";
        public const string Version = "0.0.0";

        private static Harmony harmony;

        public void Awake() {
            Log.Init(Logger);

            harmony = new Harmony(GUID);
            harmony.PatchAll();
        }
    }
}
