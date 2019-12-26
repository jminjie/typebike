using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer1 : Racer
{
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake(5,6);
        playerNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update(
            Input.GetKeyDown(KeyCode.UpArrow),
            Input.GetKeyDown(KeyCode.DownArrow),
            Input.GetKeyDown(KeyCode.LeftArrow),
            Input.GetKeyDown(KeyCode.RightArrow));
    }
}
