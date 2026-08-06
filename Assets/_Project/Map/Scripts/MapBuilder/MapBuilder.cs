using Cysharp.Text;
using UnityEngine;
using ZLinq;

public class MapBuilder : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int _cellsCentersOffset;

    [Header("Объект карты")]
    [SerializeField] private GameObject _mapTemplate;

    [Header("Контейнер клеток")]
    [SerializeField] private GameObject _cellsContainer;

    private (int x, int y) _zeroCoords;

    public void BuildMap()
    {
        FindZeroCoords();
        var cells = _cellsContainer.Children().OfComponent<Cell>();
        foreach(var cell in cells)
        {
            (int x, int z) coords = FindCoords(cell.GridPosition.x, cell.GridPosition.y);
            cell.transform.position = new Vector3(coords.x, 0, coords.z);

            var rotationAngle = GetRotationAngle(cell.HasNorthNeighbor, cell.HasWestNeighbor, cell.HasEastNeighbor, cell.HasSouthNeighbor);
            cell.transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

            cell.name = ZString.Format("Cell [{0}, {1}]", cell.GridPosition.x, cell.GridPosition.y);
        }
    }

    private void FindZeroCoords()
    {
        var x = 2 - 2 * _gridSize.x;
        var y = 2 - 2 * _gridSize.y;
        _zeroCoords = (x, y);
    }

    private (int, int) FindCoords(int gridPositionX, int gridPositionY)
    {
        return (_zeroCoords.x + _cellsCentersOffset * gridPositionX, _zeroCoords.y + _cellsCentersOffset * gridPositionY);
    }

    private float GetRotationAngle(bool north, bool west, bool east, bool south)
    {
        if (north && west && east && south) return 0f;

        if (south && west && north) return 0f;
        if (west && north && east) return 90f;
        if (north && east && south) return 180f;
        if (east && south && west) return 270f;

        if (west && north) return 0f;
        if (north && east) return 90f;
        if (east && south) return 180f;
        if (south && west) return 270f;

        return 0f;
    }
}
