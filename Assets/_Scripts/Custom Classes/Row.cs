using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    public Cell[] rowCells;
    public int rowNumber;

    public bool IsCarColorSameInRow()
    {
        int firstCarCode = -1;

        foreach (Cell cell in rowCells)
        {
            if (cell.isOccupied)
            {
                if (firstCarCode == -1)
                {
                    firstCarCode = cell.puzzleCar.carCode;
                }
                else if (cell.puzzleCar.carCode != firstCarCode)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
