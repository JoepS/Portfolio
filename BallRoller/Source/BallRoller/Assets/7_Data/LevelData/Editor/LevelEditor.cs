using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditor : EditorWindow {
    public LevelDataList _levelDataList;
    private int viewIndex = -1;

    [MenuItem("Window/Level Data Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            _levelDataList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(LevelDataList)) as LevelDataList;
        }    
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Level Data Editor", EditorStyles.boldLabel);
        if(_levelDataList != null)
        {
            if(GUILayout.Button("Show level list"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = _levelDataList;
            }
        }
        if(GUILayout.Button("Open level list"))
        {
            OpenLevelList();
        }
        if(GUILayout.Button("New level list"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = _levelDataList;
        }
        GUILayout.EndHorizontal();
        if(_levelDataList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if(GUILayout.Button("Create new level list", GUILayout.ExpandWidth(false)))
            {
                CreateNewLevelList();
            }
            if(GUILayout.Button("Open Existing level list", GUILayout.ExpandWidth(false)))
            {
                OpenLevelList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if(_levelDataList != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if(GUILayout.Button("Previous", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                    viewIndex--;
            }
            GUILayout.Space(5);
            
            if(GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < _levelDataList.levelList.Count)
                    viewIndex++;
            }

            GUILayout.Space(60);

            if(GUILayout.Button("Add Level", GUILayout.ExpandWidth(false))){
                AddLevel();
            }
            if(GUILayout.Button("Delete Level", GUILayout.ExpandWidth(false)))
            {
                DeleteLevel(viewIndex - 1);
            }

            GUILayout.EndHorizontal();

            if (_levelDataList.levelList == null)
                Debug.Log("Something went wrong");
            if(_levelDataList.levelList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Level", viewIndex, GUILayout.ExpandWidth(false)), 1, _levelDataList.levelList.Count);
                EditorGUILayout.LabelField("of " + _levelDataList.levelList.Count.ToString() + " items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                _levelDataList.levelList[viewIndex - 1].name = EditorGUILayout.TextField("Level name", _levelDataList.levelList[viewIndex - 1].name as string);
                _levelDataList.levelList[viewIndex - 1].levelNumber = EditorGUILayout.IntField("Level number", _levelDataList.levelList[viewIndex - 1].levelNumber, GUILayout.ExpandWidth(false));
                _levelDataList.levelList[viewIndex - 1].previewImage = EditorGUILayout.ObjectField("Level Preview Image", _levelDataList.levelList[viewIndex - 1].previewImage, typeof(Sprite), false) as Sprite;


            }
            else
            {
                GUILayout.Label("The level list is empty");
            }

        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_levelDataList);
        }
    }

    void CreateNewLevelList()
    {
        viewIndex = 1;
        _levelDataList = CreateLevelList.CreateLevelDataList();
        if (_levelDataList)
        {
            _levelDataList.levelList = new List<Level>();
            string relPath = AssetDatabase.GetAssetPath(_levelDataList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenLevelList()
    {
        string absPath = EditorUtility.OpenFilePanel("Select level item list", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            _levelDataList = AssetDatabase.LoadAssetAtPath(relPath, typeof(LevelDataList)) as LevelDataList;
            if (_levelDataList.levelList == null)
                _levelDataList.levelList = new List<Level>();
            if (_levelDataList)
                EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void AddLevel()
    {
        Level newLevel = new Level();
        newLevel.name = "New Level";
        _levelDataList.levelList.Add(newLevel);
        viewIndex = _levelDataList.levelList.Count;
    }

    void DeleteLevel(int index)
    {
        _levelDataList.levelList.RemoveAt(index);
    }
}
