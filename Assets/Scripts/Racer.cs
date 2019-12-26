using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    public GameObject wallGameObject;

    public Vector2 startPos;
    public Vector2 endPos;
    public int direction;

    public Wall(Vector2 startPos, int direction)
    {
        this.startPos = startPos;
        this.endPos = startPos;
        this.direction = direction;
        Debug.Log("create primitive called");
        wallGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallGameObject.transform.position = new Vector3(startPos.x, startPos.y, 0);
    }
}

public class Racer : MonoBehaviour
{
    private Vector2 gridPosition;
    private float moveTimer;
    private float moveTimerMax;


    private static int UP = 0;
    private static int DOWN = 1;
    private static int LEFT = 2;
    private static int RIGHT = 3;

    private int currentDirection = UP;
    private float velocity = 50.0f;

    private bool currentlyWalling = false;
    private Wall currentWall;
    private List<Wall> walls;

    public Floor floor;

    public void AwakeBase(int x, int y)
    {
        gridPosition = new Vector2(x, y);
        moveTimerMax = 1f/60f;
        moveTimer = moveTimerMax;
        floor = GameObject.Find("Floor").GetComponent<Floor>();
        walls = new List<Wall>();
    }

    public Vector2 getPosition()
    {
        return gridPosition;
    }

    // Update is called once per frame
    public void UpdateBase(
        bool upPressed,
        bool downPressed,
        bool leftPressed,
        bool rightPressed,
        bool wallKeyPressed)
    {
        if (upPressed)
        {
            currentDirection = UP;
        }
        else if (downPressed)
        {
            currentDirection = DOWN;
        }
        else if (leftPressed)
        {
            currentDirection = LEFT;
        }
        else if (rightPressed)
        {
            currentDirection = RIGHT;
        }

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveTimerMax) {
            if (currentDirection == UP)
            {
                gridPosition.y += moveTimerMax * velocity;
            }
            else if (currentDirection == DOWN)
            {
                gridPosition.y -= moveTimerMax * velocity;
            }
            else if (currentDirection == LEFT)
            {
                gridPosition.x -= moveTimerMax * velocity;
            }
            else if (currentDirection == RIGHT)
            {
                gridPosition.x += moveTimerMax * velocity;
            }
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            moveTimer = 0;
            floor.shipMoved(this);
            HandleWall(wallKeyPressed, currentDirection);
        }
    }

    private void StartWall(Vector2 startPos, int direction)
    {
        currentWall = new Wall(startPos, direction);
        Debug.Log("starting wall at " + startPos.x + "," + startPos.y);
    }

    private void EndWall(Vector2 endPos)
    {
        currentWall.endPos = endPos;
        Debug.Log("ending wall at " + endPos.x + "," + endPos.y);
        walls.Add(currentWall);

    }

    private void UpdateWall(Vector2 curPos)
    {
        currentWall.endPos = curPos;
        Debug.Log("updating wall");
        //currentWall.wallGameObject.transform.position = new Vector3(curPos.x, curPos.y, 0);
        Vector2 diff = curPos - currentWall.startPos;
        Vector2 avg = (curPos + currentWall.startPos) / 2;
        currentWall.wallGameObject.transform.localScale = new Vector3(
            System.Math.Max(1, System.Math.Abs(diff.x)),
            System.Math.Max(1, System.Math.Abs(diff.y)),
            1);
        currentWall.wallGameObject.transform.position = new Vector3(avg.x, avg.y);
    }

    private void HandleWall(bool wallKeyPressed, int curDirection)
    {
        if (!currentlyWalling && !wallKeyPressed)
        {
            return;
        }
        if (!currentlyWalling && wallKeyPressed)
        {
            currentlyWalling = true;
            StartWall(gridPosition, curDirection);
            return;
        }
        if (currentlyWalling && !wallKeyPressed)
        {
            currentlyWalling = false;
            Debug.Log("ending because wall key not pressed");
            EndWall(gridPosition);
            return;
        }
        if (currentlyWalling && wallKeyPressed)
        {
            Debug.Log("currently walling and holding wall button");
            if (curDirection != currentWall.direction)
            {
                Debug.Log("ending because changed direction");
                EndWall(gridPosition);
                StartWall(gridPosition, curDirection);
                return;
            } else
            {
                UpdateWall(gridPosition);
                return;
            }
        }
    }
}
