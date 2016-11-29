using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class ReadTerrainData : MonoBehaviour {

    public char seperator;
    public Terrain terrain;
	// Use this for initialization
	void Start ()
    {

        // get terrain heightmap width and height
        int xRes = terrain.terrainData.heightmapWidth;
        int yRes = terrain.terrainData.heightmapHeight;



        // read xyz
        List<Vector3> points = ReadAllLinesToList(Application.dataPath + @"/111.txt");

        // get heights - gets the heightmap points of the terrain. Store those values in a float array
        float[,] heights = terrain.terrainData.GetHeights(0, 0, xRes, yRes);

        // get extreme values
        float minX = 0f, maxX = 0f, minY = 0f, maxY = 0f, minZ = 0f, maxZ = 0f;
        int count = 0;
        foreach(Vector3 p in points)
        {
            if (count == 0)
            {
                minX = p.x;
                maxX = p.x;
                minY = p.y;
                maxY = p.y;
                minZ = p.z;
                maxZ = p.z;
            }
            else
            {
                if (p.x < minX)
                    minX = p.x;
                if (p.x > maxX)
                    maxX = p.x;
                if (p.y < minY)
                    minY = p.y;
                if (p.y > maxY)
                    maxY = p.y;
                if (p.z < minZ)
                    minZ = p.z;
                if (p.z > maxZ)
                    maxZ = p.z;
            }

            count++;
        }

        // set terrain size
        Vector3 size = new Vector3(maxX - minX, maxZ, maxY - minY);
        terrain.terrainData.size = size;

        // put points in array
        List<int> xpos = new List<int>();
        List<int> ypos = new List<int>();
        foreach (Vector3 p in points)
        {
            int x = Mathf.RoundToInt((p.x - minX) / (maxX - minX) * (xRes - 1));
            int y = Mathf.RoundToInt((p.y - minY) / (maxY - minY) * (yRes - 1));
            if(!xpos.Contains(x))
                xpos.Add(x);
            if (!ypos.Contains(y))
                ypos.Add(y);
            heights[x, y] = (p.z - minZ) / (maxZ - minZ);
        }

        // Bilinear interpolation
        for(int i=0; i<xpos.Count-1;i++)
        {
            for(int j=0; j<ypos.Count-1;j++)
            {
                for(int k = xpos[i]; k<=xpos[i+1]; k++)
                {
                    for(int m = ypos[j]; m<= ypos[j+1]; m++)
                    {
                        heights[k, m] = BiLerp(heights[xpos[i], ypos[j]], heights[xpos[i + 1], ypos[j]], heights[xpos[i], ypos[j + 1]], heights[xpos[i + 1], ypos[j + 1]],(k-xpos[i])*1.0f/(xpos[i+1]-xpos[i]), (m - ypos[j])*1.0f / (ypos[j + 1] - ypos[j]));
                    }
                }
            }
        }

        terrain.terrainData.SetHeights(0, 0, heights);

    }

    public List<Vector3> ReadAllLinesToList(string path)
    {
        List<Vector3> points = new List<Vector3>();
        StreamReader reader = new StreamReader(path);
        string aLine = string.Empty;
        while ((aLine = reader.ReadLine()) != null)
        {

            string[] xyz = aLine.Trim().Split(seperator);
            Vector3 p = new Vector3();
            p.x = float.Parse(xyz[0]);
            p.y = float.Parse(xyz[1]);
            p.z = float.Parse(xyz[2]);
            points.Add(p);

        }
        return points;
    }

    public float BiLerp(float x0y0, float x1y0, float x0y1, float x1y1, float posx, float posy)
    {
        float r1 = posx * (x1y0 - x0y0) + x0y0;
        float r2 = posx * (x1y1 - x0y1) + x0y1;
        return posy * (r2 - r1) + r1;
    }


}
