using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall
{
    public GameObject wallGameObject;

    public Vector2 startPos;
    public Vector2 endPos;
    public int direction;

    public Wall(Racer racer, int direction)
    {
        this.startPos = racer.BackOfRacer(direction);
        this.endPos = racer.BackOfRacer(direction);
        this.direction = direction;
        Debug.Log("create primitive called");
        wallGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = racer.GetColor();
        var renderer = wallGameObject.GetComponent<MeshRenderer>();
        renderer.material = material;
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
    private Racer otherRacer;
    public Floor floor;
    public GameHandler gameHandler;

    private WordSubmitter wordSubmitter;

    protected int playerNum;
    private Color color;

    public void AwakeBase(int x, int y, string otherRacerString, Color color)
    {
        gridPosition = new Vector2(x, y);
        moveTimerMax = 1f/120f;
        moveTimer = moveTimerMax;
        floor = GameObject.Find("Floor").GetComponent<Floor>();
        walls = new List<Wall>();
        gameHandler = GameObject.Find("GameObject").GetComponent<GameHandler>();
        otherRacer = GameObject.Find(otherRacerString).GetComponent<Racer>();
        wordSubmitter = new WordSubmitter();
        this.color = color;
    }

    public Vector2 BackOfRacer(int direction)
    {
        if (direction == UP)
        {
            return new Vector2(gridPosition.x, gridPosition.y - 0.5f);
        }
        if (direction == DOWN)
        {
            return new Vector2(gridPosition.x, gridPosition.y + 0.5f);
        }
        if (direction == LEFT)
        {
            return new Vector2(gridPosition.x + 0.5f, gridPosition.y);
        }
        if (direction == RIGHT)
        {
            return new Vector2(gridPosition.x - 0.5f, gridPosition.y);
        }
        return new Vector2(gridPosition.x, gridPosition.y);
    }

    public Vector2 getPosition()
    {
        return gridPosition;
    }

    public Color GetColor() => color;

    public void eatLetter(Letter l)
    {
        wordSubmitter.addLetter(l.getValueAndDestroy());
        gameHandler.updateEatenLetters(playerNum, wordSubmitter.getWord()) ;
    }
    public List<Wall> GetWalls() => walls;

    bool CollidesWithWall(Wall wall)
    {
        float frameChange = 0.5f;
        float epsilon = 0.001f;
        float bufferCheck = frameChange - epsilon;
        if (wall.direction == UP)
        {
            if (gridPosition.y > wall.startPos.y - bufferCheck && gridPosition.y < wall.endPos.y + bufferCheck)
            {
                return System.Math.Abs(gridPosition.x - wall.startPos.x) < 1.0f;
            }
            return false;
        }
        if (wall.direction == DOWN)
        {
            if (gridPosition.y < wall.startPos.y + bufferCheck && gridPosition.y > wall.endPos.y - bufferCheck)
            {
                return System.Math.Abs(gridPosition.x - wall.startPos.x) < 1.0f;
            }
            return false;
        }
        if (wall.direction == LEFT)
        {
            if (gridPosition.x < wall.startPos.x + bufferCheck && gridPosition.x > wall.endPos.x-bufferCheck)
            {
                return System.Math.Abs(gridPosition.y - wall.startPos.y) < 1.0f;
            }
            return false;
        }
        if (wall.direction == RIGHT)
        {
            if (gridPosition.x > wall.startPos.x - bufferCheck && gridPosition.x < wall.endPos.x+bufferCheck)
            {
                return System.Math.Abs(gridPosition.y - wall.startPos.y) < 1.0f;
            }
            return false;
        }
        return false;
    }
    private void CheckCollisionsWithWalls()
    {
        foreach (Wall wall in GetWalls())
        {

            if (CollidesWithWall(wall))
            {
                Debug.Log("wall start " + wall.startPos.x + "," + wall.startPos.y);
                Debug.Log("wall end " + wall.endPos.x + "," + wall.endPos.y);
                Debug.Log("racer pos " + gridPosition.x + "," + gridPosition.y);
                Destroy(gameObject);
            }
        }
        foreach (Wall wall in otherRacer.GetWalls())
        {
            if (CollidesWithWall(wall))
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    public void UpdateBase(
        bool upPressed,
        bool downPressed,
        bool leftPressed,
        bool rightPressed,
        bool wallKeyPressed)
    {
        if (upPressed && currentDirection != DOWN)
        {
            currentDirection = UP;
        }
        else if (downPressed && currentDirection != UP)
        {
            currentDirection = DOWN;
        }
        else if (leftPressed && currentDirection != RIGHT)
        {
            currentDirection = LEFT;
        }
        else if (rightPressed && currentDirection != LEFT)
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
            // Update own wall
            HandleWall(wallKeyPressed, currentDirection);

            // Check collisions with own or other racers walls
            CheckCollisionsWithWalls();
        }
    }



    private void StartWall()
    {
        currentWall = new Wall(this, currentDirection);
    }

    private void EndWall()
    {
        // Need to use wall direction here instead of racer direction because
        // racer may have already changed directions
        currentWall.endPos = BackOfRacer(currentWall.direction);
        walls.Add(currentWall);

    }

    private void UpdateWall()
    {
        currentWall.endPos = BackOfRacer(currentWall.direction);
        Debug.Log("updating wall");
        Vector2 diff = currentWall.endPos - currentWall.startPos;
        Vector2 avg = (currentWall.endPos + currentWall.startPos) / 2;
        if (currentWall.direction == UP || currentWall.direction == DOWN)
        {
            currentWall.wallGameObject.transform.localScale = new Vector3(
                1,
                System.Math.Abs(diff.y),
                1);
        } else
        {
            currentWall.wallGameObject.transform.localScale = new Vector3(
                System.Math.Abs(diff.x),
                1,
                1);

        }
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
            StartWall();
            return;
        }
        if (currentlyWalling && !wallKeyPressed)
        {
            currentlyWalling = false;
            Debug.Log("ending because wall key not pressed");
            EndWall();
            return;
        }
        if (currentlyWalling && wallKeyPressed)
        {
            Debug.Log("currently walling and holding wall button");
            if (curDirection != currentWall.direction)
            {
                Debug.Log("ending because changed direction");
                EndWall();
                StartWall();
                return;
            } else
            {
                UpdateWall();
                return;
            }
        }
    }
}
