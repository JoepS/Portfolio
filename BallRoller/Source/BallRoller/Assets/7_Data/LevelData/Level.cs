using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : ScriptableObject{
    [Header("Level Data")]
    public new string name = "LevelName";
    public int levelNumber = -1;
    public Sprite previewImage;
    [Header("Scene Data")]
    public Vector3 spawnLocation;
    public Vector3 goalLocation;
    public GameObject levelMesh;
}
