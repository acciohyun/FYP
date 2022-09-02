using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.IO;

public class ObjectsToObserve : MonoBehaviour
{
    // COLLECTION OF DATA 
    public List<GameObject> objects = new List<GameObject>();
    GameObject[] objectsArr;
    GameObject indivObject;
    private Vector3 position;
    private Quaternion rotation;
    public LinkedList<Node> myNodes = new LinkedList<Node>();
    public Node newNode;
    public LinkedListNode<Node> replayNode;
    public Vector3 currentPos;
    bool ifReplay = false;
    bool triggered = false;
    //int counter = 0;
    public Frame frame;
    public string frameStr;
    public Obj objData = new Obj();
    public string replayFrame;

    // DATA SAVING TO JSON FILE
    public string directName;
    public string fileName ;
    //StreamWriter file = new StreamWriter(fileName, true);
    int counter;


    void Start()
    {
        directName = Directory.GetCurrentDirectory() + "/DataTrial4";
        //fileName = directName + "/JSONtest1.json";
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
        Debug.Log(fileName);

        // CHECK CSV FILE / CREATE CSV FILE
        if (Directory.Exists(directName))
        {
            Debug.Log("File Exists");
            
        }
        else
        {
            Debug.Log("File does not exist");

            Directory.CreateDirectory(directName);
            ////Make the Frame Data to JSON
            //frame.obj.Add(objData);
            //frameStr = JsonUtility.ToJson(frame);
            //File.WriteAllText(fileName, frameStr);

        }
        counter = 0;
    }


    void Update()
    {
        frame = new Frame();
        //counter for every frame (rename the json file JSON1 JSON 2)
        //how it impacts performance then push to github


        //if (File.Exists(fileName))
        //{
        //    Debug.Log("File Exists");
        //    Debug.Log(fileName);

        //}
        //else
        //{
        //    Debug.Log("File does not exist");

        //}
        // COLLECTION OF DATA
        if (!ifReplay)
        {
            foreach (GameObject obj in objects)
            {
                position = obj.transform.position;
                rotation = obj.transform.rotation; 
                newNode = new Node(position, rotation, obj, 0);
                myNodes.AddLast(newNode);
                
                //Add Obj data to JSON
                objData.objID = obj;
                objData.position = obj.transform.position;
                objData.rotation = obj.transform.rotation;

                //Make the Frame Data to JSON
                frame.obj.Add(objData);
            }

            //Save the frame data into JSON
            fileName = directName + "/JSONtest" + counter.ToString() + ".json";
            frameStr = JsonUtility.ToJson(frame);
            File.AppendAllText(fileName, frameStr); //maybe async? //check if the frames are very different 
            counter++;
        }
        else if (ifReplay)
        {
            if (triggered)
            {
                replayNode = myNodes.First;
                triggered = false;
                currentPos = new Vector3(0, 0, 0);
            }
            replayNode.Value.objName.transform.position = replayNode.Value.position;
            replayNode.Value.objName.transform.rotation = replayNode.Value.rotation;

            Debug.Log("Position: " + replayNode.Value.position + " Quat: " + replayNode.Value.rotation);
            Debug.Log("objName" + replayNode.Value.objName);

            if (replayNode == myNodes.Last)
            {
                Debug.Log("End of LL");
                replayFrame = File.ReadAllText(fileName);
                Debug.Log(replayFrame);

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
