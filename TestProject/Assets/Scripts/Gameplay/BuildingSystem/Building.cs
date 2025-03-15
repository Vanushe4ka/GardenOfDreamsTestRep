using UnityEngine;

public class Building : MonoBehaviour
{
    const int ORDER_LAYER_UP_SHIFT = 1000;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private int _prefabID;

    public Sprite sprite => _spriteRenderer.sprite;

    public int PrefabID => _prefabID;

    [HideInInspector] public Vector2Int posInGrid;

    [HideInInspector] public int rows = 3;
    [HideInInspector] public int columns = 3;
    [HideInInspector] public int[] matrixData; // Одномерный массив для сериализации
    public int[,] matrix { get; private set; }

    private Color _defaultColor;

    private void OnValidate()
    {
        if (matrixData == null || matrixData.Length != rows * columns)
        {
            matrixData = new int[rows * columns];
        }
    }

    public int[,] GetMatrix()
    {
        int[,] matrix = new int[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix[i, j] = matrixData[i * columns + j];
            }
        }
        return matrix;
    }

    public void Initialize(Vector2Int posInGrid)
    {
        matrix = GetMatrix();
        _defaultColor = _spriteRenderer.color;
        this.posInGrid = posInGrid;
        DefineOrderLayer();
    }

    public void Select(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Unselect()
    {
        _spriteRenderer.color = _defaultColor;
    }

    public void DefineOrderLayer()
    {
        _spriteRenderer.sortingOrder = -posInGrid.y + ORDER_LAYER_UP_SHIFT;
    }

#if UNITY_EDITOR
    [SerializeField] Vector2 gizmosCellSize;
    private void OnDrawGizmos()
    {
        int[,] matrix = GetMatrix();
        Color redColor = Color.red;
        redColor.a = 0.25f;
        Color greenColor = Color.green;
        greenColor.a = 0.25f;
        
        for (int y =0;y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                Gizmos.color = (matrix[y, x] == 1 ? redColor : greenColor);
                Gizmos.DrawCube(new Vector2(x, y) * gizmosCellSize, gizmosCellSize);
            }
        }
    }
#endif
}