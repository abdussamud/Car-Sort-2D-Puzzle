using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [HideInInspector] public List<Cell> cellsHavingCar = new();
    public Cell[] rowCells;
    public int rowNumber;

    public bool IsCarColorSameInRow()
    {
        foreach (Cell cell in rowCells)
        {
            if (cell.isOccupide)
            {
                cellsHavingCar.Add(cell);
            }
        }
        int firstCarCode = cellsHavingCar[0].puzzleCar.carCode;
        foreach (Cell cell in cellsHavingCar)
        {
            if (cell.puzzleCar.carCode != firstCarCode)
            {
                cellsHavingCar.Clear(); return false;
            }
        }
        cellsHavingCar.Clear();
        return true;
    }
}
