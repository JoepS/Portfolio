using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateLevelList {
    [MenuItem("Assets/Create/Level Data List")]
    public static LevelDataList CreateLevelDataList()
    {
        LevelDataList asset = ScriptableObject.CreateInstance<LevelDataList>();

        AssetDatabase.CreateAsset(asset, "Assets/LevelDataList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

    [MenuItem("Assets/Create/Level")]
    public static Level CreateLevel()
    {
        Level asset = ScriptableObject.CreateInstance<Level>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(Level).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        return asset;
    }
}
