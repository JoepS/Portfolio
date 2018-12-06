using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCreator : MonoBehaviour {

    [SerializeField] SceneLevel level;
    [SerializeField]
    Image image;

	// Use this for initialization
	void Start () {
        StartCoroutine(CreateLevel());
	}

    IEnumerator CreateLevel()
    {

        MeshData data = MeshData.All()[0];

        Mesh m = level.model.GetComponent<MeshFilter>().mesh;
        Debug.Log(data._triangles.Length);
        Debug.Log(data._vertices.Length);
        Debug.Log(data._uv1.Length);
        Debug.Log(data._uv2.Length);
        Debug.Log(data._normals.Length);

        m.vertices = data._vertices;
        m.triangles = data._triangles;
        if (data._uv1.Length == data._vertices.Length)
            m.uv2 = data._uv1;
        else
            m.uv2 = new Vector2[data._vertices.Length];
        if (data._uv2.Length == data._vertices.Length)
            m.uv3 = data._uv2;
        else
            m.uv3 = new Vector2[data._vertices.Length];
        m.normals = data._normals;

        level.model.GetComponent<MeshFilter>().mesh = m;
        level.model.GetComponent<MeshCollider>().sharedMesh = m;

        level.goal.transform.position = data._goalLocation;
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
