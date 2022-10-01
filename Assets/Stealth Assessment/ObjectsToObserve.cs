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
    bool ifImport;
    bool recording;

    private Frame frame;
    private string frameStr;
    private Obj objData;
    private string replayFrame;

    [Tooltip("Specify the name of the tag of objects to record. Ensure that all objects you want to record has this tag.")]
    public List<string> tags; 

    //public string tagToObserve;

    // DATA SAVING TO JSON FILE
    private string directName;
    private string fileName ;
    [Tooltip("Name of folder to store data collected. Preferably a new and empty folder.")]
    public string enterFolderName;


    int counter;
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
        ifImport = false;
        recording = false;
        directName = Directory.GetCurrentDirectory() + "/"+ enterFolderName;
        importDirectory = Directory.GetCurrentDirectory() + "/" + enterFolderName;

        foreach (string myTag in tags)
        {
            GameObject[] objectsArr = GameObject.FindGameObjectsWithTag(myTag);
            foreach (GameObject go in objectsArr) // To convert the GameObject Array into a GameObject List 
            {
                if (go.isStatic == false)
                {
                    objects.Add(go);

                }
            }
        }
        savedObjList = new List<Obj>(new Obj [objects.Count]);

        Debug.Log("TRACKING: " + savedObjList.Count ); 
        //Debug.Log("COUNT: " + savedObjList.Count);

        
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
        if (!ifImport&&recording)
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

                //Required as Class Obj is called by Reference
                objData = new Obj();
            }
            //Save the frame data into JSON
            fileName = directName + "/JSONtest" + counter.ToString() + ".json";
            frameStr = JsonUtility.ToJson(frame); 
            Debug.Log("The STRING: " + frameStr);
            File.AppendAllText(fileName, frameStr); 
            counter++;
        }
        if (ifImport&& !recording)
        {
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
        }


    }

    //The following function toggles the start of record and end of record
    public void recordStatus(bool a)
    {
        recording = a;
    }


    // The following function toggles the start of the import
    public void importStatus(bool a)
    {
        ifImport = a;
    }
}
