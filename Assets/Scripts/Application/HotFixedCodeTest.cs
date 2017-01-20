using UnityEngine;
using System.Collections;
using XLua;

[Hotfix]
public class HotFixedCodeTest : MonoBehaviour {

    void Awake()
    {
        Debug.Log("code from c#");
    }

    private void Start()
    {
        Debug.Log("code from c# start");
    }
}
