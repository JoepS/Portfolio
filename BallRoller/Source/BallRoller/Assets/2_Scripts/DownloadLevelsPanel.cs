using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownloadLevelsPanel : MonoBehaviour {

    [SerializeField]
    GameObject _questionPanel;
    [SerializeField]
    GameObject _downloadingPanel;
    [SerializeField]
    RotatingImage _loadingImage;

    [SerializeField]
    float _rotatingImageSpeed;

    [SerializeField] GameObject _continueButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetActive(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public void StartDownload()
    {
        _questionPanel.SetActive(false);
        _downloadingPanel.SetActive(true);
        _loadingImage.rotating = true;
        _loadingImage.speed = -_rotatingImageSpeed;
    }

    public void DoneDownloading()
    {
        _loadingImage.gameObject.SetActive(false);
        _continueButton.SetActive(true);
    }
}
