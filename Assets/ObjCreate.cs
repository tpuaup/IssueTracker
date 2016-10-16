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
		Vector3[] vertices = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (10, 0, 0),
			new Vector3 (10, 10, 0),
			new Vector3 (0, 10, 0),
			new Vector3 (0, 0, 10),
			new Vector3 (10, 0, 10),
			new Vector3 (10, 10, 10),
			new Vector3 (0, 10, 10),
		};

		int[] triangles = new int[] { 
			0,3,1,1,3,2,
			4,0,1,1,5,4,
			1,2,5,5,2,6,
			4,7,0,0,7,3,
			4,5,7,7,5,6,
			3,7,2,2,7,6
		};

		Vector2[] uvs = new Vector2[] 
		{
			new Vector2(0,0),
			new Vector2(1,0),
			new Vector2(1,1),
			new Vector2(0,1),
			new Vector2(1,0),
			new Vector2(0,0),
			new Vector2(0,1),
			new Vector2(1,1)
		};
//		CreateMesh (vertices, triangles, uvs);
		ReadObjFile(Application.dataPath+@"/cow-nonormals.obj");



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
		int vCount = 0;
		int fCount = 0;
		Mesh mesh = new Mesh ();
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Mesh> meshList = new List<Mesh> ();
		while ((aLine = reader.ReadLine ()) != null) 
		{
			if (aLine.Contains ("v ")) {
				if (vCount == 0 && fCount == 0) {
					// new Mesh and clear vertices
					mesh = new Mesh ();
					vertices.Clear ();

				} else if (vCount == 0 && fCount != 0) {
					mesh.triangles = triangles.ToArray ();
					fCount = 0;
					meshList.Add (mesh);
					mesh = new Mesh ();
				}
				aLine = aLine.Substring (2);
				float x = float.Parse (aLine.Substring (0, aLine.IndexOf (" ")));
				aLine = aLine.Substring (aLine.IndexOf (" ")).TrimStart ();
				float y = float.Parse (aLine.Substring (0, aLine.IndexOf (" ")));
				aLine = aLine.Substring (aLine.IndexOf (" ")).TrimStart ();
				float z = float.Parse (aLine.Substring (0));
				vertices.Add (new Vector3 (x, y, z));
				vCount++;

			} else if (aLine.Contains ("f ")) {
				if (vCount != 0 && fCount == 0) {
					triangles.Clear ();
					mesh.SetVertices (vertices);
					vCount = 0;
				}
				aLine = aLine.Substring (2);
				int p0 = int.Parse (aLine.Substring (0, aLine.IndexOf (" ")));
				aLine = aLine.Substring (aLine.IndexOf (" ")).TrimStart ();
				int p1 = int.Parse (aLine.Substring (0, aLine.IndexOf (" ")));
				aLine = aLine.Substring (aLine.IndexOf (" ")).TrimStart ();
				int p2 = int.Parse (aLine.Substring (0));

				triangles.Add (p0-1);
				triangles.Add (p1-1);
				triangles.Add (p2-1);
				fCount++;
			} 
			else if (aLine == "" && fCount!=0) 
			{
				mesh.triangles = triangles.ToArray ();
				fCount = 0;
				meshList.Add (mesh);
			}
		}
		mesh.triangles = triangles.ToArray ();
		fCount = 0;
		meshList.Add (mesh);


		// 產生mesh
		foreach (Mesh m in meshList) {

			GameObject obj = new GameObject ("mesh");
			MeshFilter mf = obj.AddComponent<MeshFilter> ();
			mf.mesh = m;
			MeshRenderer renderer = obj.AddComponent<MeshRenderer> ();
			//renderer.material.shader = Shader.Find ("Particles/Additive");
			//			renderer.material.shader = Shader.Find("Standard");
			//			renderer.material.color = Color.white;
			//			renderer.material.name = "defaultMat";
			renderer.material = material;
			mesh.Optimize ();
			mesh.RecalculateNormals ();

		}
	}
}
