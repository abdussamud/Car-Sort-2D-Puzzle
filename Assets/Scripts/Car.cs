using UnityEngine;

public class Car : MonoBehaviour
{
    public Color carColor;
    public Cell parkingCell;
    public GameObject carLights;
    public ParticleSystem[] dust;


    private void SetCarColor() => gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = carColor;

    public Color CarColor
    {
        get => carColor;
        set
        {
            carColor = value;
            SetCarColor();
        }
    }

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
    }

    public void CreateDust()
    {
        dust[0].Play();
        dust[1].Play();
        dust[2].Play();
        dust[3].Play();
    }

    public void ClearDust()
    {
        dust[0].Stop();
        dust[1].Stop();
        dust[2].Stop();
        dust[3].Stop();
    }
}
