using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    public GameObject selectedObject;
    public GameController controller;
    public Cell oldParkingCell;
    public LayerMask carLayer;
    public LayerMask cellLayer;
    public List<Row> rowsList = new();
    //public List<Car> puzzleCars = new();
    public List<Cell> parkingCells = new();
    public bool gameOver;

    private bool checkCarColorInRow;
    private RaycastHit2D rayHit;
    private RaycastHit2D hitCell;
    private readonly float carScale = 0.2f;
    private bool selectable;

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }

        //Cell[] parkingCellsArray = FindObjectsOfType<Cell>();
        //foreach (Cell parkingCell in parkingCellsArray)
        //{
        //    parkingCells.Add(parkingCell);
        //}
        //
        //Row[] rowsArray = FindObjectsOfType<Row>();
        //foreach (Row row in rowsArray)
        //{
        //    rowsList.Add(row);
        //}
        //
        //Car[] puzzleCarsArray = FindObjectsOfType<Car>();
        //foreach (Car car in puzzleCarsArray)
        //{
        //    puzzleCars.Add(car);
        //}
    }

    private void Start()
    {
        //SetParkingCell();
        //SetRowList();
    }

    public void SetParkingCell()
    {
        Cell[] parkingCellsArray = FindObjectsOfType<Cell>();
        foreach (Cell parkingCell in parkingCellsArray)
        {
            parkingCells.Add(parkingCell);
        }
    }

    public void SetRowList()
    {
        Row[] rowsArray = FindObjectsOfType<Row>();
        foreach (Row row in rowsArray)
        {
            rowsList.Add(row);
        }
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0) && selectedObject == null && !gameOver)
        //{
        //    rayHit = Cast2DRay;
        //    if (rayHit.collider && rayHit.collider.GetComponent<Car>().IsMoveable() && rayHit.collider.CompareTag("Car"))
        //    {
        //        //selectable = true;
        //    }
        //}
        if (Input.GetMouseButtonUp(0) && selectedObject == null && !gameOver)
        {
            rayHit = Cast2DRay;
            if (rayHit.collider && rayHit.collider.CompareTag("Car"))// && selectable)
            {
                selectedObject = rayHit.collider.gameObject;
                selectedObject.transform.localScale += new Vector3(carScale, carScale, carScale);
                oldParkingCell = selectedObject.GetComponent<Car>().parkingCell;
                selectable = false;
            }
        }
        else if (Input.GetMouseButtonUp(0) && selectedObject != null && !gameOver)
        {
            rayHit = Cast2DRay;
            hitCell = Cast2DRayForCell;
            if (hitCell.collider && hitCell.collider.CompareTag("CarPosCell") && !hitCell.collider.GetComponent<Cell>().IsOccupide)
            {
                if ((hitCell.collider.transform.position.x - selectedObject.transform.position.x) *
                    (hitCell.collider.transform.position.x - selectedObject.transform.position.x) < (1.21f * 1.21f)
                    || (hitCell.collider.transform.position.y - selectedObject.transform.position.y) *
                    (hitCell.collider.transform.position.y - selectedObject.transform.position.y) < (2.41f * 2.41f))
                {
                    oldParkingCell.IsOccupide = false;
                    selectedObject.transform.position = hitCell.collider.gameObject.transform.position;
                    selectedObject.transform.GetComponent<Car>().SetParkingCell();
                    oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
                    oldParkingCell.IsOccupide = true;
                    selectedObject.transform.localScale -= new Vector3(carScale, carScale, carScale);
                    selectedObject = null;
                    checkCarColorInRow = true;
                }
            }
            if (rayHit.collider && rayHit.collider.CompareTag("Car") && selectedObject != rayHit.collider.gameObject)
            {
                selectedObject.transform.localScale -= new Vector3(carScale, carScale, carScale);
                selectedObject = rayHit.collider.gameObject;
                selectedObject.transform.localScale += new Vector3(carScale, carScale, carScale);
                oldParkingCell = selectedObject.GetComponent<Car>().parkingCell;
            }
            else if (rayHit.collider && rayHit.collider.CompareTag("Car") && selectedObject == rayHit.collider.gameObject)
            {
                selectedObject.transform.localScale -= new Vector3(carScale, carScale, carScale);
                selectedObject = null;
            }
        }
        if (checkCarColorInRow)
        {
            CheckWiningConditions();
            checkCarColorInRow = false;
        }

    }
    #endregion

    #region Private Methods
    private RaycastHit2D Cast2DRay => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, carLayer);

    private RaycastHit2D Cast2DRayForCell => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, cellLayer);

    private void CheckWiningConditions()
    {
        if (rowsList[0].IsCarColorSameInRow() && rowsList[1].IsCarColorSameInRow() && rowsList[2].IsCarColorSameInRow())
        {
            gameOver = true;
            controller.EndGameDelay();
            Debug.Log("YouWon!");
        }
    }
    #endregion
}
