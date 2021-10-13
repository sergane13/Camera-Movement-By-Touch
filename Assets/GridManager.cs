using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int rows = 5;

    [SerializeField]
    private int cols = 5;

    [SerializeField]
    private float tilesSize = 1;

    public GameObject stone;
   
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile  = (GameObject)Instantiate(stone, transform);

                float posX = col * tilesSize;
                float posY = row * -tilesSize;

                tile.transform.position = new Vector2(posX, posY);
            }         
        }
        Destroy(stone);
    }
}