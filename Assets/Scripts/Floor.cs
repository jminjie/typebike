using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Floor(int width,int height)
    {

    }

    private void spawnLetter()
    {
        
    }

    public void shipMoved(Vector2 position)
    {
        // check if ship's position is on the outer rim (destroy ship)
        // check if ship's position is on a letter
        Debug.Log("ship moved to " + position.x + "," + position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
