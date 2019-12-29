using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Floor : MonoBehaviour
{
    // board dimensions
    public const int WIDTH = 150;
    public const int HEIGHT = 100;

    // Used for racer directions as well as OOB checking
    public const int UP = 0;
    public const int DOWN = 1;
    public const int LEFT = 2;
    public const int RIGHT = 3;

    // num letters at once
    private const int MAX_LETTERS = 20;

    // spawn time between letters
    private const float MAX_LETTER_SPAWN_TIME = 2f;

    // range to count a letter collision
    private const float LETTER_COLLISION_RANGE = Letter.TILE_SIZE;

    private List<Letter> letters;
    private float spawnTime;

    void Start()
    {
        Debug.Log("Constructor for floor");
        letters = new List<Letter>();
        spawnTime = MAX_LETTER_SPAWN_TIME;

        for (int i = 0; i < 5; i++)
        {
            spawnLetter();
        }
    }

    private void spawnLetter()
    {
        if (letters.Count > MAX_LETTERS)
        {
            Debug.Log("Skip spawning letter");
            return;
        }

        Letter letter = new Letter(WIDTH, HEIGHT);
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

    static public (bool,int) OutOfBounds(Vector2 position) {
        if (position.y > HEIGHT)
        {
            return (true, UP);
        }
        if (position.y < 0)
        {
            return (true, DOWN);
        }
        if (position.x < 0)
        {
            return (true, LEFT);
        }
        if (position.x > WIDTH)
        {
            return (true, RIGHT);
        }
        return (false, -1);
    }

    public void shipMoved(Racer racer)
    {
        Vector2 position = racer.getPosition();
        // check if ship's position is on the outer rim (destroy ship)
        if (OutOfBounds(position).Item1) {
            racer.destroyTheRacer();
        }


        // check if ship's position is on a letter
        Letter removed = null;
        foreach (Letter l in letters)
        {
            if (WithinRange(l.getPosition(), position))
            {
                racer.EatLetter(l);
                removed = l;
            }
        }

        if (removed != null)
        {
            letters.Remove(removed);
        }
    }

    private bool WithinRange(Vector2 me, Vector2 them)
    {
        return System.Math.Abs(me.x - them.x) < LETTER_COLLISION_RANGE && System.Math.Abs(me.y - them.y) < LETTER_COLLISION_RANGE;
    }

    public void reset()
    {
        foreach (Letter l in letters)
        {
            l.destroy();
        }
        letters.Clear();
        Start();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= MAX_LETTER_SPAWN_TIME)
        {
            spawnLetter();
            spawnTime = 0;
        }
    }
}
