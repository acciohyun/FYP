using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.IO;

public class ObjectsToObserve : MonoBehaviour
{
    // COLLECTION OF DATA 
    public List<GameObject> objects;
    GameObject[] objectsArr;
    GameObject indivObject;
    private Vector3 position;
    private Quaternion rotation;
    public LinkedList<Node> myNodes = new LinkedList<Node>();
    public Node newNode;
    public LinkedListNode<Node> replayNode;
    public Vector3 currentPos;
    bool ifReplay;
    bool ifImport;
    bool triggered;
    //int counter = 0;
    public Frame frame;
    public string frameStr;
    public Obj objData;
    public string replayFrame;

    // DATA SAVING TO JSON FILE
    public string directName;
    public string fileName ;
    //StreamWriter file = new StreamWriter(fileName, true);
    int counter;
    int counter2;
    public List<Obj> savedObjList; 
    // RETRIEVING DATA FROM JSON FILE
    public string importDirectory;
    public string importFile;
    int importCounter;
    private string importStr;
    private Frame importFrame;

    void Awake()
    {
        objData =  new Obj();
        objects = new List<GameObject>();
        ifReplay = false;
        ifImport = false;
        triggered = false;
        directName = Directory.GetCurrentDirectory() + "/DataTrial4";
        importDirectory = Directory.GetCurrentDirectory() + "/DataTrial4";
        counter2 = 0;
        //fileName = directName + "/JSONtest1.json";
        //GameObject[] objectsArr = UnityEngine.Object.FindObjectsOfType<GameObject>();

        //To check if the problem is due to too may gameobjects, i tried only recording 2 objects (with tag "TANKS")
        //To try, just uncomment the next line and comment the first line.
        GameObject[] objectsArr = GameObject.FindGameObjectsWithTag("TANKS");

        foreach (GameObject go in objectsArr) // To convert the GameObject Array into a GameObject List 
        {
            if (go.isStatic == false)
            {
                objects.Add(go);
                
            }
        }
        savedObjList = new List<Obj>(new Obj [objects.Count]);

        
        Debug.Log("COUNT: " + savedObjList.Count);

        // CHECK CSV FILE / CREATE CSV FILE
        if (Directory.Exists(directName))
        {
            Debug.Log("File Exists");
            
        }
        else
        {
            Debug.Log("File does not exist");
            Directory.CreateDirectory(directName);

        }
        counter = 0;
        importCounter = 0;
    }


    void Update()
    {
        frame = new Frame();
        
        // COLLECTION OF DATA
        if (!ifReplay && !ifImport)
        {
            foreach (GameObject o in objects)
            {
                //Leave node option so that user can choose to not save data, maybe only within the game? 
                //position = obj.transform.position;
                //rotation = obj.transform.rotation; 
                //newNode = new Node(position, rotation, obj, 0);
                //myNodes.AddLast(newNode);

                //Add Obj data
                objData.objID = o.transform;
                objData.objName = o.name;
                objData.position = o.transform.position;
                objData.rotation = o.transform.rotation;

                
                //Add Obj data to list 
                frame.obj.Add(objData);
                counter2++;

                //Required as Class Obj is called by Reference
                objData = new Obj();
            }
            //Save the frame data into JSON
            counter2 = 0;
            fileName = directName + "/JSONtest" + counter.ToString() + ".json";
            frameStr = JsonUtility.ToJson(frame); //trouble converting lists
            Debug.Log("The STRING: " + frameStr);
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
        if (ifImport)
        {
            // Add the scripts to start download here
            //triggered?
            if (triggered)
            {
                //Check if the directory exists
                if (Directory.Exists(importDirectory))
                {
                    Debug.Log("IMPORT: File Exists");
                }
                else
                {
                    Debug.Log("IMPORT: File does not exist");
                    ifImport = false;
                }
                triggered = false;

            }
            importFile = importDirectory + "/JSONtest" + importCounter.ToString() + ".json";
            Debug.Log("File being read: " + importFile);
            //later change "/JSONtest" to input from user
            importStr = File.ReadAllText(importFile);
            importFrame = JsonUtility.FromJson<Frame>(importStr);

            foreach (Obj obj in importFrame.obj)
            {
                obj.objID.transform.position = obj.position;
                Debug.Log("Current Pos: " + obj.objID.transform.position);
                obj.objID.transform.rotation = obj.rotation;
                Debug.Log("Current Rot: " + obj.objID.transform.rotation);
            }
            importCounter++;
            if (Directory.Exists(importDirectory + "/JSONtest" + importCounter.ToString() + ".json"))
            {

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

    public void startImport(bool a)
    {
        if (a)
        {
            ifImport = true;
            Debug.Log("Import start");
        }

    }
    public void stopImport(bool a)
    {
        if (!a)
        {
            ifImport = false;
            Debug.Log("Import stop");
        }
    }
}
