using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    const string dbName = "local.db";
    public static GameController Instance;
    public static DatabaseController db;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        db = new DatabaseController(dbName);
        db.connection.Table<Highscore>();
        Instance = this;
        DontDestroyOnLoad(this);
    }

    LevelController _levelController;
    public LevelController levelController
    {
        get
        {
            if (_levelController == null)
                _levelController = this.GetComponent<LevelController>();
            return _levelController;
        }
    }

    SceneController _sceneController;
    public SceneController sceneController
    {
        get
        {
            if (_sceneController == null)
                _sceneController = this.GetComponent<SceneController>();
            return _sceneController;
        }
    }

    NetworkController _networkController;
    public NetworkController networkController
    {
        get
        {
            if (_networkController == null)
                _networkController = this.GetComponent<NetworkController>();
            return _networkController;
        }
    }

    BallSkinController _ballSkinController;
    public BallSkinController ballSkinController
    {
        get
        {
            if (_ballSkinController == null)
                _ballSkinController = this.GetComponent<BallSkinController>();
            return _ballSkinController;
        }
    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
