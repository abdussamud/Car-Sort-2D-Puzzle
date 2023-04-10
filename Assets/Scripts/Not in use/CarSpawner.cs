using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPref;
    public List<Cell> cells;
    public List<Color> randomColor;
    private int carAmount;


    private void Start()
    {
        carAmount = GameManager.Instance.currentLevel < 10 ? 8 : GameManager.Instance.currentLevel is > 9 and < 20 ? 11 : 14;

        for (int i = 0; i < 3; i++)
        {
            randomColor.Add(new(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        }
        foreach (Cell parkingCell in TouchManager.Instance.parkingCells)
        {
            cells.Add(parkingCell);
        }
        for (int i = 0; i < carAmount; i++)
        {
            SpawnPrefab(i);
        }
    }

    private void SpawnPrefab(int carNumber)
    {
        int randomIndex = Random.Range(0, cells.Count);
        Transform spawnPoint = cells[randomIndex].transform;
        GameObject spawnedPrefab = Instantiate(carPref, spawnPoint.position, Quaternion.identity);
        spawnedPrefab.transform.SetParent(gameObject.transform);

        if (carAmount == 8)
        {
            if (carNumber is 0 or 1 or 2) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[0]; }
            if (carNumber is 3 or 4 or 5) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[1]; }
            if (carNumber is 6 or 7) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[2]; }
        }
        else if (carAmount == 11)
        {
            if (carNumber is 0 or 1 or 2 or 3) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[0]; }
            if (carNumber is 4 or 5 or 6 or 7) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[1]; }
            if (carNumber is 8 or 9 or 10) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[2]; }
        }
        else if (carAmount == 14)
        {
            if (carNumber is 0 or 1 or 2 or 3 or 4) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[0]; }
            if (carNumber is 5 or 6 or 7 or 8 or 9) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[1]; }
            if (carNumber is 10 or 11 or 12 or 13) { spawnedPrefab.GetComponent<Car>().carColor = randomColor[2]; }
        }

        cells.RemoveAt(randomIndex);
        if (carNumber == carAmount - 1) { cells.Clear(); }
    }
}
