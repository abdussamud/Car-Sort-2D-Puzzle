using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Car puzzleCar;
    public bool isOccupide;
    public int rowNumber;
    public int columnNumber;


    private void Start()
    {
        List<char> charArrayOfCell = new();
        foreach (char alphabet in gameObject.name) { charArrayOfCell.Add(alphabet); }
        rowNumber = (int)char.GetNumericValue(charArrayOfCell[2]);
        columnNumber = (int)char.GetNumericValue(charArrayOfCell[6]);
        transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(!isOccupide);
    }

    public bool IsOccupide
    {
        get => isOccupide;
        set
        {
            isOccupide = value;
            transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(!isOccupide);
        }
    }
}
