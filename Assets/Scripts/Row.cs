using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    [HideInInspector]
    public List<Cell> cellsHavingCar = new();
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
        Color firstColor = cellsHavingCar[0].puzzleCar.carColor;
        foreach (Cell cell in cellsHavingCar)
        {
            if (cell.puzzleCar.carColor != firstColor)
            {
                cellsHavingCar.Clear();
                return false;
            }
        }
        cellsHavingCar.Clear();
        Debug.Log("Row " + rowNumber + " have Same Color car");
        return true;
    }
}
