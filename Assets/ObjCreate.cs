using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;

public class ObjCreate : MonoBehaviour {

	public Material material;
	// Use this for initialization
	void Start () 
	{
//		Vector3[] vertices = new Vector3[] {
//			new Vector3 (0, 0, 0),
//			new Vector3 (10, 0, 0),
//			new Vector3 (10, 10, 0),
//			new Vector3 (0, 10, 0),
//			new Vector3 (0, 0, 10),
//			new Vector3 (10, 0, 10),
//			new Vector3 (10, 10, 10),
//			new Vector3 (0, 10, 10),
//		};
//
//		int[] triangles = new int[] { 
//			0,3,1,1,3,2,
//			4,0,1,1,5,4,
//			1,2,5,5,2,6,
//			4,7,0,0,7,3,
//			4,5,7,7,5,6,
//			3,7,2,2,7,6
//		};
//
//		Vector2[] uvs = new Vector2[] 
//		{
//			new Vector2(0,0),
//			new Vector2(1,0),
//			new Vector2(1,1),
//			new Vector2(0,1),
//			new Vector2(1,0),
//			new Vector2(0,0),
//			new Vector2(0,1),
//			new Vector2(1,1)
//		};
//		CreateMesh (vertices, triangles, uvs);
		ReadObjFile(Application.dataPath+@"/Toilet.obj");



	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void CreateMesh(Vector3[] vertices, int[] triangles, Vector2[] uvs)
	{
		GameObject obj = new GameObject ();
		MeshFilter mf = obj.AddComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		mf.mesh = mesh;
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		MeshRenderer renderer = obj.AddComponent<MeshRenderer> ();
		renderer.material = material;
		mesh.RecalculateNormals ();
		mesh.Optimize ();


	}

	private Vector3 GetNormal(Vector3 p1,Vector3 p2,Vector3 p3)
	{
		Vector3 x = (p1 - p2);
		Vector3 y = (p3 - p1);
		Vector3 z = Vector3.Cross (x, y);
		return z;

	}

	private void ReadObjFile(string filePath)
	{
		StreamReader reader = new StreamReader (filePath);
		string aLine = string.Empty;

		Mesh mesh = new Mesh ();
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Mesh> meshList = new List<Mesh> ();
		while ((aLine = reader.ReadLine ()) != null) 
		{
			if (aLine.Contains ("#") || aLine == "")
				continue;
			string[] cpms = aLine.Trim ().Split (' ');

			// vertices
			if (cpms [0] == "v") {
				vertices.Add (new Vector3 (float.Parse (cpms [1]), float.Parse (cpms [2]), float.Parse (cpms [3])));
			}

			// faces
			else if (cpms [0] == "f") 
			{
				if (cpms [1].Contains ("/"))
					continue;
				else 
				{
					for (int i = 1; i <= 3; i++)
						triangles.Add (int.Parse (cpms [i]) - 1);
				}

			}

			// texture map
			else if (cpms [0] == "vt") 
			{

			}

			// normal map
			else if (cpms [0] == "vn") 
			{

			}

			// group or object
			else if (cpms [0] == "g" || cpms[0]=="o") 
			{
				if (vertices.Count == 0)
					mesh.name = cpms [1];
				
				else 
				{
					// 將資料加入mesh
					mesh.SetVertices (vertices);
					mesh.triangles = triangles.ToArray ();
					meshList.Add (mesh);

					//reset
					vertices.Clear();
					triangles.Clear ();
					mesh = new Mesh ();
					mesh.name = cpms [1];
				}
			}
		}

		// 將資料加入mesh
		mesh.SetVertices (vertices);
		mesh.triangles = triangles.ToArray ();
		meshList.Add (mesh);


		// 產生game obj
		foreach (Mesh m in meshList) {

			GameObject obj = new GameObject ("mesh");
			MeshFilter mf = obj.AddComponent<MeshFilter> ();
			mf.mesh = m;
			MeshRenderer renderer = obj.AddComponent<MeshRenderer> ();

			renderer.material = material;
			mesh.Optimize ();
			mesh.RecalculateNormals ();

		}
	}
}
