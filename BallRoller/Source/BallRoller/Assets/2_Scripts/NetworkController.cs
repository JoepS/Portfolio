using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NetworkController : MonoBehaviour
{

//#if UNITY_EDITOR
//    const string url = "joepsijtsma.local/api/";
//#else
    const string url = "http://www.joepsijtsma.com/api/";
//#endif
    const string levelsurl = "ballrollerlevels";
    const string savelevelurl = "savelevel";
    const string checknewlevelurl = "checklevels";

    const string playerprefshashstring = "levelshash";
    string hash = "none";
    Levelhash _levelhash;

    public Action<string> AskToDownload;

    void Awake()
    {
        Debug.Log(Levelhash.All().Count);
        if (Levelhash.All().Count == 0)
            return;
        _levelhash = Levelhash.All().First();
        if (_levelhash == null)
            return;
        hash = _levelhash.hash;
    }


	// Use this for initialization
	void Start () {
	}

    public void CheckForNewLevels(Action<string> action)
    {
        StartCoroutine(CheckNewLevels(action));
    }

    public void DownloadNewLevels(Action<GetLevelsResponse> succes)
    {
        StartCoroutine(GetLevels(succes));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(SaveLevels());
        }
	}

    public IEnumerator GetLevels(Action<GetLevelsResponse> succes)
    {
        WWW www = new WWW(url + levelsurl);
        yield return www;
        GetLevelsResponse glr = GetLevelsResponse.FromJson(www.text);
        Debug.Log(www.text);

        succes.Invoke(glr);

    }

    IEnumerator SaveLevels()
    {
        List<MeshData> data = MeshData.All();

        foreach(MeshData md in data)
        {
            StringMeshData d = new StringMeshData();
            d.Init(md);
            WWWForm form = new WWWForm();
            form.AddField("data", d.ToJson());
            Debug.Log(d.ToJson());
            WWW www = new WWW(url + savelevelurl, form);
            yield return www;
            Debug.Log(www.text);
        }
    }

    IEnumerator CheckNewLevels(Action<string> newLevels)
    {
        WWWForm form = new WWWForm();
        form.AddField("hash", hash);
        Debug.Log("url: " + (url + checknewlevelurl));
        WWW www = new WWW(url + checknewlevelurl, form);
        yield return www;
        Debug.Log(www.text);
        CheckLevelsResponse clr = CheckLevelsResponse.FromJson(www.text);
        if (clr.succes)
        {
            hash = clr.hash;
            Debug.Log("Getting new levels");
            newLevels.Invoke(clr.hash);
        }
        else
        {
            Debug.Log("Got the newest levels");
        }
    }
}
[Serializable]
public class GetLevelsResponse{

    public bool succes;
    public List<StringMeshData> data;

    public static GetLevelsResponse FromJson(string json)
    {
        return JsonUtility.FromJson<GetLevelsResponse>(json);
    }
}

public class CheckLevelsResponse
{
    public bool succes;
    public string hash;

    public static CheckLevelsResponse FromJson(string json)
    {
        return JsonUtility.FromJson<CheckLevelsResponse>(json);
    }
}