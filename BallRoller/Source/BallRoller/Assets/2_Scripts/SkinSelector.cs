
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinSelector : MonoBehaviour {
    [SerializeField]
    Text _nameText;
    [SerializeField]
    Text _selectedText;
    [SerializeField]
    Image _previewImage;

    int _selectorId;
    public int id
    {
        get
        {
            return _selectorId;
        }
    }

    bool _selected;
    public bool selected {
        get {
            return _selected;
        }
        set
        {
            _selected = value;
            if (_selected)
                _selectedText.text = "Selected";
            else
                _selectedText.text = "";
        }
    }

    UnityAction<int> _onClick;

    public void Initialize(string name, bool selected, Sprite image, int id, UnityAction<int> onClick)
    {
        _nameText.text = name;
        if (selected)
            _selectedText.text = "Selected";
        else
            _selectedText.text = "";

        _previewImage.sprite = image;
        _selectorId = id;
        _onClick = onClick;
        
    }

    public void OnClick()
    {
        if (!this.selected)
        {
            _onClick.Invoke(_selectorId);
        }
    }
}
