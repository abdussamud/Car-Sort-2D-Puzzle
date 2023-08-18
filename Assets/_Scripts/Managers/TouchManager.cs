using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    #region Variables
    public static TouchManager tm;

    public GameObject selectedObject;
    public Cell oldParkingCell;
    public LayerMask carLayer;
    public LayerMask cellLayer;
    private RaycastHit2D rayHit;
    private RaycastHit2D hitCell;
    public bool gameOver;
    public int moveCount;
    private bool checkCarColorInRow;
    public List<Row> rowsList = new();
    public List<Cell> parkingCells = new();
    private const float DURATION = 0.3f;
    private const float DURATION1 = 0.5f;
    private Vector3 desiredPosition;
    private GameController gc;
    private GameplayUI gameplayUI;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        tm = this;
    }

    private void Start()
    {
        gc = GameController.gc;
        gameplayUI = GameplayUI.gui;
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
                    gameOver = true;
                    selectedObject.GetComponent<Car>().CreateDust();
                    int moveDir = ((_ = deltaX <= 0.01f ? (int)deltaX : deltaX) == 0 && deltaY > 0) ? 2 :
                        (deltaX > 0 && (_ = deltaY <= 0.01f ? (int)deltaY : deltaY) == 0) ? 1 : 0;
                    _ = StartCoroutine(moveDir == 2 ? StraightCarMotion() : moveDir == 1 ?
                        (((_ = deltaX <= 0.01f ? (int)deltaX : deltaX) == 0 && deltaY > 0) ?
                        MoveToLeft() : MoveToRight()) : null);
                }
                else
                {
                    gameplayUI.WrongMoveTextPrompter();
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
            //CheckWiningConditions();
            checkCarColorInRow = false;
        }
    }
    #endregion

    #region Private Methods
    private RaycastHit2D Cast2DRay => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, carLayer);

    private RaycastHit2D Cast2DRayForCell => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, cellLayer);

    private bool CheckWiningConditions()
    {
        foreach (Row r in rowsList)
        {
            if (!r.IsCarColorSameInRow())
            {
                return false;
            }
        }
        gc.EndGameDelay();
        return true;
    }

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
        selectedObject.GetComponent<Car>().ClearDust();
        selectedObject = null;
        if (moveCount > 1)
        {
            gameOver = false;
            moveCount--;
            gameplayUI.UpdateMoveText();
            CheckWiningConditions();
        }
        else if (!CheckWiningConditions())
        {
            gameplayUI.LevelFailed();
        }
    }
    
    private IEnumerator MoveToLeft()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 endPos = desiredPosition;
        Vector3 startPos1 = new(startPos.x, startPos.y + 0.7f, startPos.z);
        Vector3 startPos2 = new(startPos.x + 0.4f, startPos.y + 1f, startPos.z);
        Vector3 startPos3 = new(startPos.x - 0.8f, startPos.y + 1f, startPos.z);
        Vector3 startPos4 = new(endPos.x, endPos.y + 0.7f, endPos.z);

        // Step 1 Move Back
        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, startPos1, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = startPos1;

        // Step 2 Rotate and Move Back
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.SetPositionAndRotation(Vector3.Lerp(startPos1, startPos2, progress), Quaternion.AngleAxis(-90 * progress, Vector3.forward));
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = startPos2;

        // Step 3 Move Forward
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos2, startPos3, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = startPos3;

        // Step 4 Rotate and Move Forward
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.SetPositionAndRotation(Vector3.Lerp(startPos3, startPos4, progress), Quaternion.AngleAxis(-90 + (90 * progress), Vector3.forward));
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.SetPositionAndRotation(startPos4, Quaternion.identity);

        //Final Step 5 Move Foraward
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos4, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject.GetComponent<Car>().ClearDust();
        selectedObject = null;
        if (moveCount > 1)
        {
            gameOver = false;
            moveCount--;
            gameplayUI.UpdateMoveText();
            CheckWiningConditions();
        }
        else if (!CheckWiningConditions())
        {
            gameplayUI.LevelFailed();
        }
    }
    
    private IEnumerator MoveToRight()
    {
        Vector3 startPos = selectedObject.transform.position;
        Vector3 endPos = desiredPosition;
        Vector3 startPos1 = new(startPos.x, startPos.y + 0.7f, startPos.z);
        Vector3 startPos2 = new(startPos.x - 0.4f, startPos.y + 1f, startPos.z);
        Vector3 startPos3 = new(startPos.x + 0.6f, endPos.y + 1f, endPos.z);
        Vector3 startPos4 = new(endPos.x, endPos.y + 0.7f, endPos.z);

        // Step 1 Move Back
        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos, startPos1, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = startPos1;

        // Step 2 Rotate and Move Back
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.SetPositionAndRotation(Vector3.Lerp(startPos1, startPos2, progress), Quaternion.AngleAxis(90 * progress, Vector3.forward));
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = startPos2;

        // Step 3 Move Forward
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos2, startPos3, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = startPos3;

        // Step 4 Rotate and Move Forward
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.SetPositionAndRotation(Vector3.Lerp(startPos3, startPos4, progress), Quaternion.AngleAxis(90 - (90 * progress), Vector3.forward));
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.SetPositionAndRotation(startPos4, Quaternion.identity);

        //Final Step 5 Move Foraward
        elapsedTime = 0;
        progress = 0;
        while (progress <= 1)
        {
            selectedObject.transform.position = Vector3.Lerp(startPos4, endPos, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION1;
            yield return null;
        }
        selectedObject.transform.position = endPos;
        selectedObject.transform.GetComponent<Car>().SetParkingCell();
        oldParkingCell = selectedObject.transform.GetComponent<Car>().GetParkingCell;
        oldParkingCell.IsOccupide = true;
        selectedObject.GetComponent<Car>().carLights.SetActive(false);
        selectedObject.GetComponent<Car>().ClearDust();
        selectedObject = null;
        if (moveCount > 1)
        {
            gameOver = false;
            moveCount--;
            gameplayUI.UpdateMoveText();
            CheckWiningConditions();
        }
        else if (!CheckWiningConditions())
        {
            gameplayUI.LevelFailedDelay();
        }
    }
    #endregion

    #region Public Methods
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
    #endregion
}
