using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class MeshData : Model
{


    public string _name;
    [PrimaryKey]
    public string name { get { return _name; } set { _name = value; } }

    public int _levelNumber;
    public int levelNumber { get { return _levelNumber; } set { _levelNumber = value; } }


    #region MeshData
    public int[] _triangles;
    public string triangles {
        get {
            string returnstring = "{";
            foreach(int i in _triangles)
            {
                returnstring += i + ";";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);
            returnstring += "}";
            return returnstring;    
        }
        set
        {
            string val = value;
            val = val.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(';');
            _triangles = new int[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                _triangles[i] = int.Parse(vals[i]);
            }
        }
    }
    public Vector3[] _vertices;
    public string vertices
    {
        get
        {
            string returnstring = "{";
            foreach(Vector3 v in _vertices)
            {
                returnstring += "{" + v.x + "," + v.y + "," + v.z + "};";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);
            returnstring += "}";
            return returnstring;
        }
        set
        {
            string val = value;
            val = val.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(';');
            _vertices = new Vector3[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                string vec = vals[i];
                vec = vec.Substring(1);
                vec = vec.Substring(0, vec.Length - 1);
                string[] vecvals = vec.Split(',');
                Vector3 temp = Vector3.zero;
                temp.x = float.Parse(vecvals[0]);
                temp.y = float.Parse(vecvals[1]);
                temp.z = float.Parse(vecvals[2]);
                _vertices[i] = temp;
            }
        }
    }
    public Vector2[] _uv1;
    public string uv1
    {
        get
        {
            if (_uv1.Length == 0)
            {
                return "{}";
            }
            string returnstring = "{";
            foreach (Vector2 v in _uv1)
            {
                returnstring += "{" + v.x + "," + v.y + "};";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);
            returnstring += "}";
            return returnstring;
        }
        set
        {
            string val = value;
            val = val.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(';');
            _uv1 = new Vector2[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                string vec = vals[i];
                if (vec.Equals(""))
                    continue;
                vec = vec.Substring(1);
                vec = vec.Substring(0, vec.Length - 1);
                string[] vecvals = vec.Split(',');
                Vector2 temp = Vector2.zero;
                temp.x = float.Parse(vecvals[0]);
                temp.y = float.Parse(vecvals[1]);
                _uv1[i] = temp;
            }
        }
    }
    public Vector2[] _uv2;
    public string uv2
    {
        get
        {
            if(_uv2.Length == 0)
            {
                return "{}";
            }
            string returnstring = "{";
            foreach (Vector2 v in _uv2)
            {
                returnstring += "{" + v.x + "," + v.y + "};";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);
            returnstring += "}";
            return returnstring;
        }
        set
        {
            string val = value;
            val = val.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(';');
            _uv2 = new Vector2[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                string vec = vals[i];
                if (vec.Equals(""))
                    continue;
                vec = vec.Substring(1);
                vec = vec.Substring(0, vec.Length - 1);
                string[] vecvals = vec.Split(',');
                Vector2 temp = Vector2.zero;
                temp.x = float.Parse(vecvals[0]);
                temp.y = float.Parse(vecvals[1]);
                _uv2[i] = temp;
            }
        }
    }
    public Vector3[] _normals;
    public string normals
    {
        get
        {
            string returnstring = "{";
            foreach (Vector3 v in _normals)
            {
                returnstring += "{" + v.x + "," + v.y + "," + v.z + "};";
            }
            returnstring = returnstring.Substring(0, returnstring.Length - 1);
            returnstring += "}";
            return returnstring;
        }
        set
        {
            string val = value;
            val = val.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(';');
            _normals = new Vector3[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                string vec = vals[i];
                vec = vec.Substring(1);
                vec = vec.Substring(0, vec.Length - 1);
                string[] vecvals = vec.Split(',');
                Vector3 temp = Vector3.zero;
                temp.x = float.Parse(vecvals[0]);
                temp.y = float.Parse(vecvals[1]);
                temp.z = float.Parse(vecvals[2]);
                _normals[i] = temp;
            }
        }
    }
    #endregion

    public Vector3 _goalLocation;
    public string goalLocation
    {
        get
        {
            return "{" + _goalLocation.x + "," + _goalLocation.y + "," + _goalLocation.z + "}";
        }
        set
        {
            string val = value.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(',');
            _goalLocation = Vector3.zero;
            _goalLocation.x = float.Parse(vals[0]);
            _goalLocation.y = float.Parse(vals[1]);
            _goalLocation.z = float.Parse(vals[2]);
        }
    }

    public Vector3 _spawnLocation;
    public string spawnLocation
    {
        get
        {
            return "{" + _spawnLocation.x + "," + _spawnLocation.y + "," + _spawnLocation.z + "}";
        }
        set
        {
            string val = value.Substring(1);
            val = val.Substring(0, val.Length - 1);
            string[] vals = val.Split(',');
            _spawnLocation = Vector3.zero;
            _spawnLocation.x = float.Parse(vals[0]);
            _spawnLocation.y = float.Parse(vals[1]);
            _spawnLocation.z = float.Parse(vals[2]);
        }
    }

    public static MeshData FromJson(string json)
    {
        return JsonUtility.FromJson<MeshData>(json);
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static List<MeshData> All()
    {
        return GameController.db.connection.Table<MeshData>().ToList();
    }
}

[Serializable]
public class StringMeshData
{
    public string name;
    public int levelNumber;
    public string goalLocation;
    public string spawnLocation;
    public string triangles;
    public string vertices;
    public string uv1;
    public string uv2;
    public string normals;

    public void Init(MeshData data)
    {
        this.name = data.name;
        this.levelNumber = data.levelNumber;
        this.goalLocation = data.goalLocation;
        this.spawnLocation = data.spawnLocation;
        this.triangles = data.triangles;
        this.vertices = data.vertices;
        this.uv1 = data.uv1;
        this.uv2 = data.uv2;
        this.normals = data.normals;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public MeshData ToMeshData()
    {
        MeshData md = new MeshData();
        md.name = name;
        md.levelNumber = levelNumber;
        md.goalLocation = goalLocation;
        md.spawnLocation = spawnLocation;
        md.triangles = triangles;
        md.vertices = vertices;
        md.uv1 = uv1;
        md.uv2 = uv2;
        md.normals = normals;
        return md;
    }
}
