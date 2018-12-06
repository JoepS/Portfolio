using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AddNewLevel : MonoBehaviour {

    [SerializeField] string name;
    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject goalLocation;
    [SerializeField] int levelNumber;

    [SerializeField] GameObject level;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(SaveLevel(level));
        }
	}

    IEnumerator SaveLevel(GameObject go)
    {
        Mesh mesh = go.GetComponentInChildren<MeshFilter>().mesh;
        MeshData data = null;
        List<MeshData> results = MeshData.All().Where(x => x.name == name).ToList();
        if(results.Count > 0)
        {
            data = results.First();
        }
        bool newdata = false;
        if (data == null)
        {
            data = new MeshData();
            data.name = name;
            newdata = true;
        }
        data._triangles = mesh.triangles;
        data._vertices = mesh.vertices;
        data._uv1 = mesh.uv2;
        data._uv2 = mesh.uv3;
        data._normals = mesh.normals;
        data._spawnLocation = spawnLocation.transform.localPosition;
        data._goalLocation = goalLocation.transform.localPosition;
        data.levelNumber = levelNumber;
        if (newdata)
        {
            data.New();
        }
        else
        {
            data.Save();
        }
        Debug.Log("Saved"); 
        yield return null;

    }
}
