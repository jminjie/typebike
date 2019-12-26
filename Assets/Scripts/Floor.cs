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

    private const int MAX_LETTERS = 20;

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
        if (letters.Count > MAX_LETTERS)
        {
            Debug.Log("Skip spawning letter");
            return;
        }
        Debug.Log("Spawning letter");

        Letter letter = new Letter(width, height);
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

    public void shipMoved(Racer racer)
    {
        Vector2 position = racer.getPosition();
        // check if ship's position is on the outer rim (destroy ship)
        // check if ship's position is on a letter
        Letter removed = null;
        foreach (Letter l in letters)
        {
            if (withinRange(l.getPosition(), position))
            {
                racer.eatLetter(l);
                removed = l;
            }
        }
        if (removed != null)
        {
            letters.Remove(removed);
        }
    }

    private bool withinRange(Vector2 me, Vector2 them)
    {
        return System.Math.Abs(me.x - them.x) < 2 && System.Math.Abs(me.y - them.y) < 2;
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
