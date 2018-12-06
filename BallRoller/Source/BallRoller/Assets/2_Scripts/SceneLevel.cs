using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLevel : MonoBehaviour {

    [SerializeField]
    GameObject _goal;
    public GameObject goal
    {
        get
        {
            return _goal;
        }
    }
    [SerializeField]
    GameObject _model;
    public GameObject model
    {
        get
        {
            return _model;
        }
    }
}
