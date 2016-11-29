using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
//using Tri = TriangleNet;
//using TSG3D = Tekla.Structures.Geometry3d;

public class ObjCreate : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

        ObjFile file = new ObjFile();
        file.ReadObjFile (Application.dataPath+@"/NPP3.obj");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
