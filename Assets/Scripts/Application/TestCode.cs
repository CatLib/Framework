using CatLib;
using CatLib.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour {

	// Use this for initialization
	void Start () {

        var io = new IO();
        //(io.DataPath as Directory).Create("testdir/hello/test");

        //Directory dir = (new Directory(Env.DataPath + "/" + "testdir/hello/test"));
        //dir.Create();
        //dir.Rename("hahaha");

        //dir.Move(Env.DataPath + "/testdir/" + "ddd");
        Directory dir = (new Directory(Env.DataPath + "/testdir/" + "copysrc"));
        dir.Duplicate(Env.DataPath + "/testdir/wantcopy");


        Debug.Log(dir.Exists());
	}
	

}
