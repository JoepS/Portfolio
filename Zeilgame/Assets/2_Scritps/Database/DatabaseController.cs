﻿using SQLite4Unity3d;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Text;
using System.Security.Cryptography;

public class DatabaseController {

    string EncryptionKey = "abcdefghijklmnop";
    string EncryptionIv = "ponmlkjihgfedcba";

    public SQLiteConnection connection
    {
        get
        {
            return _connection;
        }
    }

    private SQLiteConnection _connectionVersionCheck;

    private SQLiteConnection _connection;

    GameObject go;

    string _filePath;

    public DatabaseController(string DatabaseName)
    {

        go = GameObject.Find("StartupText");
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
        string connectionPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName.Replace(".db", ".db"));
        _connectionVersionCheck = new SQLiteConnection(connectionPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            //Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);
#endif

            //Debug.Log("Database written");
        }
        else{
#if UNITY_ANDROID
            var tempFilepath = "";
            //try {
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                tempFilepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName.Replace(".", "2."));
                File.WriteAllBytes(tempFilepath, loadDb.bytes);
                _connectionVersionCheck = new SQLiteConnection(tempFilepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                go.GetComponent<Text>().text += "Created new Connection check database!\n";
                
            //}
            //catch (Exception e)
            //{
            //    go.GetComponent<Text>().text += e.Message + "\n Path DB.db: " + filepath + "\n Path DB2.db: " + tempFilepath;
            //}
            _filePath = filepath;
            try
            {
                DecryptDB(filepath);
            }
            catch(Exception e)
            {
                Debug.LogWarning("Decription error: " + e.Message + "\n" + filepath + "\n");
            }
#endif
        }
        var dbPath = filepath;
#endif


        //_connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
#if UNITY_EDITOR
        Debug.Log("Final PATH: " + dbPath);
#endif
        CheckIfDBUpToDate();
    }

    public void CheckIfDBUpToDate() {
        if (_connectionVersionCheck == null)
        {
            go.GetComponent<Text>().text += "New database so it's up to date";
            return;
        }
        int versionId = -1;
        try
        {
            versionId = _connection.Table<Version>().First().versionId;
            #if UNITY_EDITOR
            Debug.Log("Current Version ID: " + versionId);
            #endif
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
        int newDbId = -1;
        try
        {
            newDbId = _connectionVersionCheck.Table<Version>().First().versionId;
            #if UNITY_EDITOR
            Debug.Log("New Version ID: " + newDbId);
            #endif
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message + " \n " + _connectionVersionCheck + "\n It throws an error here?");
        }
        go.GetComponent<Text>().text += "\n Current Version " + versionId + " / New Version " + newDbId;
        //als versie verouderd is
        if (newDbId > versionId)
        {
            UpdateDB();
        }
        else
        {
#if !UNITY_EDITOR
            File.Delete(_connectionVersionCheck.DatabasePath);
#endif
        }
    }

    public void DecryptDB()
    {
        DecryptDB(_filePath);
    }

    public bool DecryptDB(string dbPath)
    {
        return true;
        byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
        byte[] iv = Encoding.UTF8.GetBytes(EncryptionIv);

        var encMessage = File.ReadAllBytes(dbPath);

        byte[] decMessage;
        using (var rijndael = new RijndaelManaged())
        {
            rijndael.Key = key;
            rijndael.IV = iv;
            decMessage = DecryptBytes(rijndael, encMessage);
        }

        File.WriteAllBytes(dbPath, decMessage);
        return true;
    }

    public bool EncryptDB()
    {
        return true;
        _connection.Close();
        string dbPath = _connection.DatabasePath;
        _connection = null;
        var file = File.ReadAllBytes(dbPath);
        //var encryptedFile = EncryptorDecryptor.EncryptDecrypt(file);
        //File.Delete(dbPath);
        byte[] encMessage;
        byte[] key;
        byte[] iv;

        key = Encoding.UTF8.GetBytes(EncryptionKey);
        iv = Encoding.UTF8.GetBytes(EncryptionIv);
        using (var rijndael = new RijndaelManaged())
        {
            rijndael.Key = key;
            rijndael.IV = iv;
            encMessage = EncryptBytes(rijndael, file);
        }
        File.WriteAllBytes(dbPath, encMessage);
        return true;
    }

    private static byte[] EncryptBytes(
        SymmetricAlgorithm alg,
        byte[] message)
    {
        if ((message == null) || (message.Length == 0))
        {
            return message;
        }

        if (alg == null)
        {
            throw new ArgumentNullException("alg");
        }

        using (var stream = new MemoryStream())
        using (var encryptor = alg.CreateEncryptor())
        using (var encrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
        {
            encrypt.Write(message, 0, message.Length);
            encrypt.FlushFinalBlock();
            return stream.ToArray();
        }
    }

    private static byte[] DecryptBytes(
        SymmetricAlgorithm alg,
        byte[] message)
    {
        if ((message == null) || (message.Length == 0))
        {
            return message;
        }

        if (alg == null)
        {
            throw new ArgumentNullException("alg");
        }

        using (var stream = new MemoryStream())
        using (var decryptor = alg.CreateDecryptor())
        using (var encrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
        {
            encrypt.Write(message, 0, message.Length);
            encrypt.FlushFinalBlock();
            return stream.ToArray();
        }
    }
    /// <summary>
    /// Updates the table in the database to match the latest database
    /// Adds and removes colums
    /// Adds data
    /// Removes data if parameter equals true
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="removeOld"></param>
    public void UpdateTable<T>(bool removeOld) where T : Model, new()
    {
        TableMapping cvcTableMapping = _connectionVersionCheck.GetMapping<T>();
        List<SQLiteConnection.ColumnInfo> cColumnInfo = _connection.GetTableInfo(typeof(T).ToString());

        //Columns that need to be added
        List<TableMapping.Column> addColumns = cvcTableMapping.Columns.Where(x => cColumnInfo.Where(y => y.Name.Equals(x.Name)).Count() == 0).ToList();

        //Add each column
        foreach(TableMapping.Column c in addColumns)
        {
            //Debug.Log(typeof(T) + " / " + c.Name + " / " + c.ColumnType.Name);
            string query = string.Format("ALTER TABLE {0} ADD {1} {2}", typeof(T).ToString(), c.Name, GetDatabaseType(c.ColumnType.Name));
            //Debug.Log(query);
            _connection.Execute(query);
        }

        //Get updated table mapping
        cvcTableMapping = _connectionVersionCheck.GetMapping<T>();
        //Get updated column info
        cColumnInfo = _connection.GetTableInfo(typeof(T).ToString());
        //Columns that need to be removed
        List<SQLiteConnection.ColumnInfo> removeColumns = cColumnInfo.Where(x => cvcTableMapping.Columns.Where(y => y.Name.Equals(x.Name)).Count() == 0).ToList();

        //If there are columns that need to be removed
        if (removeColumns.Count > 0)
        {
            //Create backup of table
            string query1 = "CREATE TEMPORARY TABLE table_backup (";
            query1 += cvcTableMapping.FindColumnWithPropertyName("id").Name + " PRIMARY KEY NOT NULL, ";
            foreach (TableMapping.Column c in cvcTableMapping.Columns)
            {
                if(c.Name != "id")
                {
                    query1 += c.Name + ",";
                }
            }
            query1 = query1.Remove(query1.Length - 1);
            query1 += ")";
            //insert backup data into table
            string query2 = "INSERT INTO table_backup SELECT id, ";
            foreach(TableMapping.Column c in cvcTableMapping.Columns)
            {
                if(c.Name != "id")
                    query2 +=  c.Name + ",";
            }
            query2 = query2.Remove(query2.Length - 1);
            query2 += " from " + typeof(T).ToString();
            //Drop table with columns that need to be removed
            string query3 = "DROP TABLE " + typeof(T).ToString();
            //Create new table acording to new database structure
            string query4 = "CREATE TABLE \"" + typeof(T).ToString() + "\"('id' INTEGER PRIMARY KEY NOT NULL, ";
            foreach(TableMapping.Column c in cvcTableMapping.Columns)
            {
                if(c.Name != "id")
                    query4 += "'" + c.Name + "'" + GetDatabaseType(c.ColumnType.Name.ToString()) + ",";
            }
            query4 = query4.Remove(query4.Length - 1);
            query4 += ")";
            //Insert backup data in new table
            string query5 = "INSERT INTO " + typeof(T).ToString() + " SELECT * FROM table_backup";
            //Remove backup table
            string query6 = "DROP TABLE table_backup";
            
            _connection.Execute(query1);
            _connection.Execute(query2);
            _connection.Execute(query3);
            _connection.Execute(query4);
            _connection.Execute(query5);
            _connection.Execute(query6);
        }

        //List of data to be cecked
        List<T> checkList = _connectionVersionCheck.Table<T>().ToList();
        //Foreach row in the new database table
        foreach(T t in checkList)
        {
            List<T> test = _connection.Table<T>().ToList();
            int id = t.id;
            //Get corresponding data entry
            List<T> temp = test.Where(x => (x.id) == (id)).ToList();
            //If the corresponding data entry is found
            if (temp.Count() > 0)
            {
                //Copy data to the excisting entry
                T old = temp.First();
                old.Copy(t);
                _connection.Update(old);
            }
            else
            {
                //No corresponding data entry is found
                //Insert new data entry
                _connection.Insert(t);
            }
        }

        //If old values need to be removed
        if (removeOld)
        {
            //Get all the data entries
            List<T> deleteList = _connection.Table<T>().ToList();
            //Select data entries not found in the new database
            deleteList = deleteList.Where(x => _connectionVersionCheck.Table<T>().Where(y => y.id == x.id).Count() == 0 && x.id != 0).ToList();
            //Foreach data entry that needs to be deleted
            foreach (T t in deleteList)
            {
                //Remove from database
                _connection.Delete(t);
            }
        }

    }

    public void UpdateDB()
    {
        /* Update
         *
         * Boats
         * Items
         * Locations
         * Opponent setups
         * Persons
         * Races
         * Routes
         * Sails
         * Track
         * Upgrade
         * Achievements
         * AchievementProperty
         * Version
        */
        //Boats
        UpdateTable<Achievement>(true);
        UpdateTable<Property>(true);
        UpdateTable<Boat>(true);
        UpdateTable<BoatRanking>(false);
        UpdateTable<Item>(true);
        UpdateTable<Location>(true);
        UpdateTable<OpponentSetup>(true);
        UpdateTable<Person>(true);
        UpdateTable<Player>(false);
        UpdateTable<Race>(true);
        UpdateTable<Route>(true);
        UpdateTable<Sail>(true);
        UpdateTable<Track>(true);
        UpdateTable<Upgrade>(true);
        UpdateTable<Version>(true);
        UpdateTable<Request>(true);
        UpdateTable<RequestProperty>(true);
    }

    public string GetDatabaseType(string type)
    {
        switch (type)
        {
            case "String":
                return DBTypes.TEXT.ToString();
            case "Int32":
                return DBTypes.INTEGER.ToString();
            case "Boolean":
                return DBTypes.INTEGER.ToString();
            case "Single":
                return DBTypes.REAL.ToString();
            default:
                Debug.LogWarning("Didnt recognize this type: " + type);
                return DBTypes.TEXT.ToString();
        }
    }

    public override string ToString()
    {
        return "DatabaseController: " + _connection;
    }

    enum DBTypes
    {
        TEXT,
        INTEGER,
        BLOB,
        REAL,
        NUMERIC
    }
}

public static class EncryptorDecryptor
{
    public static int key = 129;

    public static string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ key);
            outSb.Append(c);
        }
        return outSb.ToString();
    }
}
