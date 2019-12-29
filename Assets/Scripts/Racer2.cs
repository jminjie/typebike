using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer2 : Racer
{
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
        base.UpdateBase(
            Input.GetKeyDown(KeyCode.W),
            Input.GetKeyDown(KeyCode.S),
            Input.GetKeyDown(KeyCode.A),
            Input.GetKeyDown(KeyCode.D),
            /*wallKey=*/Input.GetKeyDown(KeyCode.Space),
            /*submitKey=*/Input.GetKeyDown(KeyCode.C));
    }
}
