using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Database",menuName = "Config/Users")]

public class Database : ScriptableObject
{
    public string sheetId;
    public string gridId;
    public List<User> _users;

    [ContextMenu("Sync")]
    private void Sync()
    {
        ReadGoogleSheets.FillData<User>(sheetId, gridId, list =>
          {
              _users = list;
              ReadGoogleSheets.SetDirty(this);
          });
    }
}


[Serializable]
public class User
{
    public string Name;
    public string Number;
    public string Location;
}
