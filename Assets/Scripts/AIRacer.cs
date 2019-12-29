using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	int ChooseNewDirection()
    {
        // try moving randomly
        return Random.Range(0, 3);
	}

    int SelectDirection()
    {

        moveTimer += Time.deltaTime;
        if (moveTimer < 0.3f)
        {
            return _direction;
        }
        
        moveTimer = 0.0f;
        return ChooseNewDirection();
        
    }
void Move() {
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

        _direction = SelectDirection();
		Move();
	}
}
