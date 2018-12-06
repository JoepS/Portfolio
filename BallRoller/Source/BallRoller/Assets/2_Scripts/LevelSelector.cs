using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelector : MonoBehaviour {

    [SerializeField] Text _levelName;
    [SerializeField] Text _highScoreText;
    MeshData _level;
    public MeshData level
    {
        set
        {
            _level = value;
        }
    }

	// Use this for initialization
	void Start () {
		
	}

    public void initialize(MeshData level)
    {
        _level = level;
        _levelName.text = level.name;

        float elapsedTime = 0;
        List <Highscore> all = Highscore.All();
        if(all != null && all.Count > 0)
        {
            List<Highscore> hsw = all.Where(x => x.levelnumber == level.levelNumber).ToList();
            if (hsw != null && hsw.Count > 0)
            {
                Highscore hs = hsw.First();
                if (hs != null)
                    elapsedTime = hs.time;
            }                
        }
        int miliseconds = Mathf.RoundToInt((elapsedTime - Mathf.RoundToInt(elapsedTime)) * 1000);
        TimeSpan span = new TimeSpan(0, 0, 0, Mathf.RoundToInt(elapsedTime), miliseconds);
        _highScoreText.text = "Fastest: " + span.Minutes.ToString("00") + ":" + span.Seconds.ToString("00") + ":" + span.Milliseconds.ToString("000");
    }

    public void LoadLevel()
    {
        SceneGameController.LevelToLoad = _level;
        GameController.Instance.sceneController.GoToScene(SceneType.SceneGame);
    }

}
