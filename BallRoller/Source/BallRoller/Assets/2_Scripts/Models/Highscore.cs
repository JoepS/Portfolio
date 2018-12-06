using SQLite4Unity3d;
using System.Collections.Generic;
using System.Linq;

public class Highscore : Model  {
    [PrimaryKey]
    public int levelnumber { get; set; }
    public float time { get; set; }

    public static List<Highscore> All()
    {
        return GameController.db.connection.Table<Highscore>().ToList();
    }
}
