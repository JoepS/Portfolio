using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPreview : MonoBehaviour {

    [SerializeField]
    float _rotationSpeed;

    [SerializeField]
    Vector3 _direction;

 	// Update is called once per frame
	void Update () {
        this.transform.Rotate(_direction, Time.deltaTime * _rotationSpeed);
	}
}
