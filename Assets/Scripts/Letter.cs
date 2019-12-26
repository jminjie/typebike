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
        Debug.Log("Letter is generated");
        position = pos;
        letterGameObject = new GameObject("Letter A", typeof(TextMesh));
        letterGameObject.transform.position = new Vector3(pos.x, pos.y);
        letterGameObject.transform.localScale += new Vector3(3, 3);
        TextMesh mesh = letterGameObject.GetComponent<TextMesh>();
        mesh.text = "A";
        mesh.alignment = TextAlignment.Center;
        mesh.anchor = TextAnchor.MiddleCenter;

    }

    public Vector2 getPosition()
    {
        return position;
    }

    public void destroy()
    {
        Debug.Log("DESTROYING LETTER A at position=" + position);
        GameObject.Destroy(letterGameObject);
    }
}
