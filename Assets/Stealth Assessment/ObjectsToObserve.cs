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
    private LinkedList<Node> myNodes = new LinkedList<Node>();
    private Node newNode;
    private LinkedListNode<Node> replayNode;
    private Vector3 currentPos;
    bool ifReplay;
    bool ifImport;
    bool triggered;

    private Frame frame;
    private string frameStr;
    private Obj objData;
    private string replayFrame;

    [Tooltip("Specify the name of the tag of objects to record. Ensure that all objects you want to record has this tag.")]
    public string tagToObserve;

    // DATA SAVING TO JSON FILE
    private string directName;
    private string fileName ;
    [Tooltip("Name of folder to store data collected. Preferably a new and empty folder.")]
    public string enterFolderName;


    int counter;
    int counter2;
    private List<Obj> savedObjList; 

    // RETRIEVING DATA FROM JSON FILE
    private string importDirectory;
    private string importFile;
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
        directName = Directory.GetCurrentDirectory() + "/"+ enterFolderName;
        importDirectory = Directory.GetCurrentDirectory() + "/" + enterFolderName;
        counter2 = 0;

        GameObject[] objectsArr = GameObject.FindGameObjectsWithTag(tagToObserve);

        foreach (GameObject go in objectsArr) // To convert the GameObject Array into a GameObject List 
        {
            if (go.isStatic == false)
            {
                objects.Add(go);
                
            }
        }
        savedObjList = new List<Obj>(new Obj [objects.Count]);

        
        Debug.Log("COUNT: " + savedObjList.Count);

        
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
            frameStr = JsonUtility.ToJson(frame); 
            Debug.Log("The STRING: " + frameStr);
            File.AppendAllText(fileName, frameStr); 
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
            if (triggered)
            {
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
