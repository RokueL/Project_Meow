using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{


    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

   public GameObject Instatiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if(prefab == null)
        {
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public void Destroy(GameObject prefab)
    {
        if(prefab == null)
        {
            return;
        }

        Object.Destroy(prefab);
    }
}
