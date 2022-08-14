using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataBase : ScriptableObject
{
    public List<GunDataBase> GunDataBases;
    [MenuItem("Tools/Create Material")]
    public static void CreatAsset()
    {
        AssetDatabase.CreateAsset(new DataBase(),"Assets/data.asset");
    }
}

[Serializable]
public class GunDataBase 
{
    public Sprite sprite;
    public int Volume;
    public int Atk;
    public int speed;
}
