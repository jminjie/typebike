using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{

    private float spawnTime;
    private float maxSpawnTime;

    private List<Letter> letters;

    private int width;
    private int height;

    void Start()
    {
        Debug.Log("Constructor for floor");
        width = 100;
        height = 100;
        letters = new List<Letter>();
        maxSpawnTime = 2f;
        spawnTime = maxSpawnTime;
    }

    private void spawnLetter()
    {
        Debug.Log("Spawning letter");
        Vector2 letterPosition = new Vector2(Random.Range(0, width), Random.Range(0, height));

        Letter letter = new Letter(letterPosition);
        if (letters == null )
        {
            Debug.Log("letters is null");
        }
        if (letter == null)
        {
            Debug.Log("letter is null");
        }
        letters.Add(letter);
    }

    public void shipMoved(Vector2 position)
    {
        // check if ship's position is on the outer rim (destroy ship)
        // check if ship's position is on a letter
        foreach (Letter l in letters)
        {
            if (l.getPosition() == position)
            {
                // ship.consumeLetter(l);
                l.destroy();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= maxSpawnTime)
        {
            spawnLetter();
            spawnTime = 0;
        }
    }
}
