using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    public void Init()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    List<int> triangles = new List<int>();
    List<Vector3> vertices = new List<Vector3>();

    Mesh mesh;

    public int cX=0, cY=0, cZ=0;
    public static int cwidth = 16, cheight=64, clength=16;
    public double divider = 16.0, block_threshold=0.5;

    int[,,] map = new int[cwidth + 2, cheight + 2, clength + 2];
    double[,] omap = new double[cwidth + 2, clength + 2];

    public double maxHeight = 16;
    public double heightOffset = 62;

    bool first_time = true;
    public bool caves = true;

    public void UpdateChunk(double w=0)
    {
            mesh.Clear();
            triangles.Clear();
            vertices.Clear();

            //Overworld
            for (int x = 0; x < cwidth + 2; x++)
            {
                for (int z = 0; z < clength + 2; z++)
                {
                    omap[x, z] = ((NoiseS3D.Noise(((x - 1) + (cX * cwidth)) / divider, w, ((z - 1) + (cZ * clength)) / divider) + 1) / 2) * maxHeight + heightOffset;
                }
            }
            //omap Translation
            for (int x = 0; x < cwidth + 2; x++)
            {
                for (int y = 0; y < cheight + 2; y++)
                {
                    for (int z = 0; z < clength + 2; z++)
                    {
                        map[x, y, z] = omap[x, z] < y + (cY * cheight) ? 0 : 1;
                    }
                }
            }

            //Caves
            if (caves) {
                for (int x = 0; x < cwidth + 2; x++)
                {
                    for (int y = 0; y < cheight + 2; y++)
                    {
                        for (int z = 0; z < clength + 2; z++)
                        {
                            if (map[x, y, z] != 0) {
                                map[x, y, z] = NoiseS3D.Noise(((x - 1) + (cX * cwidth)) / divider, ((y - 1) + (cY * cheight)) / divider, ((z - 1) + (cZ * clength)) / divider, w) < block_threshold ? 1 : 0;
                            }
                        }
                    }
                }
            }

            //Mesh
            for (int x = 1; x < cwidth + 1; x++)
            {
                for (int y = 1; y < cheight + 1; y++)
                {
                    for (int z = 1; z < clength + 1; z++)
                    {
                        if (map[x, y, z] != 0)
                        {
                            /*AddQuad(0, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));
                            AddQuad(1, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));
                            AddQuad(2, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));
                            AddQuad(3, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));
                            AddQuad(4, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));
                            AddQuad(5, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));
                            AddQuad(6, new Vector3(x + (cX * cwidth), y + (cY * cheight), z + (cZ * clength)));*/
                            Vector3 vec = new Vector3(x - 1, y - 1, z - 1);

                            if (map[x, y + 1, z] == 0)
                            {
                                AddQuad(0, vec);
                            }
                            if (map[x, y - 1, z] == 0)
                            {
                                AddQuad(5, vec);
                            }
                            //MAY NEED EXPERIMENTAION!!!
                            if (map[x - 1, y, z] == 0)
                            {
                                AddQuad(1, vec);
                            }
                            if (map[x + 1, y, z] == 0)
                            {
                                AddQuad(4, vec);
                            }

                            if (map[x, y, z - 1] == 0)
                            {
                                AddQuad(2, vec);
                            }
                            if (map[x, y, z + 1] == 0)
                            {
                                AddQuad(3, vec);
                            }
                        }
                    }
                }
            }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void AddQuad(int dir, Vector3 pos)
    {

        int numVerts = vertices.Count;

        switch (dir)
        {
            case 0:
                vertices.Add(new Vector3(pos.x, pos.y + 1, pos.z));
                vertices.Add(new Vector3(pos.x, pos.y + 1, pos.z + 1));
                vertices.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z));
                vertices.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z + 1));
                break;
            case 1:
                vertices.Add(new Vector3(pos.x, pos.y, pos.z));
                vertices.Add(new Vector3(pos.x, pos.y, pos.z + 1));
                vertices.Add(new Vector3(pos.x, pos.y + 1, pos.z));
                vertices.Add(new Vector3(pos.x, pos.y + 1, pos.z + 1));
                break;
            case 2:
                vertices.Add(new Vector3(pos.x + 1, pos.y, pos.z));
                vertices.Add(new Vector3(pos.x, pos.y, pos.z));
                vertices.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z));
                vertices.Add(new Vector3(pos.x, pos.y + 1, pos.z));
                break;
            case 3:
                vertices.Add(new Vector3(pos.x, pos.y, pos.z + 1));
                vertices.Add(new Vector3(pos.x + 1, pos.y, pos.z + 1));
                vertices.Add(new Vector3(pos.x, pos.y + 1, pos.z + 1));
                vertices.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z + 1));
                break;
            case 4:
                vertices.Add(new Vector3(pos.x + 1, pos.y, pos.z + 1));
                vertices.Add(new Vector3(pos.x + 1, pos.y, pos.z));
                vertices.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z + 1));
                vertices.Add(new Vector3(pos.x + 1, pos.y + 1, pos.z));
                break;
            case 5:
                vertices.Add(new Vector3(pos.x, pos.y, pos.z + 1));
                vertices.Add(new Vector3(pos.x, pos.y, pos.z));
                vertices.Add(new Vector3(pos.x + 1, pos.y, pos.z + 1));
                vertices.Add(new Vector3(pos.x + 1, pos.y, pos.z));
                break;
        }

        triangles.Add(numVerts + 0);
        triangles.Add(numVerts + 1);
        triangles.Add(numVerts + 2);
        triangles.Add(numVerts + 1);
        triangles.Add(numVerts + 3);
        triangles.Add(numVerts + 2);
    }

}
