using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SceneOptionsController : MonoBehaviour {
    [SerializeField] Dropdown _qualityDropdown;
    [SerializeField]
    GameObject _areYouSurePanel;
    [SerializeField]
    GameObject _highScoresDeletedPanel;

    SceneController _sceneController;
    

	// Use this for initialization
	void Start () {
        _sceneController = GameController.Instance.sceneController;
        InitializeDropDown();
	}

    void InitializeDropDown()
    {
        List<string> names = QualitySettings.names.ToList();
        _qualityDropdown.AddOptions(names);
        int id = QualitySettings.GetQualityLevel();
        _qualityDropdown.value = id;
        _qualityDropdown.onValueChanged.AddListener(OnDropDownValueChanged);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDropDownValueChanged(int value)
    {
        Debug.Log("Set Quality Level: " + value);

        QualitySettings.SetQualityLevel(value, true);
    }

    public void OnBackButtonClicked()
    {
        _sceneController.GoToScene(SceneType.SceneMain);
    }

    public void OnResetHighscoresClick()
    {
        _areYouSurePanel.SetActive(true);
    }

    public void YesButtonClick()
    {
        GameController.db.connection.DeleteAll<Highscore>();
        _areYouSurePanel.SetActive(false);
        _highScoresDeletedPanel.SetActive(true);
    }

    public void NoButtonClick()
    {
        _areYouSurePanel.SetActive(false);
    }

    public void OkButtonClick()
    {
        _highScoresDeletedPanel.SetActive(false);
    }
}
