using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Alpha2)) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerVsPlayerScene");
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("PlayerVsAIScene");
		}
	}
}
