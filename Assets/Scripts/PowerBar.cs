using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    private float currentBlockY = 1.8f;

    private float BLOCK_WIDTH = 3f;
    private float BLOCK_HEIGHT = 1.6f;
    private float BLOCK_BUFFER = 1.5f;

    private Color blockColor = Color.white;

    private Stack<GameObject> blocks;

    // Start is called before the first frame update
    void Start()
    {
        blocks = new Stack<GameObject>();
    }

    public void addPoints(int numPoints)
    {
        for (int i = 0; i < numPoints; i++)
        {
            if (blocks.Count == 30)
            {
                return;
            }
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
            block.transform.position = new Vector3(transform.position.x, currentBlockY);
            block.transform.localScale += new Vector3(BLOCK_WIDTH, BLOCK_HEIGHT);
            Material material = new Material(Shader.Find("Unlit/Color"));
            material.color = blockColor;
            var renderer = block.GetComponent<MeshRenderer>();
            renderer.material = material;

            currentBlockY += BLOCK_HEIGHT + BLOCK_BUFFER;
            blocks.Push(block);
        }
    }

    public void removePoints(int numPoints)
    {
        if (blocks.Count < numPoints)
        {
            Debug.Log("Tried to remove too many points in power bar. This shouldn't happen");
            numPoints = blocks.Count;
        }
        for (int i = 0; i < numPoints; i++)
        {
            GameObject oldBlock = blocks.Pop();
            GameObject.Destroy(oldBlock);
            currentBlockY -= BLOCK_HEIGHT + BLOCK_BUFFER;
        }
    }

    public void reset()
    {
        removePoints(blocks.Count);
    }
}
