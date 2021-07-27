using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManagement : MonoBehaviour
{

    public GameObject chunkPrefab;

    public int fXc=0, fYc=0, fZc=0, tXc=1, tYc=1, tZc=1;
    public double w = 0.0;
    int i_chunk = 0;

    public bool createChunks=true,updateChunks = true;

    List<GameObject> chunks;


    // Start is called before the first frame update
    void Start()
    {
        chunks = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (createChunks)
        {
            for (int x=fXc;x<tXc;x++) {
                for (int y = fYc; y < tYc; y++) {
                    for (int z = fZc; z < tZc; z++) {
                        chunks.Add(Instantiate(chunkPrefab,new Vector3(x*ChunkGenerator.cwidth,y * ChunkGenerator.cheight, z * ChunkGenerator.clength),transform.rotation));
                        ChunkGenerator cgen = (ChunkGenerator)(chunks[chunks.Count - 1].GetComponent("ChunkGenerator"));
                        cgen.Init();
                        cgen.cX = x;
                        cgen.cY = y;
                        cgen.cZ = z;
                        cgen.UpdateChunk(w);
                    }
                }
            }

            createChunks = false;
        }
        if (updateChunks)
        {
            /*for (int i=0;i<chunks.Count;i++)
            {
                ChunkGenerator cgen = (ChunkGenerator)chunks[i].GetComponent("ChunkGenerator");
                cgen.UpdateChunk(w);
            }*/
            ChunkGenerator cgen = (ChunkGenerator)chunks[i_chunk++].GetComponent("ChunkGenerator");
            cgen.UpdateChunk(w);
            if (i_chunk==chunks.Count)
            {
                i_chunk = 0;
                w += 0.001;
            }

            //updateChunks = false;
        }
    }

}
