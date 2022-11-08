﻿using System.Collections;
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

    [Tooltip("Specify the keyword of objects to record. Note that ALL objects with theis keyword will be recored.")]
    public List<string> keywords;


    //public string tagToObserve;

    // DATA SAVING TO JSON FILE
    private string directName;
    private string fileName ;
    [Tooltip("Name of folder to store data collected. Preferably a new and empty folder.")]
    public string enterFolderName;


    int recordCounter;
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

      

        
        if (Directory.Exists(directName))
        {
            Debug.Log("File Exists");
            
        }
        else
        {
            Debug.Log("File does not exist");
            Directory.CreateDirectory(directName);

        }
        recordCounter = 0;
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
                objData.activeStatus = o.activeSelf; //Save State of gameObject

                //Add Obj data to list 
                frame.obj.Add(objData);

                //Required as Class Obj is called by Reference
                objData = new Obj();
            }
            //Save the frame data into JSON
            fileName = directName + "/JSONtest" + recordCounter.ToString("00000000") + ".json";
            frameStr = JsonUtility.ToJson(frame);
            Debug.Log("The STRING: " + frameStr);
            File.AppendAllText(fileName, frameStr);
            File.WriteAllText(fileName, frameStr);

            recordCounter++;
        }
        if (ifImport&& !recording)
        {
            importFile = importDirectory + "/JSONtest" + importCounter.ToString("00000000") + ".json";
            Debug.Log("File being read: " + importFile);


            if (importCounter != recordCounter)
            {
                Debug.Log("File Exists");
                importStr = File.ReadAllText(importFile);
                importFrame = JsonUtility.FromJson<Frame>(importStr);

                foreach (Obj obj in importFrame.obj)
                {
                    obj.objID.transform.gameObject.SetActive(obj.activeStatus); //Set State of gameObject
                    Debug.Log("SAVED ACTIVE: " + obj.activeStatus);
                    obj.objID.transform.position = obj.position;
                    obj.objID.transform.rotation = obj.rotation;
                }
                importCounter++;

            }
            else
            {
                importCounter = 0;
                ifImport = false;

            }

        }


    }

    //The following function toggles the start of record and stop of record
    public void recordStatus(bool a)
    {
        recording = a;
    }


    // The following function toggles the start of the import
    public void importStatus(bool a)
    {
        ifImport = a;
    }

    // The following function toggles the start of the import
    public void rebeginRecordStatus(bool a)
    {
        recording = a;
        if (a)
        {
            recordCounter = 0;
            importCounter = 0;
        }
        
    }
}
