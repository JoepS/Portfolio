using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class SceneGameController : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] GameObject _countDownPanel;
    [SerializeField] GameObject _menuPanel;
    [SerializeField] GameObject _finishedLevelPanel;
    [SerializeField] Button _nextLevelButton;
    [SerializeField] Text _finishTimeText;
    [SerializeField] Text _countDowntext;
    [SerializeField] Text _timerText;

    float _elapsedTime = 0;
    float _countDownTime = 3;

    public static MeshData LevelToLoad;

    [Header("Game Related")]
    [SerializeField]
    BallController _ball;
    [SerializeField] SceneLevel _levelContainer;
    [SerializeField]
    Material _heightMaterial;

    bool _gamePlaying = false;
    bool _countingDown = true;

    private void Awake()
    {
        if (LevelToLoad == null)
        {
            GameController.Instance.sceneController.GoToScene(SceneType.SceneMain);
            return;
        }
        loadLevel();
        _ball.sceneGameController = this;
        _ball.respawnLocation = LevelToLoad._spawnLocation;
        _ball.transform.position = LevelToLoad._spawnLocation;
    }
    // Use this for initialization
    void Start()
    {
        float heightmax = _levelContainer.model.GetComponent<MeshFilter>().mesh.bounds.extents.z * 2;
        float heightmin = -0.5f;
        _heightMaterial.SetFloat("_HeightMin", heightmin);
        _heightMaterial.SetFloat("_HeightMax", heightmax);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gamePlaying)
        {
            _elapsedTime += Time.deltaTime;
            int miliseconds = Mathf.RoundToInt((_elapsedTime - Mathf.RoundToInt(_elapsedTime)) * 1000);
            TimeSpan span = new TimeSpan(0, 0, 0, Mathf.RoundToInt(_elapsedTime), miliseconds);
            _timerText.text = "Time: " + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00") + ":" + span.Milliseconds.ToString("000");
            
        }
        else if (_countingDown) 
        {
            _countDowntext.text = "" + Mathf.RoundToInt(_countDownTime);
            _countDownTime -= Time.deltaTime;

            if (_countDownTime <= 0)
            {
                _countDownPanel.SetActive(false);
                _gamePlaying = true;
                _countingDown = false;  
                _ball.controllsLocked = false;
            }
        }

        _heightMaterial.SetFloat("_Height", _ball.transform.position.y-1);
    }

    void loadLevel()
    {
        Mesh m = new Mesh();
        m.vertices = LevelToLoad._vertices;
        m.triangles = LevelToLoad._triangles;
        if (LevelToLoad._uv1.Length == LevelToLoad._vertices.Length)
            m.uv2 = LevelToLoad._uv1;
        else
            m.uv2 = new Vector2[LevelToLoad._vertices.Length];
        if (LevelToLoad._uv2.Length == LevelToLoad._vertices.Length)
            m.uv3 = LevelToLoad._uv2;
        else
            m.uv3 = new Vector2[LevelToLoad._vertices.Length];
        m.normals = LevelToLoad._normals;


        _levelContainer.model.GetComponent<MeshFilter>().mesh = m;
        _levelContainer.model.GetComponent<MeshCollider>().sharedMesh = m;

        _levelContainer.goal.transform.position = LevelToLoad._goalLocation;
        //StartCoroutine(SaveLevel(go));
    }

 /*   IEnumerator SaveLevel(GameObject go)
    {
        Mesh mesh = go.GetComponentInChildren<MeshFilter>().mesh;
        MeshData data = new MeshData()
        {
            name = LevelToLoad.name,
            _triangles = mesh.triangles,
            _vertices = mesh.vertices,
            _uv1 = mesh.uv2,
            _uv2 = mesh.uv3,
            _normals = mesh.normals,
            _spawnLocation = LevelToLoad.spawnLocation,
            _goalLocation = LevelToLoad.goalLocation,
            levelNumber = LevelToLoad.levelNumber
        };
        data.New();
        Debug.Log("Saved"); 
        yield return null;

    }*/

    public void MenuButtonClick()
    {
        if (_countingDown)
            return;
        _gamePlaying = false;
        _ball.controllsLocked = true;
        _menuPanel.SetActive(true);
    }


    public void RetryButtonClick()
    {
        GameController.Instance.sceneController.GoToScene(SceneType.SceneGame);
    }

    public void ContinueButtonClick()
    {
        _gamePlaying = true;
        _ball.controllsLocked = false;
        _menuPanel.SetActive(false);
    }

    public void ExitButtonClick()
    {
        GameController.Instance.sceneController.GoToScene(SceneType.SceneMain);
    }

    public void finishedLevel()
    {
        _gamePlaying = false;
        _finishedLevelPanel.SetActive(true);

        int miliseconds = Mathf.RoundToInt((_elapsedTime - Mathf.RoundToInt(_elapsedTime)) * 1000);
        TimeSpan span = new TimeSpan(0, 0, 0, Mathf.RoundToInt(_elapsedTime), miliseconds);
        _finishTimeText.text = "You finished in " + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00") + ":" + span.Milliseconds.ToString("000");
        List<Highscore> all = Highscore.All();
        Highscore levelHighscore = null;
        float ft = 0;
        if (all != null && all.Count > 0)
        {
            IEnumerable<Highscore> hs = all.Where(x => x.levelnumber == LevelToLoad.levelNumber);
            if (hs.Count() > 0)
            {
                levelHighscore = hs.First();
                ft = levelHighscore.time;
            }
        }
        if (_elapsedTime < ft || ft == 0)
        {
            int pfmiliseconds = Mathf.RoundToInt((ft - Mathf.RoundToInt(ft)) * 1000);
            TimeSpan pfspan = new TimeSpan(0, 0, 0, Mathf.RoundToInt(ft), pfmiliseconds);
            string pftime = pfspan.Minutes.ToString("00") + ":" + pfspan.Seconds.ToString("00") + ":" + pfspan.Milliseconds.ToString("000");
            _finishTimeText.text += "\nYou beat your fastest time!\nThe previous fastest time was " + pftime;
            if (levelHighscore == null)
            {
                levelHighscore = new Highscore();
                levelHighscore.levelnumber = LevelToLoad.levelNumber;
                levelHighscore.New();
            }
            levelHighscore.time = _elapsedTime;
            levelHighscore.Save();

        }
        else if (_elapsedTime > ft)
        {
            int fmiliseconds = Mathf.RoundToInt((ft - Mathf.RoundToInt(ft)) * 1000);
            TimeSpan fspan = new TimeSpan(0, 0, 0, Mathf.RoundToInt(ft), fmiliseconds);
            string ftime = fspan.Minutes.ToString("00") + ":" + fspan.Seconds.ToString("00") + ":" + fspan.Milliseconds.ToString("000");
            _finishTimeText.text += "\nYour fastest time is " + ftime;
        }
        else
        {
            Debug.Log("Elapsed: " + _elapsedTime + " / Fastets " + ft);
        }
        if (GameController.Instance.levelController.levels.Count < LevelToLoad.levelNumber + 1)
        {
            _nextLevelButton.interactable = false;
            Debug.Log(GameController.Instance.levelController.levels.Count + " < " + (LevelToLoad.levelNumber + 1));
        }
        else
        {
            Debug.Log(GameController.Instance.levelController.levels.Count + " > " + (LevelToLoad.levelNumber + 1));
            MeshData newleveltoload = GameController.Instance.levelController.levels.First(x => x.levelNumber == LevelToLoad.levelNumber + 1);
            LevelToLoad = newleveltoload;
        }
    }

    public void LevelSelectClick()
    {
        GameController.Instance.sceneController.GoToScene(SceneType.SceneLevelSelect);
    }

    public void NextLevelClick()
    {  
        GameController.Instance.sceneController.GoToScene(SceneType.SceneGame);
    }
}
