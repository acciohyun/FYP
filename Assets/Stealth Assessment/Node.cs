using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector3 position;
    public int objectID;

    public Node(Vector3 _position, int _objectID)
    {
        position = _position;
        objectID = _objectID;
    }
}
