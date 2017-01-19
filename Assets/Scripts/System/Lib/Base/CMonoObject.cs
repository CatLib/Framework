using UnityEngine;
using System.Collections;

/// <summary>
/// CatLib Mono Object
/// </summary>
public class CMonoObject : MonoBehaviour
{

    protected Transform tran;

    /// <summary>
    /// Transform
    /// </summary>
    public Transform Transform
    {
        get
        {
            if (!tran) { tran = transform; }
            return tran;
        }
    }


    protected GameObject obj;

    /// <summary>
    /// GameObject
    /// </summary>
    public GameObject GameObject
    {
        get
        {
            if (!obj) { obj = gameObject; }
            return obj;
        }
    }

}
