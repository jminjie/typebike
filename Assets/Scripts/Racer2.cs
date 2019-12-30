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
            Input.GetKeyDown(KeyCode.UpArrow),
            Input.GetKeyDown(KeyCode.DownArrow),
            Input.GetKeyDown(KeyCode.LeftArrow),
            Input.GetKeyDown(KeyCode.RightArrow),
            /*wallKey=*/Input.GetKeyDown(KeyCode.Return),
            /*submitKey=*/Input.GetKeyDown(KeyCode.Quote));
    }
}
