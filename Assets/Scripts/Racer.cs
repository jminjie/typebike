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
    public const int UP = Floor.UP;
    public const int DOWN = Floor.DOWN;
    public const int LEFT = Floor.LEFT;
    public const int RIGHT = Floor.RIGHT;
    
    // enable walls and boost without points
    // TODO this doenst' work anymore, causes runtime exceptions
    private bool GODMODE = false;

    protected Vector2 _gridPosition;
    private Vector2 startGridPosition;

    private float moveTimer;
    private float moveTimerMax;

    private int[] ButtonCounter;
    private float[] ButtonCooler;

    private const float DOUBLE_TAP_TIME = 0.5f;
    private const float BOOST_DURATION = 0.5f;
    private const float WALL_POINT_DURATION = 1f;

    private float activateBoostTime;
    private float wallingStartTime;

    public Color racerColor;


    private const int START_DIR = UP;
    private int currentDirection = START_DIR;
    private const float ORIGINAL_VELOCITY = 60.0f;
    private float velocity = ORIGINAL_VELOCITY;

    protected bool _currentlyWalling = false;
    protected Wall currentWall;
    protected List<Wall> walls;
    protected List<PointsDisplay> pointsDisplays;
    protected Racer otherRacer;
    public Floor floor;
    public GameHandler gameHandler;

    protected PowerBar _powerBar;
    
    protected WordSubmitter wordSubmitter;

    public int playerNum;
    private Color color;

    private Transform speedLine1;
    private Transform speedLine2;

    public void AwakeBase(int x, int y, string otherRacerString, Color color)
    {

        _gridPosition = new Vector2(x, y);
        startGridPosition = _gridPosition;
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

        speedLine1 = gameObject.transform.GetChild(0);
        speedLine2 = gameObject.transform.GetChild(1);

        SetSpeedLineBrightness(0);
    }

    private void SetSpeedLineBrightness(float brightness)
    {
        // create transparent material
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = new Color(brightness, brightness, brightness);

        // set the speed lines to the transparent material
        Renderer renderer1 = speedLine1.GetComponent<Renderer>();
        renderer1.material = material;
        Renderer renderer2 = speedLine2.GetComponent<Renderer>();
        renderer2.material = material;
    }

    private void clearWalls()
    {
        foreach (Wall w in walls)
        {
            w.destroy();   
        }
        walls.Clear();
        if (currentWall != null) {
            currentWall.destroy();
            currentWall = null;
        }
    }

    private void SetStartPosAndDir()
    {
        _gridPosition = startGridPosition;
        currentDirection = START_DIR;
        setSpeedLines();
    }

    public int GetDirection()
    {
        return currentDirection;
    }

    // called whenever direction changes
    private void setSpeedLines()
    {
        if (currentDirection == UP)
        {
            speedLine1.transform.localPosition = new Vector3(-0.5f, -1f);
            speedLine2.transform.localPosition = new Vector3(0.5f, -1f);
            speedLine1.transform.localScale = new Vector3(0.2f, 3f);
            speedLine2.transform.localScale = new Vector3(0.2f, 3f);
        } else if (currentDirection == DOWN)
        {
            speedLine1.transform.localPosition = new Vector3(0.5f, 1f);
            speedLine2.transform.localPosition = new Vector3(-0.5f, 1f);
            speedLine1.transform.localScale = new Vector3(0.2f, 3f);
            speedLine2.transform.localScale = new Vector3(0.2f, 3f);
        } else if (currentDirection == LEFT)
        {
            speedLine1.transform.localPosition = new Vector3(1f, -0.5f);
            speedLine2.transform.localPosition = new Vector3(1f, 0.5f);
            speedLine1.transform.localScale = new Vector3(3f, 0.2f);
            speedLine2.transform.localScale = new Vector3(3f, 0.2f);
        } else if (currentDirection == RIGHT)
        {
            speedLine1.transform.localPosition = new Vector3(-1f, 0.5f);
            speedLine2.transform.localPosition = new Vector3(-1f, -0.5f);
            speedLine1.transform.localScale = new Vector3(3f, 0.2f);
            speedLine2.transform.localScale = new Vector3(3f, 0.2f);
        }
    }

    public Vector2 BackOfRacer(int direction)
    {
        if (direction == UP)
        {
            return new Vector2(_gridPosition.x, _gridPosition.y - 0.5f);
        }
        if (direction == DOWN)
        {
            return new Vector2(_gridPosition.x, _gridPosition.y + 0.5f);
        }
        if (direction == LEFT)
        {
            return new Vector2(_gridPosition.x + 0.5f, _gridPosition.y);
        }
        if (direction == RIGHT)
        {
            return new Vector2(_gridPosition.x - 0.5f, _gridPosition.y);
        }
        return new Vector2(_gridPosition.x, _gridPosition.y);
    }

    public Vector2 getPosition()
    {
        return _gridPosition;
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

    // Does not include currentWall
    public List<Wall> GetWalls() => walls;

    public Wall GetCurrentWall() => currentWall;

    public bool CollidesWithWall(Vector2 gridPosition, Wall wall)
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

            if (CollidesWithWall(_gridPosition, wall))
            {
                Debug.Log("wall start " + wall.startPos.x + "," + wall.startPos.y);
                Debug.Log("wall end " + wall.endPos.x + "," + wall.endPos.y);
                Debug.Log("racer pos " + _gridPosition.x + "," + _gridPosition.y);
                destroyTheRacer();
            }
        }
        foreach (Wall wall in otherRacer.GetWalls())
        {
            if (CollidesWithWall(_gridPosition, wall))
            {
                destroyTheRacer();
            }
        }
        // TODO: merge this logic with above loop
        if (otherRacer.currentWall != null && CollidesWithWall(_gridPosition, otherRacer.currentWall))
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
        SetSpeedLineBrightness(200);
        gameHandler.addPoints(playerNum, -1);
        _powerBar.removePoints(1);
        SetRacerColor(Color.white);
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
        SetSpeedLineBrightness(0);
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
            setSpeedLines();
        }
        else if (downPressed && currentDirection != UP)
        {
            HandleBoost(DOWN);
            currentDirection = DOWN;
            setSpeedLines();
        }
        else if (leftPressed && currentDirection != RIGHT)
        {
            HandleBoost(LEFT);
            currentDirection = LEFT;
            setSpeedLines();
        }
        else if (rightPressed && currentDirection != LEFT)
        {
            HandleBoost(RIGHT);
            currentDirection = RIGHT;
            setSpeedLines();
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
                _powerBar.addPoints(points);
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
                _gridPosition.y += moveTimer * velocity;
            }
            else if (currentDirection == DOWN)
            {
                _gridPosition.y -= moveTimer * velocity;
            }
            else if (currentDirection == LEFT)
            {
                _gridPosition.x -= moveTimer * velocity;
            }
            else if (currentDirection == RIGHT)
            {
                _gridPosition.x += moveTimer * velocity;
            }
            transform.position = new Vector3(_gridPosition.x, _gridPosition.y);
            moveTimer = 0;
            floor.shipMoved(this);

            // Check collisions with own or other racers walls
            CheckCollisionsWithWalls();
        }


        if (_currentlyWalling && wallingStartTime + WALL_POINT_DURATION < Time.time)
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
                _powerBar.removePoints(1);
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

    public virtual void respawn()
    {
        Debug.Log("Respawning");
        Explode();

        // reset power bar
        _powerBar.reset();

        // reset word
        wordSubmitter.reset();

        // clear walls
        clearWalls();

        // no longer lay wall, no longer dash
        _currentlyWalling = false;
        EndBoost();
        

        // set position and direction
        SetStartPosAndDir();

        // enable
        gameObject.SetActive(true);
    }

    private void StartWall()
    {
        _currentlyWalling = true;
        currentWall = new Wall(this, currentDirection);
    }

    private void EndWall()
    {
        _currentlyWalling = false;
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
        if (!_currentlyWalling && !wallKeyPressed)
        {
            return;
        }
        if (!_currentlyWalling && wallKeyPressed)
        {
            if (!GODMODE && (gameHandler.getPoints(playerNum) < 1)) {
                Debug.Log("not creating wall because player has no points");
                return;
            }
            gameHandler.addPoints(playerNum, -1);
            _powerBar.removePoints(1);
            wallingStartTime = Time.time;
            StartWall();
            return;
        }
        if (_currentlyWalling && wallKeyPressed)
        {
            Debug.Log("ending because wall key not pressed");
            EndWall();
            return;
        }
        if (_currentlyWalling && !wallKeyPressed)
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
