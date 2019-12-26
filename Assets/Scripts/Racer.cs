using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Floor floor;

    public void Awake(int x, int y)
    {
        gridPosition = new Vector2(x, y);
        moveTimerMax = 1f/60f;
        moveTimer = moveTimerMax;
        floor = GameObject.Find("Floor").GetComponent<Floor>();
    }

    // Update is called once per frame
    public void Update(
        bool upPressed,
        bool downPressed,
        bool leftPressed,
        bool rightPressed)
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
            floor.shipMoved(gridPosition);
        }
    }
}
