using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
//using Tri = TriangleNet;
//using TSG3D = Tekla.Structures.Geometry3d;

public class ObjFile {

	// constructor
	public ObjFile()
	{

	}

    private List<Vector3> Vertices;

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
		renderer.material = new Material (Shader.Find ("Diffuse"));
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

	public GameObject ReadObjFile(string filePath)
	{
		StreamReader reader = new StreamReader (filePath);
		string aLine = string.Empty;

		Mesh mesh = new Mesh ();
		List<Vector3> vertices = new List<Vector3> ();
		List<int> triangles = new List<int> ();
		List<Mesh> meshList = new List<Mesh> ();
        List<Vector3> normals = new List<Vector3>();
        int faceCount = 0;

		while ((aLine = reader.ReadLine ()) != null && aLine!="") 
		{
			if (aLine.Contains ("#") || aLine == "")
				continue;
            string[] cpms = aLine.Trim().Replace("  ", " ").Split(' ');

			// vertices
			if (cpms [0] == "v") {
				vertices.Add (new Vector3 (float.Parse (cpms [1]), float.Parse (cpms [2]), float.Parse (cpms [3])));
			}

			// faces
			else if (cpms [0] == "f") 
			{
				if (cpms [1].Contains ("/"))
                {
                    // 非三角面
                    if (cpms.Length > 4)
                    {

                    }

                    // 三角面
                    else
                    {
                        for (int i = 1; i < cpms.Length; i++)
                        {
                            string data = cpms[i];
                            triangles.Add(int.Parse(data.Substring(0, data.IndexOf("/"))) - 1);
                        }
                    }
                }
				else 
				{
                    for (int i = 1; i < cpms.Length; i++)
                        triangles.Add(int.Parse(cpms[i]) - 1 - faceCount);
				}

			}

			// texture map
			else if (cpms [0] == "vt") 
			{

			}

			// normal map
			else if (cpms [0] == "vn") 
			{
                normals.Add(new Vector3(float.Parse(cpms[1]), float.Parse(cpms[2]), float.Parse(cpms[3])));
            }

			// group or object
			else if ( cpms[0]=="o") 
			{
				if (vertices.Count == 0 || triangles.Count==0)
					mesh.name = cpms [1];
				
				else 
				{
                    // 將資料加入mesh
                    faceCount += vertices.Count;
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

            else if(cpms[0] == "g")
            {

            }
		}

		// 將資料加入mesh
		mesh.SetVertices (vertices);
		mesh.triangles = triangles.ToArray ();
		meshList.Add (mesh);


		// 產生game obj
		GameObject mainObj = new GameObject();
        
		foreach (Mesh m in meshList) {

			GameObject obj = new GameObject (m.name);
			obj.transform.parent = mainObj.transform;
			MeshFilter mf = obj.AddComponent<MeshFilter> ();
			mf.mesh = m;
			MeshRenderer renderer = obj.AddComponent<MeshRenderer> ();
			renderer.material = new Material (Shader.Find ("Diffuse"));
			m.Optimize ();
			m.RecalculateNormals ();
            
		}
		return mainObj;
	}
}
