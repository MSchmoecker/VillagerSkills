using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle : MonoBehaviour {
    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles() {
        Directory.CreateDirectory("AssetBundles/StandaloneWindows");
        BuildPipeline.BuildAssetBundles("AssetBundles/StandaloneWindows", BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows);
        FileUtil.ReplaceFile("AssetBundles/StandaloneWindows/villager_skills_asset_bundle", "../VillagerSkills/villager_skills_asset_bundle");
    }
}
