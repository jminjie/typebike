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
    private string bestWord;
    private int bestWordPoints;

    private bool gameIsOver;

    private const int MAX_POINTS = 30;

    // Start is called before the first frame update
    void Start()
    {
        gameIsOver = false;
        player1Letters = "";
        player2Letters = "";
        player1Points = 0;
        player2Points = 0;
        WordSubmitter.initDict();
        resetBestWord();
        bestWord = "";
        TextMesh winTextUI = transform.GetChild(4).GetComponent<TextMesh>();
        winTextUI.text = "";

        updateGameInfo();
    }

    private void resetBestWord()
    {
        bestWord = "";
        bestWordPoints = 0;
    }

    public void setBestWord(string word, int points)
    {
        bestWord = word;
        bestWordPoints = points;
        updateGameInfo();
    }

    public void updateEatenLetters(int playerNum, string letters)
    {
        if (playerNum == 1) {
            player1Letters = letters;
        } else
        {
            player2Letters = letters;
        }
        updateGameInfo();
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
        updateGameInfo();
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
        TextMesh winTextUI = transform.GetChild(4).GetComponent<TextMesh>();
        winTextUI.text = "PLAYER " + playerNum + " WINS\nPress 'r' to restart";
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
        if (player1Points > MAX_POINTS)
        {
            setWin(1);
            gameIsOver = true;
        } else if (player2Points > MAX_POINTS)
        {
            setWin(2);
            gameIsOver = true;
        }
    }

    private void updateGameInfo()
    {
        if (gameIsOver)
        {
            return;
        }
        TextMesh bestWordUI = transform.GetChild(0).GetComponent<TextMesh>();
        TextMesh scoreUI = transform.GetChild(1).GetComponent<TextMesh>();
        TextMesh p1WordUI = transform.GetChild(2).GetComponent<TextMesh>();
        TextMesh p2WordUI = transform.GetChild(3).GetComponent<TextMesh>();
        bestWordUI.text = "BEST WORD: " + bestWord + " (" + bestWordPoints + ")";
        scoreUI.text = "SCORE: : " + player1Points + " - " + player2Points;
        p1WordUI.text = "PLAYER 1: " + player1Letters;
        p2WordUI.text = "PLAYER 2: " + player2Letters;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }
    }
}
