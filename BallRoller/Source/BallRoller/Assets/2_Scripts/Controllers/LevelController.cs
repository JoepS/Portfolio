using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelController : MonoBehaviour {
    [SerializeField] List<MeshData> _levels;
    public List<MeshData> levels
    {
        get
        {
            return _levels;
        }
    }

    void Awake()
    {
        _levels = MeshData.All();
    }

    void Start()
    {

    }

    public void ReloadLevels()
    {
        _levels = MeshData.All();
    }
}
