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

    public void destroy()
    {
        GameObject.Destroy(wallGameObject);
    }
}


public class Racer : MonoBehaviour
{

    // enable walls and boost without points
    private bool GODMODE = false;

    private Vector2 startGridPosition;
    private Vector2 gridPosition;
    private float moveTimer;
    private float moveTimerMax;

    private int[] ButtonCounter;
    private float[] ButtonCooler;

    private const float DOUBLE_TAP_TIME = 0.5f;
    private const float BOOST_DURATION = 0.5f;
    private const float WALL_POINT_DURATION = 1f;

    private float activateBoostTime;
    private float wallingStartTime;

    protected const int UP = 0;
    protected const int DOWN = 1;
    protected const int LEFT = 2;
    protected const int RIGHT = 3;

    private const int START_DIR = UP;
    private int currentDirection = START_DIR;
    private const float ORIGINAL_VELOCITY = 60.0f;
    private float velocity = ORIGINAL_VELOCITY;

    private bool currentlyWalling = false;
    protected Wall currentWall;
    protected List<Wall> walls;
    protected List<PointsDisplay> pointsDisplays;
    protected Racer otherRacer;
    public Floor floor;
    public GameHandler gameHandler;

    protected PowerBar powerBar;
    
    private WordSubmitter wordSubmitter;

    protected int playerNum;
    private Color color;

    public void AwakeBase(int x, int y, string otherRacerString, Color color)
    {
        gridPosition = new Vector2(x, y);
        startGridPosition = gridPosition;
        moveTimerMax = 1f/60f;
        moveTimer = moveTimerMax;
        floor = GameObject.Find("Floor").GetComponent<Floor>();
        walls = new List<Wall>();
        pointsDisplays = new List<PointsDisplay>();
        gameHandler = GameObject.Find("GameObject").GetComponent<GameHandler>();
        otherRacer = GameObject.Find(otherRacerString).GetComponent<Racer>();
        this.color = color;
        wordSubmitter = new WordSubmitter(color);
        ButtonCounter = new int[]{ 0, 0, 0, 0 };
        ButtonCooler = new float[] { DOUBLE_TAP_TIME, DOUBLE_TAP_TIME, DOUBLE_TAP_TIME, DOUBLE_TAP_TIME };
        activateBoostTime = 0f;
    }

    private void clearWalls()
    {
        foreach (Wall w in walls)
        {
            w.destroy();   
        }
        walls.Clear();
    }

    private void SetStartPosAndDir()
    {
        gridPosition = startGridPosition;
        currentDirection = START_DIR;
    }

    private void ClearWalls()
    {
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

    private void SetRacerColor(Color c)
    {
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = c;
        var renderer = GetComponent<MeshRenderer>();
        renderer.material = material;
    }

    public Color GetColor() => color;

    public void EatLetter(Letter l)
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
                destroyTheRacer();
            }
        }
        foreach (Wall wall in otherRacer.GetWalls())
        {
            if (CollidesWithWall(wall))
            {
                destroyTheRacer();
            }
        }
        // TODO: merge this logic with above loop
        if (otherRacer.currentWall != null && CollidesWithWall(otherRacer.currentWall))
        {
            destroyTheRacer();
        }
    }

    public void destroyTheRacer()
    {
        gameHandler.racerDied(playerNum);
        Explode();
    }

    private void Explode()
    {
        gameObject.SetActive(false);
    }

    private void ActivateBoost()
    {
        int points = gameHandler.getPoints(playerNum);
        if (!GODMODE && (points < 1))
        {
            Debug.Log("not enough points");
            return;
        }
        gameHandler.addPoints(playerNum, -1);
        powerBar.removePoints(1);
        SetRacerColor(Color.red);
        activateBoostTime = Time.time;
        velocity = ORIGINAL_VELOCITY * 1.6f;
        if (points < 1)
        {
            EndWall();
        }
    }

    private void EndBoost()
    {
        SetRacerColor(GetColor());
        activateBoostTime = 0f;
        velocity = ORIGINAL_VELOCITY;

    }

    private void HandleBoost(int dir)
    {
        if (dir > 3 || dir < 0)
        {
            Debug.Log("unexpected direction=" + 3);
            return;
        }

        if (ButtonCounter[dir] == 1 && ButtonCooler[dir] > 0)
        {
            ActivateBoost();
        }
        else
        {
            ButtonCooler[dir] = DOUBLE_TAP_TIME;
            ButtonCounter[dir] += 1;

            // clear button counter for all other directions
            for (int i = 0; i < 4; i++)
            {
                if (i == dir)
                {
                    continue;
                }
                ButtonCounter[i] = 0;
                ButtonCooler[i] = DOUBLE_TAP_TIME;
            }
        }

        if (ButtonCooler[dir] > 0)
        {
            ButtonCooler[dir] -= 1 * Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < 4; i++) {
                ButtonCounter[i] = 0;
            }
        }
    }

    // Update is called once per frame
    public void UpdateBase(
        bool upPressed,
        bool downPressed,
        bool leftPressed,
        bool rightPressed,
        bool wallKeyPressed,
        bool submitKeyPressed)
    {
        if (upPressed && currentDirection != DOWN)
        {
            HandleBoost(UP);
            currentDirection = UP;
        }
        else if (downPressed && currentDirection != UP)
        {
            HandleBoost(DOWN);
            currentDirection = DOWN;
        }
        else if (leftPressed && currentDirection != RIGHT)
        {
            HandleBoost(LEFT);
            currentDirection = LEFT;
        }
        else if (rightPressed && currentDirection != LEFT)
        {
            HandleBoost(RIGHT);
            currentDirection = RIGHT;
        }

        if (activateBoostTime != 0f && activateBoostTime + BOOST_DURATION < Time.time)
        {
            EndBoost();
        }
        
        if (submitKeyPressed)
        {
            if (wordSubmitter.getWord() != "")
            {
                int points = wordSubmitter.submitWord();
                ShowPointsFromSubmitting(points);
                gameHandler.addPoints(playerNum, points);
                powerBar.addPoints(points);
                gameHandler.updateEatenLetters(playerNum, "");
            }
        }

        UpdatePointsDisplays();

        // Update own wall
        HandleWall(wallKeyPressed, currentDirection);

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

            // Check collisions with own or other racers walls
            CheckCollisionsWithWalls();
        }


        if (currentlyWalling && wallingStartTime + WALL_POINT_DURATION < Time.time)
        {
            int points = gameHandler.getPoints(playerNum);
            if (points <= 1)
            {
                EndWall();
            }
            wallingStartTime = Time.time;
            if (points > 0)
            {
                gameHandler.addPoints(playerNum, -1);
                powerBar.removePoints(1);
            }
        }
    }

    private void ShowPointsFromSubmitting(int points)
    {
        pointsDisplays.Add(new PointsDisplay(points, getPosition()));
    }

    private void UpdatePointsDisplays()
    {
        foreach (PointsDisplay p in pointsDisplays)
        {
            p.Update();
        }
    }

    public void respawn()
    {
        Debug.Log("Respawning");
        Explode();

        // reset power bar
        powerBar.reset();

        // reset word
        wordSubmitter.reset();

        // clear walls
        clearWalls();

        // set position and direction
        SetStartPosAndDir();

        // enable
        gameObject.SetActive(true);
    }

    private void StartWall()
    {
        currentlyWalling = true;
        currentWall = new Wall(this, currentDirection);
    }

    private void EndWall()
    {
        currentlyWalling = false;
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
            if (!GODMODE && (gameHandler.getPoints(playerNum) < 1)) {
                Debug.Log("not creating wall because player has no points");
                return;
            }
            gameHandler.addPoints(playerNum, -1);
            powerBar.removePoints(1);
            wallingStartTime = Time.time;
            StartWall();
            return;
        }
        if (currentlyWalling && wallKeyPressed)
        {
            Debug.Log("ending because wall key not pressed");
            EndWall();
            return;
        }
        if (currentlyWalling && !wallKeyPressed)
        {
            Debug.Log("currently walling");
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
