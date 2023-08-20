using UnityEngine;

public class Car : MonoBehaviour
{
    public int carCode;
    public Cell parkingCell;
    public GameObject carLights;
    public ParticleSystem[] dust;
    private TouchManager tm;

    private void Awake()
    {
        tm = TouchManager.tm;
    }

    public int CarCode
    {
        get => carCode;
        set => carCode = value;
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
        parkingCell.IsOccupied = true;
    }

    public void CreateDust() { foreach (ParticleSystem ps in dust) { ps.Play(); } }

    public void ClearDust() { foreach (ParticleSystem ps in dust) { ps.Stop(); } }
}
