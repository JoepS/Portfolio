using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    [SerializeField] Rigidbody _ball;
    [SerializeField] float _maxVelocity;
    
    Vector3 _pauzedVelocity;

    [SerializeField] Camera _folowCamera;
    [SerializeField]
    Light _light;

    [SerializeField] float _respawnLevel;

    [SerializeField]
    Canvas _locatorCanvas;
    [SerializeField]
    GameObject _pointerBase;
    [SerializeField]
    GameObject _goal;

    string _goalColliderName = "Goal";

    Vector3 _respawnLocation;
    public Vector3 respawnLocation
    {
        set
        {
            _respawnLocation = value;
        }
    }

    bool _controllsLocked = true;
    public bool controllsLocked
    {
        set
        {
            if (value)
            {
                _pauzedVelocity = _ball.velocity;
                _ball.useGravity = false;
            }
            else
            {
                _ball.velocity = _pauzedVelocity;
                _ball.useGravity = true;
            }
            _controllsLocked = value;
        }
    }

    SceneGameController _sceneGameController;
    public SceneGameController sceneGameController
    {
        set
        {
            _sceneGameController = value;
        }
    }
    BallSkinController _ballSkinController;
    [SerializeField]
    Material _ballMaterial;

	// Use this for initialization
	void Start () {
        _ballSkinController = GameController.Instance.ballSkinController;
        _ballMaterial.mainTexture = _ballSkinController.selected._texture;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 ballpos = _ball.position;
        ballpos.y += 10;
        _light.transform.position = ballpos;
        ballpos.z -= 1.5f;
        _folowCamera.transform.position = ballpos;
        _folowCamera.transform.LookAt(_ball.transform);
        _light.transform.LookAt(_ball.transform);
        if (!_controllsLocked)
        {
            _ball.isKinematic = false;
#if UNITY_EDITOR
            EditorUpdate();
#else
        MobileUpdate();
#endif
        }
        else
        {
            _ball.velocity = Vector3.zero;
            _ball.isKinematic = true;
        }
        if(this.transform.position.y <= _respawnLevel)
        {
            this.transform.position = _respawnLocation;
            _ball.velocity = Vector3.zero;
        }
        _locatorCanvas.transform.rotation = Quaternion.Euler(0, 0, 0);
        _pointerBase.transform.LookAt(_goal.transform);
        Vector3 rot = _pointerBase.transform.localRotation.eulerAngles;
        rot.x = 90;
        //rot.y = 0;  
        _pointerBase.transform.localRotation = Quaternion.Euler(rot);
    }

    void MobileUpdate()
    {
        float xVelocity = Input.acceleration.x;
        float zVelocity = Input.acceleration.y;

        Vector3 velocity = _ball.velocity;
        velocity.x += xVelocity;
        velocity.z += zVelocity;
        if (velocity.x > _maxVelocity)
            velocity.x = _maxVelocity;
        else if (velocity.x < -_maxVelocity)
            velocity.x = -_maxVelocity;
        if (velocity.z > _maxVelocity)
            velocity.z = _maxVelocity;
        else if (velocity.z < -_maxVelocity)
            velocity.z = -_maxVelocity;
        _ball.velocity = velocity;
    }

    void EditorUpdate()
    {
        float xVelocity = Input.GetAxis("Horizontal");
        float zVelocity = Input.GetAxis("Vertical");
        Vector3 velocity = _ball.velocity;
        velocity.x += xVelocity;
        velocity.z += zVelocity;
        if (velocity.x > _maxVelocity)
            velocity.x = _maxVelocity;
        else if (velocity.x < -_maxVelocity)
            velocity.x = -_maxVelocity;
        if (velocity.z > _maxVelocity)
            velocity.z = _maxVelocity;
        else if (velocity.z < -_maxVelocity)
            velocity.z = -_maxVelocity;
        _ball.velocity = velocity;
    }

    bool isGrounded()
    {
        return Physics.Raycast(_ball.transform.position, -Vector3.up, _ball.GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger");
        if (other.transform.parent.name.Equals(_goalColliderName) && !_controllsLocked)
        {
            //if (Vector3.Distance(other.transform.position, this.transform.position) < 0.25)
            //{
                this.controllsLocked = true;
                _sceneGameController.finishedLevel();
            //}
        }
    }
}
