using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLevelSelectController : MonoBehaviour {

    LevelController _levelController;
    SceneController _sceneController;

    [Header("Prefab")]
    [SerializeField] GameObject _levelSelector;
    [Header("Level Selector Container")]
    [SerializeField] GameObject _container;

	// Use this for initialization
	void Start () {
        _levelController = GameController.Instance.levelController;
        _sceneController = GameController.Instance.sceneController;

        createLevelSelectors();
	}

    void createLevelSelectors()
    {
        foreach(MeshData level in _levelController.levels){
            GameObject selector = Instantiate(_levelSelector, _container.transform);
            selector.GetComponent<LevelSelector>().initialize(level);
        }
    }

    public void BackButtonClick()
    {
        _sceneController.GoToScene(SceneType.SceneMain);
    }

    public void SkinsButtonClick()
    {
        _sceneController.GoToScene(SceneType.SceneSkins);
    }
}
