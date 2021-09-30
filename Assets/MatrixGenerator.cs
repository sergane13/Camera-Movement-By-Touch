using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixGenerator : MonoBehaviour
{
    private int[,] matrix;

    public int row;
    public int col;
    
    void Start()
    {
        matrix = new int[row, col];

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                matrix[i, j] = 0;
            }
        }

        PatternOne();

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                print(matrix[i, j]);
            }
        }
    }

    private void PatternOne()
    {
        int start_x = Random.Range(2, row-3);
        int start_y = Random.Range(1, col-4); 

        matrix[start_x, start_y] = 1;
        matrix[start_x, start_y + 1] = 1;
        matrix[start_x, start_y + 2] = 1;
        matrix[start_x, start_y + 3] = 1;

        matrix[start_x - 1, start_y + 1] = 1;
        matrix[start_x - 1, start_y + 1] = 1;

        matrix[start_x - 2, start_y + 1] = 1;

        matrix[start_x + 1, start_y + 2] = 1;
        matrix[start_x + 1, start_y + 3] = 1;
    }
}
