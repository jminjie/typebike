using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

    private Floor floor;

    private string player1Letters;
    private string player2Letters;
    // Start is called before the first frame update
    void Start()
    {
        player1Letters = "PLAYER 1:";
        player1Letters = "PLAYER 2:";
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

    private void updateGameInfo()
    {
        TextMesh mesh = GetComponent<TextMesh>();
        mesh.text = player1Letters + "\n" + player2Letters;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
