using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DontDestroyOnLoadManager
{
    static List<GameObject> ddolObjects = new List<GameObject>();

    public static void DontDestroyOnLoad(this GameObject go) 
    {
        UnityEngine.Object.DontDestroyOnLoad(go);
        ddolObjects.Add(go);
    }

    public static void DestroyAll() 
    {
        foreach(var go in ddolObjects)
        {
            if(go != null)
            {
                UnityEngine.Object.Destroy(go);
            }
                
        }
        
        ddolObjects.Clear();
    }
}
