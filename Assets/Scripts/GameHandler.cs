using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    private string player1Letters;
    private string player2Letters;
    private int player1Points;
    private int player2Points;

    private int player1Wins;
    private int player2Wins;

    private Color bestWordPlayerColor;
    private string bestWord;
    private int bestWordPoints;

    private bool gameIsOver;

    private const int MAX_POINTS = 30;

    private TextMesh bestWordUI;
    private TextMesh scoreUI;
    private TextMesh p1WordUI;
    private TextMesh p2WordUI;
    private TextMesh winTextUI;

    private Floor floor;
    private GameObject racers;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start called");
        WordSubmitter.initDict();

        bestWordUI = transform.GetChild(0).GetComponent<TextMesh>();
        scoreUI = transform.GetChild(1).GetComponent<TextMesh>();
        p1WordUI = transform.GetChild(2).GetComponent<TextMesh>();
        p2WordUI = transform.GetChild(3).GetComponent<TextMesh>();
        winTextUI = transform.GetChild(4).GetComponent<TextMesh>();
        floor = GameObject.Find("Floor").GetComponent<Floor>();
        racers = GameObject.Find("Racers");

        reset();
        setUIPositions();
        updateGameUI();
    }

    private void setUIPositions()
    {
        bestWordUI.transform.position = new Vector3(76, 106);
        transform.GetChild(1).transform.position = new Vector3(0, 106); // score
        transform.GetChild(2).transform.position = new Vector3(0, -1); //p1word
        transform.GetChild(3).transform.position = new Vector3(103, -1); //p2word
        winTextUI.transform.position = new Vector3(75, 60);
    }

    private void reset()
    {
        bestWord = "";
        bestWordPlayerColor = Color.white;
        bestWordPoints = 0;
        winTextUI.text = "";

        gameIsOver = false;
        player1Letters = "";
        player2Letters = "";
        player1Points = 0;
        player2Points = 0;
    }

    public void setBestWord(Color playerColor, string word, int points)
    {
        bestWordPlayerColor = playerColor;
        Debug.Log("Setting best word player color = " + playerColor);
        bestWord = word;
        bestWordPoints = points;
        updateGameUI();
    }

    public void updateEatenLetters(int playerNum, string letters)
    {
        if (playerNum == 1) {
            player1Letters = letters;
        } else
        {
            player2Letters = letters;
        }
        updateGameUI();
    }

    public void addPoints(int playerNum, int points)
    {
        if (playerNum == 1) {
            player1Points += points;
        } else
        {
            player2Points += points;
        }
        checkWin();
        updateGameUI();
    }

    public int getPoints(int playerNum)
    {
        if (playerNum == 1)
        {
            return player1Points;
        } else
        {
            return player2Points;
        }
    }

    private void setWin(int playerNum)
    {
        if (gameIsOver)
        {
            return;
        }
        if (playerNum == 1)
        {
            player1Wins += 1;
        } else
        {
            player2Wins += 1;
        }
        winTextUI.text = "PLAYER " + playerNum + " WINS\nPress 'r' to restart\nPress 'm' for menu";
        scoreUI.text = "SCORE: " + player1Wins + " - " + player2Wins;
        gameIsOver = true;
    }

    public void racerDied(int playerWhoDied)
    {
        if (playerWhoDied == 1)
        {
            setWin(2);
        } else
        {
            setWin(1);
        }
        gameIsOver = true;
    }

    private void checkWin()
    {
        if (player1Points >= MAX_POINTS)
        {
            setWin(1);
            gameIsOver = true;
        } else if (player2Points >= MAX_POINTS)
        {
            setWin(2);
            gameIsOver = true;
        }
    }

    private void updateGameUI()
    {
        if (gameIsOver)
        {
            return;
        }
        bestWordUI.text = "BEST WORD: " + bestWord + " (" + bestWordPoints + ")";
        bestWordUI.color = bestWordPlayerColor;
        p1WordUI.text = "PLAYER 1: " + player1Letters;
        p2WordUI.text = "PLAYER 2: " + player2Letters;
    }

    private void ResetWins()
    {
        player1Wins = 0;
        player2Wins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver && Input.GetKeyDown(KeyCode.R))
        {
            reset();
            floor.reset();
            racers.transform.GetChild(0).GetComponent<Racer>().respawn();
            racers.transform.GetChild(1).GetComponent<Racer>().respawn();
        }
        if (gameIsOver && Input.GetKeyDown(KeyCode.M))
        {
            ResetWins();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
