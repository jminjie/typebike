using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDisplay
{
    private const float TEXT_SIZE = 1f;
    private Color LETTER_COLOR = new Color(255, 255, 255, 255);
    private float createTime;
    private float LIFE_SPAN = 1;
    private GameObject pointsGameObject;
    private int value;

    public PointsDisplay(int points, Vector2 position)
    {
        value = points;
        pointsGameObject = new GameObject("PointsDisplay", typeof(TextMesh));
        pointsGameObject.transform.position = new Vector3(position.x, position.y);
        pointsGameObject.transform.localScale += new Vector3(TEXT_SIZE, TEXT_SIZE);
        TextMesh mesh = pointsGameObject.GetComponent<TextMesh>();
        mesh.text = points.ToString();
        mesh.color = LETTER_COLOR;
        mesh.alignment = TextAlignment.Center;
        mesh.anchor = TextAnchor.MiddleCenter;
        createTime = Time.time;
    }

    // called from Racer
    public void Update()
    {
        Debug.Log("PointsDisplay update called");
        if (Time.time > (createTime + LIFE_SPAN))
        {
            GameObject.Destroy(pointsGameObject);
        } else
        {
            // fade
            Vector3 position = pointsGameObject.transform.position;
            pointsGameObject.transform.position = new Vector3(position.x+0.07f, position.y + 0.07f);
            TextMesh mesh = pointsGameObject.GetComponent<TextMesh>();
            mesh.text = value.ToString();
            mesh.color = new Color(255, 255, 255, mesh.color.a - 0.01f);
        }
    }
}
