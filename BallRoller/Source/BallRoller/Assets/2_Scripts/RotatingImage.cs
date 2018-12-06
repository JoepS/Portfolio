using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingImage : MonoBehaviour {

    float _speed;
    public float speed
    {
        set
        {
            _speed = value;
        }
    }

    bool _rotating;
    public bool rotating
    {
        set
        {
            _rotating = value;
        }
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (_rotating)
        {
            this.transform.Rotate(Vector3.forward, Time.deltaTime * _speed);
        }
	}
}
