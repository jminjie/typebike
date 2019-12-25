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



    private void Awake()
    {
        gridPosition = new Vector2(5, 6);
        moveTimerMax = 1f/60f;
        moveTimer = moveTimerMax;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentDirection = UP;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentDirection = DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentDirection = LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
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
        }
    }
}
