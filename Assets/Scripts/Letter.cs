using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter
{
    private static Color letterColor = Color.black;
    private static Color tileColor = Color.white;
    private const float TEXT_SIZE = 1.2f;
    public const float TILE_SIZE = 2.0f;
    private const string ALPHABET = "AAAAAAAAABBCCDDDDEEEEEEEEEEEEFFGGGHHIIIIIIIIIJKLLLLMMNNNNNNOOOOOOOOPPQRRRRRRRSSSSTTTTTTUUUUVVWXYYZ";

    private Vector2 position;
    private GameObject letterGameObject;
    private GameObject letterFrame;
    private string value;
    private bool isEaten;

    public Letter(int floorWidth, int floorHeight)
    {
        int edgeBuffer = 5;
        Vector2 pos = new Vector2(Random.Range(0+edgeBuffer, floorWidth-edgeBuffer), Random.Range(0+edgeBuffer, floorHeight-edgeBuffer));
        isEaten = false;
        value = "" + ALPHABET[Random.Range(0, ALPHABET.Length)];
        position = pos;
        letterGameObject = new GameObject("Letter " + value, typeof(TextMesh));
        letterGameObject.transform.position = new Vector3(pos.x, pos.y);
        letterGameObject.transform.localScale += new Vector3(TEXT_SIZE, TEXT_SIZE);
        letterFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
        letterFrame.transform.position = new Vector3(pos.x, pos.y);
        letterFrame.transform.localScale += new Vector3(TILE_SIZE, TILE_SIZE);
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = tileColor;
        var renderer = letterFrame.GetComponent<MeshRenderer>();
        renderer.material = material;
        TextMesh mesh = letterGameObject.GetComponent<TextMesh>();
        mesh.text = value;
        mesh.color = letterColor;
        mesh.alignment = TextAlignment.Center;
        mesh.anchor = TextAnchor.MiddleCenter;
    }

    public Vector2 getPosition()
    {
        return position;
    }

    public string getValueAndDestroy()
    {
        if (isEaten)
        {
            return "";
        }
        isEaten = true;
        destroy();
        return value;
    }

    private void destroy()
    {
        GameObject.Destroy(letterGameObject);
        GameObject.Destroy(letterFrame);
    }
}
