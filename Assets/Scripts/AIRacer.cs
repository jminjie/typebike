using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRacer : Racer
{
    // Visible Game State that AI needs 


    // Start is called before the first frame update
    void Awake()
    {
		base.AwakeBase(
			/*x=*/Floor.WIDTH / 3 * 2,
			/*y=*/Floor.HEIGHT / 3,
			/*otherRacerString=*/"Racer1",
			new Color(255, 0, 221));
		playerNum = 2;
	}

    // Update is called once per frame
    void Update()
    {
        // try moving randomly
    }
}
