using UnityEngine;
using UnityEngine.Tilemaps;
public class VisualGrid : MonoBehaviour
{
    [SerializeField] private Tilemap _gridTilemap;
    [SerializeField] private TileBase _freeCell;
    [SerializeField] private TileBase _ocupedCell;

    public void DrawGrid(int[,] matrix)
    {
        for (int y=0; y < matrix.GetLength(0); y++)
        {
            for (int x =0;x < matrix.GetLength(1); x++)
            {
                _gridTilemap.SetTile(new Vector3Int(x, y, 0), (matrix[y, x] == 0 ? _freeCell : _ocupedCell));
            }
        }
    }
}
