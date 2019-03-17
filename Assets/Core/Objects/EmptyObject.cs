using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class EmptyObject : MonoBehaviour {

    public TileTypes ObjectType;
    private Vector3 position = new Vector3Int(0, 0, 0);
    private int rotation = 0;
    private bool backLayer = false;

    public Vector3 Position
    {
        get { return position; }
        set
        {
            position = value;
            transform.position = position;
        }
    }

    public int Rotation
    {
        get { return rotation; }
        set
        {
            rotation = value;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }

    public bool BackLayer
    {
        get { return backLayer; }
        set
        {
            backLayer = value;
            if (backLayer)
            {
                GetComponent<PolygonCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().color = new Color(0.45f, 0.45f, 0.45f, 1);
                GetComponent<SpriteRenderer>().sortingOrder -= 5;
            }
            else
            {
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                GetComponent<SpriteRenderer>().sortingOrder += 5;
            }
        }
    }
}
