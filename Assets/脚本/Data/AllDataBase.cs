using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AllDataBase : ScriptableObject
{
    public List<GunDataBase> GunDataBases;
    public List<BulletDataBase> BulletDataBase;
    [MenuItem("Tools/Create Material")]
    public static void CreatAsset()
    {
        AssetDatabase.CreateAsset(new AllDataBase(),"Assets/data.asset");
    }
}