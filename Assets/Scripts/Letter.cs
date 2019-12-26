using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter
{
    private Vector2 position;
    private GameObject letterGameObject;

    public Letter(Vector2 pos)
    {
        position = pos;
        letterGameObject = new GameObject("Letter A", typeof(Text));
        letterGameObject.GetComponent<Text>().text = "A";
        letterGameObject.transform.position = new Vector3(pos.x, pos.y);
    }

    public Vector2 getPosition()
    {
        return position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void destroy()
    {
        Debug.Log("DESTROYING LETTER A at position=" + position);
        GameObject.Destroy(letterGameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
