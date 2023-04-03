using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupide;
    public Car puzzleCar;
    public int rowNumber;
    public int columnNumber;
    private void Start()
    {
        List<char> charArrayOfCell = new();
        foreach (char alphabet in gameObject.name)
        {
            charArrayOfCell.Add(alphabet);
        }
        rowNumber = (int)char.GetNumericValue(charArrayOfCell[2]);
        columnNumber = (int)char.GetNumericValue(charArrayOfCell[6]);
    }

    public bool IsOccupide
    {
        get => isOccupide;
        set => isOccupide = value;
    }
}
