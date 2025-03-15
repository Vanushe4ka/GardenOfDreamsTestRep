using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class VisualGrid : MonoBehaviour
{
    [SerializeField] Tilemap gridTilemap;
    [SerializeField] TileBase freeCell;
    [SerializeField] TileBase ocupedCell;

    public void DrawGrid(int[,] matrix)
    {
        for (int y=0; y < matrix.GetLength(0); y++)
        {
            for (int x =0;x < matrix.GetLength(1); x++)
            {
                gridTilemap.SetTile(new Vector3Int(x, y, 0), (matrix[y, x] == 0 ? freeCell : ocupedCell));
            }
        }
    }
}
