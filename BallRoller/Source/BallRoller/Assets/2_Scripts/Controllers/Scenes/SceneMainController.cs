using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneMainController : MonoBehaviour {

    [SerializeField]
    GameObject _menuPanel;
    [SerializeField]
    DownloadLevelsPanel _downloadLevelsPanel;

    SceneController _sceneController;
    NetworkController _networkController;
    LevelController _levelController;

    bool _newlevelsAvaliable = false;
    string _newlevelsHash;

    void Start()
    {
        _sceneController = GameController.Instance.sceneController;
        _networkController = GameController.Instance.networkController;
        _levelController = GameController.Instance.levelController;

        _networkController.CheckForNewLevels(AskToDownloadNewLevels);
    }

    public void AskToDownloadNewLevels(string hash)
    {
        _newlevelsAvaliable = true;
        _newlevelsHash = hash;
    }

    public void StartGameButtonClick()
    {
        if (_newlevelsAvaliable)
        {
            _menuPanel.SetActive(false);
            _downloadLevelsPanel.SetActive(true);
        }
        else
        {
            _sceneController.GoToScene(SceneType.SceneLevelSelect);
        }
    }

    public void YesButtonClick()
    {
        _networkController.DownloadNewLevels(LevelDownloadDone);
        _downloadLevelsPanel.StartDownload();
    }

    public void LevelDownloadDone(GetLevelsResponse glr)
    {
        if (glr.succes)
        {
            Debug.Log("Succesfuly downloaded new levels");
            _downloadLevelsPanel.DoneDownloading();

            Levelhash lh = Levelhash.All().First();
            lh.hash = _newlevelsHash;
            lh.Save();

            SaveLevels(glr.data);
            _levelController.ReloadLevels();
        }
        else
        {
            Debug.Log("Failed to download new levels");
        }
    }

    public void SaveLevels(List<StringMeshData> newLevels)
    {
        GameController.db.connection.DeleteAll<MeshData>();
        foreach(StringMeshData smd in newLevels)
        {
            MeshData md = smd.ToMeshData();
            md.New();
        }
    }

    public void NoButtonClick()
    {
        _sceneController.GoToScene(SceneType.SceneLevelSelect);
    }
    
    public void OptionsButtonClick()
    {
        _sceneController.GoToScene(SceneType.SceneOptions);
    }

    public void ExitGameButtonClick()
    {
        Application.Quit();
    }
}
