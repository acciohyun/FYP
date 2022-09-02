using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 position;
    public GameObject objName;
    public Quaternion rotation;
    public int frame;

    public Node(Vector3 _position, Quaternion _rotation, GameObject _objName, int _frame)
    {
        position = _position;
        rotation = _rotation;
        objName = _objName;
        frame = _frame;
    }
}
