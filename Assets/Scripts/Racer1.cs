﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer1 : Racer
{
    // Start is called before the first frame update
    void Awake()
    {
        base.AwakeBase(
            /*x=*/5,
            /*y=*/6,
            /*otherRacerString=*/"Racer2",
            Color.white);
        playerNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateBase(
            Input.GetKeyDown(KeyCode.UpArrow),
            Input.GetKeyDown(KeyCode.DownArrow),
            Input.GetKeyDown(KeyCode.LeftArrow),
            Input.GetKeyDown(KeyCode.RightArrow),
            Input.GetKey(KeyCode.Return));
    }
}
