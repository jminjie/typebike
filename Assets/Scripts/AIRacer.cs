using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AIRacer : Racer
{
	// Visible Game State that AI needs 
	private float moveTimer;
	private int direction;

	int ChooseNewDirection()
	{
		return Random.Range(0, 3);
	}

	void Move(int direction) {
		bool moveUp = direction == UP;
		bool moveDown = direction == DOWN;
		bool moveLeft = direction == LEFT;
		bool moveRight = direction == RIGHT;
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
		powerBar = GameObject.Find("Bar2").GetComponent<PowerBar>();
	}

    // Update is called once per frame
    void Update()
    {
		// try moving randomly
        
		moveTimer += Time.deltaTime;
        if (moveTimer >= 0.3f)
		{
			direction = ChooseNewDirection();
			Debug.Log("choosing new direction" + direction);
			moveTimer = 0.0f;
		}
		Move(direction);
	}
}
