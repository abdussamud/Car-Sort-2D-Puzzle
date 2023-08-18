using UnityEngine;

public class Car : MonoBehaviour
{
    public CarInfo carInfo;
    public Color carColor;
    public Cell parkingCell;
    public GameObject carLights;
    public ParticleSystem[] dust;
    private TouchManager tm;

    private void Awake()
    {
        tm = TouchManager.tm;
    }

    private void SetCarColor() { gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = carColor; }

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
        foreach (Cell cell in tm.parkingCells)
        {
            bool isPositionMatch = cell.transform.position == transform.position;
            parkingCell = isPositionMatch ? cell : parkingCell;
            cell.puzzleCar = isPositionMatch ? this : cell.puzzleCar;
        }
        parkingCell.IsOccupide = true;
    }

    public void CreateDust() { foreach (ParticleSystem ps in dust) { ps.Play(); } }

    public void ClearDust() { foreach (ParticleSystem ps in dust) { ps.Stop(); } }
}
