using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

public class ObjectsToObserve : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    GameObject[] objectsArr;
    GameObject indivObject;
    private Vector3 position;
    public LinkedList<Node> myNodes = new LinkedList<Node>();
    public Node newNode;
    public LinkedListNode<Node> replayNode;
    public Vector3 currentPos;
    bool ifReplay = false;
    bool triggered = false;
    int counter = 0;
    void Start()
    {
        GameObject[] objectsArr = UnityEngine.Object.FindObjectsOfType<GameObject>();

        //To check if the problem is due to too may gameobjects, i tried only recording 2 objects (with tag "TANKS")
        //To try, just uncomment the next line and comment the first line.
        //GameObject[] objectsArr = GameObject.FindGameObjectsWithTag("TANKS");

        foreach (GameObject go in objectsArr) // To convert the GameObject Array into a GameObject List 
        {
            if (go.isStatic == false)
            {
                indivObject = go;
                    objects.Add(indivObject);
            }
        }
        Debug.Log("No #1: " + objects.Count);
    }


    void Update()
    {
        if (!ifReplay)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                position = objects[i].transform.position;
                newNode = new Node(position, i);
                myNodes.AddLast(newNode);
                Debug.Log("Record: " + objects[i]);
            }
            

            Debug.Log("The x pos is " + position.y);
        }else if (ifReplay)
        {
            if (triggered)
            {
                Debug.Log("RECORDING STOP");
                replayNode = myNodes.First;
                triggered = false;
                currentPos = new Vector3(0, 0, 0);
            }
            objects[replayNode.Value.objectID].transform.position = replayNode.Value.position;
            Debug.Log("Object " + replayNode.Value.objectID);
            Debug.Log("Y pos " + replayNode.Value.position);
            Debug.Log("Record: " + objects[replayNode.Value.objectID]);
            if (replayNode == myNodes.Last)
            {
                Debug.Log("End of LL");
            }
            else
            {
                replayNode = replayNode.Next;
            }
        }
      
    }
    public void startReplay(bool a)
    {
        if (a)
        {
            triggered = true;
            ifReplay = true;
            Debug.Log("TOGGLE ON");
        }
        
    }
    public void stopReplay(bool a)
    {
        if (!a)
        {
            ifReplay = false;
        }
    }
}
