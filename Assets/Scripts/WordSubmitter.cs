﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSubmitter
{

    private static HashSet<string> dict;
    private static Trie trie;

    private const string ONE_POINT = "AEIOULNSTR";
    private const string TWO_POINT = "DG";
    private const string THREE_POINT = "BCMP";
    private const string FOUR_POINT = "FHVWY";
    private const string FIVE_POINT = "K";
    private const string EIGHT_POINT = "JX";
    private const string TEN_POINT = "QZ";

    private static string highestScoringWord;
    private static int highestScoringWordPoints;
    private static GameHandler gameHandler;

    private Color ownerColor;

    public static void initDict(GameHandler gameHandler)
    {
        if (dict == null) {
            TextAsset textAsset = (TextAsset) Resources.Load("dictionary", typeof(TextAsset));
            if (textAsset != null) {
                dict = new HashSet<string>(textAsset.text.Split("\n"[0]));
                trie = new Trie(dict);
                
            } else
            {
                Debug.Log("Could not find dictionary txt file");
            }
        } else
        {
            Debug.Log("should not call initDict twice but you did so");
        }
        WordSubmitter.gameHandler = gameHandler;

    }

    public HashSet<string> GetDict()
    {
        return dict;
    }

    public Trie GetTrie()
    {
        return trie;
    }

    // used for tests
    public static WordSubmitter GetWordSubmitterForTest()
    {
        return new WordSubmitter(Color.white);
    }

    public WordSubmitter(Color color)
    {
        currentWord = "";
        ownerColor = color;
        highestScoringWord = "";
        highestScoringWordPoints = 0;
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
        // use scrabble logic
        int total = 0;
        foreach (char l in currentWord) {
            string letter = l.ToString();
            if (ONE_POINT.Contains(letter)) {
                total += 1;
            } else if (TWO_POINT.Contains(letter))
            {
                total += 2;
            } else if (THREE_POINT.Contains(letter))
            {
                total += 3;
            } else if (FOUR_POINT.Contains(letter))
            {
                total += 4;
            } else if (FIVE_POINT.Contains(letter))
            {
                total += 5;
            } else if (EIGHT_POINT.Contains(letter))
            {
                total += 8;
            } else if (TEN_POINT.Contains(letter))
            {
                total += 10;
            } else
            {
                Debug.Log("Unexpected letter=" + letter);
            }
        }
        return total;
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
            if (points > highestScoringWordPoints)
            {
                highestScoringWord = currentWord;
                highestScoringWordPoints = points;
                gameHandler.setBestWord(ownerColor, highestScoringWord, points);
            }
            currentWord = "";
            return points;
        } else
        {
            Debug.Log("Not a real word: " + currentWord);
            dumpWord();
            return 0;
        }
    }

    public void reset()
    {
        dumpWord();
    }

    private void dumpWord()
    {
        currentWord = "";
    }
}
