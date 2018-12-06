using SQLite4Unity3d;
using System.Collections.Generic;

public class Model {

	public void Save()
    {
        GameController.db.connection.Update(this);
    }

    public void New()
    {
        GameController.db.connection.Insert(this);
    }
}
