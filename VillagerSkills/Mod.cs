using System.IO;
using System.Reflection;
using BepInEx;
using BerryLoaderNS;
using HarmonyLib;
using UnityEngine;

namespace VillagerSkills {
    [BepInDependency("BerryLoader")]
    [BepInPlugin(GUID, Name, Version)]
    public class Mod : BaseUnityPlugin {
        public const string Name = "Villager Skills";
        public const string GUID = "com.maxsch.stacklands.villagerskills";
        public const string Version = "0.0.0";

        private static AssetBundle assetBundle;
        public static GameObject SkillUIPrefab { get; private set; }
        public static GameObject ColumnPrefab { get; private set; }
        public static GameObject RowPrefab { get; private set; }

        private static Harmony harmony;

        public void Awake() {
            Log.Init(Logger);

            harmony = new Harmony(GUID);
            harmony.PatchAll();

            string localizationsPath = Path.Combine(Path.GetDirectoryName(Info.Location), "localization.txt");
            LocAPI.LoadTsvFromFile(localizationsPath);
        }

        private void Start() {
            assetBundle = LoadAssetBundle("VillagerSkills.villager_skills_asset_bundle");
            SkillUIPrefab = assetBundle.LoadAsset<GameObject>("SkillsUI");
            ColumnPrefab = assetBundle.LoadAsset<GameObject>("TableColumn");
            RowPrefab = assetBundle.LoadAsset<GameObject>("TableRow");
            assetBundle.Unload(false);
        }

        public static AssetBundle LoadAssetBundle(string resourceName) {
            Assembly execAssembly = Assembly.GetExecutingAssembly();

            using (Stream stream = execAssembly.GetManifestResourceStream(resourceName)) {
                return AssetBundle.LoadFromStream(stream);
            }
        }
    }
}
