using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSubmitter
{

    private static HashSet<string> dict;

    public static void initDict()
    {
        if (dict == null) {
            TextAsset textAsset = (TextAsset) Resources.Load("dictionary", typeof(TextAsset));
            if (textAsset != null) {
                dict = new HashSet<string>(textAsset.text.Split("\n"[0]));
            } else
            {
                Debug.Log("Could not find dictionary txt file");
            }
        } else
        {
            Debug.Log("should not call initDict twice but you did so");
        }
    }

    public WordSubmitter()
    {
        currentWord = "";
    }

    private string currentWord;

    public void addLetter(string s)
    {
        currentWord += s;
    }

    public string getWord()
    {
        return currentWord;
    }

    private int evaluatePoints()
    {
        // TODO use scrabble logic or something similar
        return currentWord.Length;
    }

    public bool isValidWord()
    {
        if (dict == null) {
            Debug.Log("isValidWord: Dictionary is not yet initialized!");
        }
        return dict.Contains(currentWord.ToUpper());
    }

    // return the amount points that the word gives
    public int submitWord()
    {
        if (isValidWord())
        {
            int points = evaluatePoints();
            currentWord = "";
            return points;
        } else
        {
            Debug.Log("Not a real word: " + currentWord);
            dumpWord();
            return 0;
        }
    }

    private void dumpWord()
    {
        currentWord = "";
    }
}
