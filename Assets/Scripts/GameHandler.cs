using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    private Floor floor;

    private string player1Letters;
    private string player2Letters;
    private int player1Points;
    private int player2Points;

    private bool gameIsOver;

    private const int MAX_POINTS = 30;

    // Start is called before the first frame update
    void Start()
    {
        gameIsOver = false;
        player1Letters = "PLAYER 1:";
        player2Letters = "PLAYER 2:";
        player1Points = 0;
        player2Points = 0;
        WordSubmitter.initDict();
    }

    public void updateEatenLetters(int playerNum, string letters)
    {
        if (playerNum == 1) {
            player1Letters = "PLAYER 1: " + letters;
        } else
        {
            player2Letters = "PLAYER 2: " + letters;
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
        TextMesh mesh = GetComponent<TextMesh>();
        mesh.text = "PLAYER " + playerNum + " WINS\n SCORE: " + player1Points + " - " + player2Points + "\nPress 'r' to restart";
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
        TextMesh mesh = GetComponent<TextMesh>();
        mesh.text = player1Letters + "\n" + player2Letters
            + "\n SCORE: " + player1Points + " - " + player2Points;
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
