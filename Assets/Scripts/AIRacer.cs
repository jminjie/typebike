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

    // Gonna be a little sloppy here, it's okay since this is for obstacle avoidance
    bool CollidesWithWalls(
        Vector2 futurePos,
        List<Wall> walls)
    {
        foreach (Wall wall in walls)
        {
            if (base.CollidesWithWall(futurePos, wall))
            {
                return true;
            }
        }
        return false;
    }


    private List<float> CollisionAvoidance(
    Vector2 gridPosition,
    List<Wall> allWalls,
    List<int> possibleDirections)
    {
        List<float> votes = new List<float>();
        foreach (int direction in possibleDirections)
        {
            Vector2 futurePos3 = ProjectForward(gridPosition, direction, 3.0f);

            Vector2 futurePos1 = ProjectForward(gridPosition, direction, 1.0f);
            Vector2 futurePos2 = ProjectForward(gridPosition, direction, 2.0f);


            bool collidesWithWalls =
                    CollidesWithWalls(futurePos1, allWalls) ||
                    CollidesWithWalls(futurePos2, allWalls) ||
                    CollidesWithWalls(futurePos3, allWalls);
            Debug.Log("colliding: " + collidesWithWalls);

            (bool wouldBeOob, _) = Floor.OutOfBounds(futurePos3);

            if (wouldBeOob || collidesWithWalls)
            {
                votes.Add(0.0f);
            }
            else
            {
                votes.Add(100.0f);
            }
        }
        Assert.IsTrue(votes.Count == 3);
        return votes;
    }

    float ManhattanDistance(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    List<float> GoalSeeking(
    Vector2 gridPosition,
    List<Letter> letters,
    List<int> possibleDirections)
    {
        List<float> votes = new List<float>();

        // Get closest letter
        if (letters.Count == 0)
        {
            return new List<float>(new float[possibleDirections.Count]);
        }

        float closestLetterDistance = float.MaxValue;
        int closestLetterIdx = -1;
        for (int i = 0; i < letters.Count; ++i)
        {
            float distanceToLetter = ManhattanDistance(gridPosition, letters[i].getPosition());
            if (distanceToLetter < closestLetterDistance)
            {
                closestLetterIdx = i;
                closestLetterDistance = distanceToLetter;
            }
        }

        foreach (int direction in possibleDirections)
        {
            Vector2 futurePos = ProjectForward(gridPosition, direction, 1.0f);
            if (ManhattanDistance(futurePos, letters[closestLetterIdx].getPosition()) < closestLetterDistance)
            {
                votes.Add(10.0f);
            } else
            {
                votes.Add(0.0f);
            }
        }
        Assert.IsTrue(votes.Count == 3);
        return votes;
    }



    List<float> AvoidOtherRacer(
    Vector2 gridPosition,
    Vector2 otherRacerPosition,
    List<int> possibleDirections)
    {
        List<float> votes = new List<float>();
        float currentDistance = ManhattanDistance(gridPosition, otherRacerPosition);
        foreach (int direction in possibleDirections)
        {
            // Add a random number for now
            Vector2 futurePosition = ProjectForward(gridPosition, direction, 1.0f);
            if (ManhattanDistance(futurePosition, otherRacerPosition) >
                currentDistance)
            {
                votes.Add(10.0f);
            } else
            {
                votes.Add(0.0f);
            }
        }
        return votes;
    }


    // Returns true if _direction was changed
    bool SelectDirection()
    {

        List<int> possibleDirections =  new List<int>() {
            _direction,
            Clockwise(_direction),
            Counterclockwise(_direction)};

        List<Wall> allWalls = new List<Wall>();
        allWalls.AddRange(walls);
        allWalls.AddRange(otherRacer.GetWalls());
        if (otherRacer.GetCurrentWall() != null)
        {
            allWalls.Add(otherRacer.GetCurrentWall());
        }
        List<float> collisionAvoidanceVotes = CollisionAvoidance(_gridPosition, allWalls, possibleDirections);
        List<float> seekingGoalVotes = GoalSeeking(_gridPosition, floor.GetLetters(), possibleDirections);
        List<float> avoidingOpponentVotes = AvoidOtherRacer(_gridPosition, otherRacer.getPosition(), possibleDirections);

        Assert.IsTrue(collisionAvoidanceVotes.Count == 3);
        Assert.IsTrue(seekingGoalVotes.Count == 3);
        Assert.IsTrue(avoidingOpponentVotes.Count == 3);

        float bestScore = -1f;
        int bestChoice = -1;
        for (int i =0; i < possibleDirections.Count; ++i)
        {
            float score = collisionAvoidanceVotes[i] + seekingGoalVotes[i] + avoidingOpponentVotes[i];
            if (score > bestScore)
            {               
                bestChoice = i;
                bestScore = score;
            }
        }

        if (possibleDirections[bestChoice] != _direction)
        {
            _direction = possibleDirections[bestChoice];
            return true;
        }
        return false;
    }

    public override void respawn()
    {
        base.respawn();
        _direction = UP;
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
