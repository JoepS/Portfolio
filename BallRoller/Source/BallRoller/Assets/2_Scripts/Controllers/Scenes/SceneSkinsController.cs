using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSkinsController : MonoBehaviour {
    [SerializeField]
    GameObject _skinSelector;
    [SerializeField]
    GameObject _container;

    [SerializeField]
    Material _ballMaterial;
    [SerializeField]
    Material _previewTexture;
    [SerializeField]
    GameObject _previewPanel;
    int _previewSelected;

    List<SkinSelector> _skinSelectors;

    SceneController _sceneController;
    BallSkinController _ballSkinController;

    string _MainTex = "_MainTex";

	// Use this for initialization
	void Start ()
    {
        _sceneController = GameController.Instance.sceneController;
        _ballSkinController = GameController.Instance.ballSkinController;
        _skinSelectors = new List<SkinSelector>();
        Initialize();
	}

    void Initialize()
    {
        for (int i = 0; i < _ballSkinController.skins.Count; i++)
        {
            Skin skin = _ballSkinController.skins[i];
            Sprite s = skin._previewImage;
            GameObject skinSelector = GameObject.Instantiate(_skinSelector, _container.transform);
            SkinSelector ss = skinSelector.GetComponent<SkinSelector>();
            _skinSelectors.Add(ss);
            bool selected = false;
            if (i == _ballSkinController.selected.id)
            {
                selected = true;
            }
            ss.Initialize(s.name, selected, s, i, OnSkinSelect);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSkinSelect(int id)
    {
        _previewSelected = id;
        Texture newTexture = _ballSkinController.skins[id]._texture;
        _previewTexture.SetTexture(_MainTex, newTexture);
        _previewPanel.SetActive(true);

    }

    public void BackButtonClick()
    {
        _sceneController.GoToScene(SceneType.SceneLevelSelect);
    }

    public void YesButtonClick()
    {
        _previewPanel.SetActive(false);
        _ballMaterial.mainTexture = _ballSkinController.skins[_previewSelected]._texture;
        _ballSkinController.selected = _ballSkinController.skins[_previewSelected];
        PlayerPrefs.SetInt("SelectedSkin", _ballSkinController.selected.id);
        Debug.Log(_ballMaterial.mainTexture.name);

        foreach (SkinSelector ss in _skinSelectors)
        {
            if (ss.id != _previewSelected)
                ss.selected = false;
            else
                ss.selected = true;
        }
    }

    public void NoButtonClick()
    {
        _previewPanel.SetActive(false);
    }
}
