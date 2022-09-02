using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //[SerializeField] private Frame _Frame = new Frame();

    //public void SaveIntoJson(string path, [SerializeField] Frame _frame)
    //{
    //    string frame = JsonUtility.ToJson(_frame);
    //    System.IO.File.AppendAllText(path, frame);
    //}
}

[System.Serializable]
public class Frame
{
    public string impEvent;
    public List<Obj> obj = new List<Obj>();
}

[System.Serializable]
public class Obj
{
    public GameObject objID;
    //somehow understand the gameobject name for human reading 
    public Vector3 position;
    public Quaternion rotation;
}