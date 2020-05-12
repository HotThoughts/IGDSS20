using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Texture2D heightMap;
    public int dim;

    // Prefabs
    public Object waterTile;
    public Object sandTile;
    public Object grassTile;
    public Object forestTile;
    public Object stoneTile;
    public Object mountainTile;

    // Start is called before the first frame update
    void Start()
    {
        dim = heightMap.width;

        for (int i = 0; i < dim; i++)
            for (int j = 0; j < dim; j++)
            {
                float height = heightMap.GetPixel(i, j).r;

                // We need bias to arrange hexagons over X axis
                int bias = i % 2 == 0 ? 0 : 5;
                // Spawn tiles (8.66 IS MAGIC NUM HERE)
                float magicNum = 8.66f;
                if (height == 0f)
                    Instantiate(waterTile, 
                        new Vector3(i * magicNum, height * 10, j * 10 + bias), 
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0f && height <= 0.2f)
                    Instantiate(sandTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0.2f && height <= 0.4f)
                    Instantiate(grassTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0.4f && height <= 0.6f)
                    Instantiate(forestTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else if (height > 0.6f && height <= 0.8f)
                    Instantiate(stoneTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
                else
                    Instantiate(mountainTile,
                        new Vector3(i * magicNum, height * 10, j * 10 + bias),
                        new Quaternion(0f, 0f, 0f, 0f));
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
