using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer2 : Racer
{
    // Start is called before the first frame update
    void Awake()
    {
        base.AwakeBase(50, 50);
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateBase(
            Input.GetKeyDown(KeyCode.W),
            Input.GetKeyDown(KeyCode.S),
            Input.GetKeyDown(KeyCode.A),
            Input.GetKeyDown(KeyCode.D),
            Input.GetKey(KeyCode.Space));
    }
}
