using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WorldState
{
    public WorldState(Racer me, Racer opponent, Floor floor)
    {
        this.me = me;
        this.opponent = opponent;
        this.floor = floor;
    }
    public Racer me;
    public Racer opponent;
    public Floor floor;
}

public class AIRacer : Racer
{
	// Visible Game State that AI needs 
	private float moveTimer;

    // current direction
	private int _direction;
    private WorldState worldState;

    Vector2 ProjectForward(Vector2 curPos, int direction, float projectionDistance)
    {
        if (direction == UP)
        {
            return new Vector2(curPos.x, curPos.y + projectionDistance);
        }
        if (direction == DOWN)
        {
            return new Vector2(curPos.x, curPos.y - projectionDistance);
        }
        if (direction == LEFT)
        {
            return new Vector2(curPos.x - projectionDistance, curPos.y);
        }
        if (direction == RIGHT)
        {
            return new Vector2(curPos.x + projectionDistance, curPos.y);
        }
        Assert.IsTrue(false, "should not be here");
        return new Vector2(curPos.x, curPos.y);
    }

    int OppositeDir(int direction)
    {
        if (direction == UP)
        {
            return DOWN;
        }
        if (direction == DOWN)
        {
            return UP;
        }
        if (direction == LEFT)
        {
            return RIGHT;
        }
        if (direction == RIGHT)
        {
            return LEFT;
        }
        return -1;
    }

    int Clockwise(int direction)
    {
        if (direction == UP)
        {
            return RIGHT;
        }
        if (direction == RIGHT)
        {
            return DOWN;
        }
        if (direction == DOWN)
        {
            return LEFT;
        }
        if (direction == LEFT)
        {
            return UP;
        }
        return -1;
    }

    int Counterclockwise(int direction)
    {
        if (direction == UP)
        {
            return LEFT;
        }
        if (direction == LEFT)
        {
            return DOWN;
        }
        if (direction == DOWN)
        {
            return RIGHT;
        }
        if (direction == RIGHT)
        {
            return UP;
        }
        return -1;
    }
    void TurnAwayFromDirection(int direction)
    {
        Debug.Log("Turning away from direction " + direction);
        if (_direction != direction)
        {
            _direction = OppositeDir(_direction);
            Debug.Log("Choosing new direction " + _direction);
            return;
        }
        // Try turning clockwise
        (bool wouldBeOobIfClockwiseTurn, _) = Floor.OutOfBounds(ProjectForward(_gridPosition, Clockwise(_direction), 3.0f));
        if (!wouldBeOobIfClockwiseTurn)
        {
            _direction = Clockwise(_direction);
            Debug.Log("Choosing new direction " + _direction);
            return;
        }
        _direction = Counterclockwise(_direction);
        Debug.Log("Choosing new direction " + _direction);
    }

    bool CollisionAvoidance()
    {
        (bool wouldBeOob, int oobDirection) = Floor.OutOfBounds(ProjectForward(_gridPosition, _direction, 3.0f));
        if (wouldBeOob) 
        {
            Debug.Log("AI detecting collision, attempting to turn");
            // Check the oob code and redirect accordingly
            TurnAwayFromDirection(oobDirection);            
            return true;
        }
        return false;
    }

    // Returns true if action selected
    bool SelectDirection()
    {
        moveTimer += Time.deltaTime;
        bool avoiding = CollisionAvoidance();
        if (avoiding)
        {
            return true;
        }
        if (moveTimer < 0.3f)
        {
            return false;
        }        
        moveTimer = 0.0f;
        Debug.Log("Choosing random direction");
        // Try turning cw/ccw
        int coinFlip = Random.Range(0, 3);
        if (coinFlip == 0)
        {
            (bool wouldBeOobIfClockwiseTurn, _) = Floor.OutOfBounds(ProjectForward(_gridPosition, Clockwise(_direction), 3.0f));
            if (!wouldBeOobIfClockwiseTurn)
            {
                _direction = Clockwise(_direction);
                Debug.Log("Choosing new direction " + _direction);
            }
            else
            {
                _direction = Counterclockwise(_direction);
                Debug.Log("Choosing new direction " + _direction);
            }
        } else if (coinFlip == 1)
        {
            (bool wouldBeOobIfCcwTurn, _) = Floor.OutOfBounds(ProjectForward(_gridPosition, Counterclockwise(_direction), 3.0f));
            if (!wouldBeOobIfCcwTurn)
            {
                _direction = Counterclockwise(_direction);
                Debug.Log("Choosing new direction " + _direction);
            }
            else
            {
                _direction = Clockwise(_direction);
                Debug.Log("Choosing new direction " + _direction);
            }
        }
        // do nothing if coinFlip returned 2
        return true;
    }
    void Move(bool changed) {
        if (!changed)
        {
            UpdateBase(false, false, false, false, false, false);
            return;
        }
		bool moveUp = _direction == UP;
		bool moveDown = _direction == DOWN;
		bool moveLeft = _direction == LEFT;
		bool moveRight = _direction == RIGHT;
		UpdateBase(moveUp, moveDown, moveLeft, moveRight, false, false);
	}
	// Start is called before the first frame update
	void Awake()
    {
		base.AwakeBase(
			/*x=*/Floor.WIDTH / 3 * 2,
			/*y=*/Floor.HEIGHT / 3,
			/*otherRacerString=*/"Racer1",
			new Color(255, 0, 221));
		playerNum = 2;
        worldState = new WorldState(gameObject.GetComponent<Racer>(), otherRacer, floor);
		powerBar = GameObject.Find("Bar2").GetComponent<PowerBar>();
	}

    // Update is called once per frame
    void Update()
    {

        bool changed = SelectDirection();
		Move(changed);
	}
}
