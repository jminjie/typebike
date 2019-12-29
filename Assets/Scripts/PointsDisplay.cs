using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsDisplay
{
    private const float TEXT_SIZE = 1f;
    private Color letterColor;
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
        if (points == 0) {
            letterColor = Color.red;
        } else if (points < 5)
        {
            letterColor = new Color(255, 255, 255, 1.0f);
        } else
        {
            letterColor = new Color(153, 255, 153, 1.0f);
        }
        mesh.color = letterColor;
        mesh.alignment = TextAlignment.Center;
        mesh.anchor = TextAnchor.MiddleCenter;
        createTime = Time.time;
    }

    // called from Racer
    public void Update()
    {
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
            mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, mesh.color.a - 0.01f);
        }
    }
}
