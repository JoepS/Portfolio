using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class Levelhash : Model {
    [PrimaryKey]
    public int id { get; set; }
    public string hash { get; set; }

    public static List<Levelhash> All()
    {
        return GameController.db.connection.Table<Levelhash>().ToList();
    }
}
