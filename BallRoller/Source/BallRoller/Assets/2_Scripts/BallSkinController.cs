using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSkinController : MonoBehaviour {
    [SerializeField]
    List<Skin> _skins;
    public List<Skin> skins
    {
        get
        {
            return _skins;
        }
    }


    Skin _selected;
    public Skin selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;
        }
    }
	// Use this for initialization
	void Start () {
		if(selected == null)
        {
            int id = PlayerPrefs.GetInt("SelectedSkin");
            selected = _skins[id];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
