using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Car puzzleCar;
    public bool isOccupide;
    public int rowNumber;
    public int columnNumber;
    public GameObject greenParkingSign;

    private void Start()
    {
        List<char> charArrayOfCell = new();
        foreach (char alphabet in gameObject.name) { charArrayOfCell.Add(alphabet); }
        rowNumber = (int)char.GetNumericValue(charArrayOfCell[2]);
        columnNumber = (int)char.GetNumericValue(charArrayOfCell[6]);
        greenParkingSign.SetActive(!isOccupide);
    }

    public bool IsOccupide
    {
        get => isOccupide;
        set
        {
            isOccupide = value;
            greenParkingSign.SetActive(!isOccupide);
        }
    }
}
