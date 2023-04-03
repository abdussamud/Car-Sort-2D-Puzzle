using UnityEngine;

public class Car : MonoBehaviour
{
    public Color carColor;
    public Cell parkingCell;
    public bool isMoveable;


    public void SetCarColor() => gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = carColor;

    public Cell GetParkingCell => parkingCell;

    public void SetParkingCell()
    {
        foreach (Cell cell in TouchManager.Instance.parkingCells)
        {
            if (cell.transform.position == transform.position)
            {
                parkingCell = cell;
                cell.puzzleCar = this;
            }
        }
        parkingCell.IsOccupide = true;

        Invoke(nameof(IsMoveable), 0.3f);
    }

    public bool IsMoveable()
    {
        Cell upperCell = new();
        Cell leftCell = new();
        Cell rightCell = new();
        Cell lowerCell = new();
        if (parkingCell.rowNumber == 0 && parkingCell.columnNumber == 0)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 0 && cell.columnNumber == 1)
                {
                    rightCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 0)
                {
                    lowerCell = cell;
                }
            }
            if (!rightCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!lowerCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 0 && parkingCell.columnNumber == 1)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 0 && cell.columnNumber == 0)
                {
                    leftCell = cell;
                }
                if (cell.rowNumber == 0 && cell.columnNumber == 2)
                {
                    rightCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 1)
                {
                    lowerCell = cell;
                }
            }
            if (!leftCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!rightCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!lowerCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 0 && parkingCell.columnNumber == 2)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 0 && cell.columnNumber == 1)
                {
                    leftCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 2)
                {
                    lowerCell = cell;
                }
            }
            if (!leftCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!lowerCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 1 && parkingCell.columnNumber == 0)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 0 && cell.columnNumber == 0)
                {
                    upperCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 1)
                {
                    rightCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 0)
                {
                    lowerCell = cell;
                }
            }
            if (!upperCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!rightCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!lowerCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 1 && parkingCell.columnNumber == 1)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 0 && cell.columnNumber == 1)
                {
                    upperCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 0)
                {
                    leftCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 2)
                {
                    rightCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 1)
                {
                    lowerCell = cell;
                }
            }
            if (!upperCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!leftCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!rightCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!lowerCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 1 && parkingCell.columnNumber == 2)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 0 && cell.columnNumber == 2)
                {
                    upperCell = cell;
                }
                if (cell.rowNumber == 1 && cell.columnNumber == 1)
                {
                    leftCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 2)
                {
                    lowerCell = cell;
                }
            }
            if (!upperCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!leftCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!lowerCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 2 && parkingCell.columnNumber == 0)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 1 && cell.columnNumber == 0)
                {
                    upperCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 1)
                {
                    rightCell = cell;
                }
            }
            if (!upperCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!rightCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 2 && parkingCell.columnNumber == 1)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 1 && cell.columnNumber == 1)
                {
                    upperCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 0)
                {
                    leftCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 2)
                {
                    rightCell = cell;
                }
            }
            if (!upperCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!leftCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!rightCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        else if (parkingCell.rowNumber == 2 && parkingCell.columnNumber == 2)
        {
            foreach (Cell cell in TouchManager.Instance.parkingCells)
            {
                if (cell.rowNumber == 1 && cell.columnNumber == 2)
                {
                    upperCell = cell;
                }
                if (cell.rowNumber == 2 && cell.columnNumber == 1)
                {
                    leftCell = cell;
                }
            }
            if (!upperCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
            if (!leftCell.IsOccupide)
            {
                isMoveable = true;
                return true;
            }
        }
        isMoveable = false;
        return false;
    }
}
