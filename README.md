# FYP

## Premise
This project is an example project to implement a streamlined data collection.
This project was created for use in VR educational games for the purpose of stealth assessment. 
After identifying that storing data efficiently (in terms of storage space and time taken) is crucial in stealth assessment, a plug-in was created to support this process

## How to Use 
### Scripts
1. ObjectToObserve - This script contains the bulk of the data collection and replaying code
2. ToggleImport - This script contains the script that supports the UIUX element to trigger replay 
3. SaveData - This script contains the data structure of the JSON structure of the data collected

### Steps to Start Tracking Data
1. Download the 3 scripts mentioned in the previous section (Assets > Stealth Assessment)
2. Add ObjectsToObserve Script as a GameObject into game environment
3. In Inspecter under ObjectsToObserve, populate the field "Tag To Observe" with the tag of objects that you want to record
4. In Inspecter under ObjectsToObserve, populate the field "Enter Folder Name" with the new folder name where the data collected will be exported to
5. Create a toggle within the game environment and attach the ToggleImport Script to this object. Don't forget to populate the Toggle field in the script with the Toggle used
