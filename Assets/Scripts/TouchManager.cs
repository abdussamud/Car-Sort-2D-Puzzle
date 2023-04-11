using System.Collections;
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
    public List<Cell> parkingCells = new();
    public bool gameOver;

    private bool checkCarColorInRow;
    private RaycastHit2D rayHit;
    private RaycastHit2D hitCell;
    private readonly float carScale = 0.15f;

    #region Unity Methods
    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
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
        if (Input.GetMouseButtonUp(0) && selectedObject == null && !gameOver)
        {
            rayHit = Cast2DRay;
            if (rayHit.collider && rayHit.collider.CompareTag("Car"))
            {
                selectedObject = rayHit.collider.gameObject;
                selectedObject.GetComponent<Car>().carLights.SetActive(true);
                oldParkingCell = selectedObject.GetComponent<Car>().parkingCell;
            }
        }
        else if (Input.GetMouseButtonUp(0) && selectedObject != null && !gameOver)
        {
            rayHit = Cast2DRay;
            hitCell = Cast2DRayForCell;
            if (hitCell.collider && hitCell.collider.CompareTag("CarPosCell") && !hitCell.collider.GetComponent<Cell>().IsOccupide)
            {
                float deltaX = Mathf.Clamp(Mathf.Abs(hitCell.collider.transform.position.x - selectedObject.transform.position.x), 0, 5);
                float deltaY = Mathf.Clamp(Mathf.Abs(hitCell.collider.transform.position.y - selectedObject.transform.position.y), 0, 5);
                if (deltaX <= 1.2f && deltaY <= 2.4f &&
                    (((deltaX = deltaX <= 0.01f ? (int)deltaX : deltaX) == 0 && deltaY > 0) ||
                    (deltaX > 0 && (deltaY = deltaY <= 0.01f ? (int)deltaY : deltaY) == 0)))
                {
                    oldParkingCell.IsOccupide = false;
                    oldParkingCell.puzzleCar = null;
                    desiredPosition = hitCell.collider.gameObject.transform.position;
                    if ((_ = deltaX <= 0.01f ? (int)deltaX : deltaX) == 0 && deltaY > 0)
                    {
                        gameOver = true;
                        StartCoroutine(StraightCarMotion());
                    }
                    else if (deltaX > 0 && (_ = deltaY <= 0.01f ? (int)deltaY : deltaY) == 0)
                    {
                        gameOver = true;
                        StartCoroutine(MoveToLeftR2R3());
                    }
                }
                else
                {
                    UIManager.Instance.WrongMoveTextPrompter();
                }
            }
            if (rayHit.collider && rayHit.collider.CompareTag("Car") && selectedObject != rayHit.collider.gameObject)
            {
                selectedObject.GetComponent<Car>().carLights.SetActive(false);
                selectedObject = rayHit.collider.gameObject;
                selectedObject.GetComponent<Car>().carLights.SetActive(true);
                oldParkingCell = selectedObject.GetComponent<Car>().parkingCell;
            }
            else if (rayHit.collider && rayHit.collider.CompareTag("Car") && selectedObject == rayHit.collider.gameObject)
            {
                selectedObject.GetComponent<Car>().carLights.SetActive(false);
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
            controller.EndGameDelay();
        }
    }
    #endregion

    private const float DURATION = 1f;
    private Vector3 desiredPosition;
    private IEnumerator StraightCarMotion()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 endPos = desiredPosition;

        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject = null;
        checkCarColorInRow = true;
        gameOver = false;
    }
    private IEnumerator MoveToLeftR2R3()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 startPos1 = new(selectedObject.transform.position.x,selectedObject.transform.position.y + 0.7f,selectedObject.transform.position.z);
        Vector3 startPos2 = new(selectedObject.transform.position.x - 2.2f,selectedObject.transform.position.y + 1f,selectedObject.transform.position.z);
        Vector3 startPos3 = new(selectedObject.transform.position.x - 1.2f,selectedObject.transform.position.y + 0.7f, selectedObject.transform.position.z);
        Vector3 endPos = desiredPosition;

        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, startPos1, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = startPos1;
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Slerp(startPos1, startPos2, progress);
            selectedObject.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = startPos2;
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Slerp(startPos2, startPos3, progress);
            selectedObject.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = startPos3;
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos3, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject = null;
        checkCarColorInRow = true;
        gameOver = false;
    }
    IEnumerator<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, float centerOffset)
    {
        var centerPivot = (start + end) * 0.5f;

        centerPivot -= new Vector3(0, -centerOffset);

        var startRelativeCenter = start - centerPivot;
        var endRelativeCenter = end - centerPivot;

        var f = 1f / 10;

        for (var i = 0f; i < 1 + f; i += f)
        {
            yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centerPivot;
        }
    }
    private IEnumerator MoveToRightR2R3()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 endPos = desiredPosition;

        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject = null;
        checkCarColorInRow = true;
        gameOver = false;
    }
    private IEnumerator MoveToLeftR1()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 endPos = desiredPosition;

        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject = null;
        checkCarColorInRow = true;
        gameOver = false;
    }
    private IEnumerator MoveToRightR1()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 endPos = desiredPosition;

        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject = null;
        checkCarColorInRow = true;
        gameOver = false;
    }
}
