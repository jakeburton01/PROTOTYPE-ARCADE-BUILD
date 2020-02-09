using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class ChangeScene : MonoBehaviour
{
    public string path;    //Used to define a File Path
    public string tempRead;  //A string that gets wiped after every use
    public string readOut;   //Same as above     (Implementation will be commented at instance)

    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public GameObject TempGameObject;
    public Text HoldText;
    public Text TempText;  //Above UI objects are temporary for my specific test scene, will change dependant on final menu


    public GameObject AIprefab;
    public GameObject PlayerPrefab;
    // Start is called before the first frame update
    void Start()
    {
       
        path = Application.dataPath.ToString(); //Sets the initial datapath to the application/to the game folder
        if(SceneManager.GetActiveScene().name == "SampleScene") //If the current scene is the arena scene, currently named samplescene
        {
            ReadString("");  //If on a specific scene then call the read string function
        }
    }

    // Update is called once per frame
    void Update()
    {
			//Nothing should be placed here, should all be reactionary to user input through UI 
    }

    public void SetWriteValue()
    {  
		//This function will be called onButtonPress 
	
        HoldText.text = HoldText.text + dropdown1.value.ToString();//Temporary UI objects that contain text values that can be pulled out
        HoldText.text = HoldText.text + dropdown2.value.ToString(); //Joins the two strings together in another UI text 
        //print(HoldText.text);  //DEBUG
        WriteString(HoldText.text); //Pushes the two joined strings into the WriteString function

        SceneManager.LoadScene("SampleScene"); //Changes scene to the arena scene

    }
	
	
	/*
	
	The text file should have 3 numberical characters. 
	The first position represents how many player controlled fighters will be in the arena, 1-4.   (Currently 0 is accepted)
	The second position represents how many AI controlled fighters will be in the arena, 0-3	   (Currently 4 is accepted)
	The third position represents what difficulty the AI should be set to. Not currently implemented but will be from 1-3
	
	Any other initializations can be added to the document.
	For some reason there is always a space written at the front of the text file, I think due to how i've set up the HoldText.text object. So all array values will be shifted right.
	
	*/

    

    public void ReadString(string input)   //Parameter string isn't currently implemented, but could be used in later iterations so it's there
    {

        path = "Assets/TextFiles/SceneChange.txt";  //Creates the full path to the correct text file
        StreamReader reader = new StreamReader(path); //Opens up a text file reader in memory and is directed to the previously defined text file
        readOut = reader.ReadToEnd(); //Reads the entire document and sets the output string to readOut
        reader.Close(); //Closes and removes the reader from memory    #####Important#####
        tempRead = readOut[1].ToString(); // DEBUG
        //print(readOut.Length); // DEBUG
       // print(tempRead);//DEBUG
        InitializeObjects(readOut); //Takes the string and moves into InitializeObjects function
    }

    public void InitializeObjects(string input)
    {
        string x = input; //Sets the input parameter to the previously read output string    (I don't know why I defined it straight to input but)

        x = input[1].ToString();  //Takes the second character in the string and sets it to x     (For some reason there's always a space in the 0 slot, haven't fixed it)
       // ActivatePlayers(x);  //This will then take the character and move into ActivatePlayers function

        x = input[2].ToString();
        ActivateAI(x); //Same as above but for third character, 2 in array, and into ActivateAI rather than player

    }


    public void ActivatePlayers(string input)    //Function will eventually be relpaced with instantiating when final prefabs are ready
    {
        string x = input;

        if(x == "0")
        {
			
            
			TempGameObject = null; //TempGameObject is then set to null for next sweep
			
        }
        if (x == "1")
        {

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(5, 0, 0), Quaternion.identity);
            TempGameObject.name = "Player1";
            
			
			
            TempGameObject = null;
        }
        if (x == "2")
        {
            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(5, 0, 0), Quaternion.identity);

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(-5, 0, 0), Quaternion.identity);
            TempGameObject.name = "Player2";

           

            TempGameObject = null;
        }

        if (x == "3")
        {
            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(5, 0, 0), Quaternion.identity);
            TempGameObject.name = "Player1";

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(-5, 0, 0), Quaternion.identity);
            TempGameObject.name = "Player2";

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(5, 0, 8), Quaternion.identity);
            TempGameObject.name = "Player3";

            

            TempGameObject = null;
        }

        if (x == "4")
        {
            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(5, 0, 0), Quaternion.identity);
            TempGameObject.name = "Player1";

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(-5, 0, 0), Quaternion.identity);
            TempGameObject.name = "Player2";

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(5, 0, 8), Quaternion.identity);
            TempGameObject.name = "Player3";

            TempGameObject = GameObject.Instantiate(PlayerPrefab, new Vector3(-5, 0, 8), Quaternion.identity);
            TempGameObject.name = "Player4";

            TempGameObject = null;
        }

    }


    public void ActivateAI(string input)
    {
        string x = input;

        if(x == "0")
        {
            
            return;
        }
        if(x == "1")
        {
            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall = TempGameObject.GetComponent<AI>();
            TempAICall.InitializeVariables(1, 2, 5, 5, "AI1");
            TempAICall.enabled = true;


           

            TempGameObject = null;
        }
        if (x == "2")
        {
            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall = TempGameObject.GetComponent<AI>();
            TempAICall.InitializeVariables(1, 2, 5, 5, "AI1");
            TempAICall.enabled = true;

            
            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(-5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall2 =  TempGameObject.GetComponent<AI>();
            TempAICall2.InitializeVariables(2, 2, 5, 5, "AI2");
            TempAICall2.enabled = true;


            TempGameObject = null;
        }
        if (x == "3")
        {
            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall = TempGameObject.GetComponent<AI>();
            TempAICall.InitializeVariables(1, 2, 5, 5, "AI1");
            TempAICall.enabled = true;


            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(-5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall2 = TempGameObject.GetComponent<AI>();
            TempAICall2.InitializeVariables(2, 2, 5, 5, "AI2");
            TempAICall2.enabled = true;

            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(5, 0, 8), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall3 = TempGameObject.GetComponent<AI>();
            TempAICall3.InitializeVariables(3, 2, 5, 5, "AI3");
            TempAICall3.enabled = true;


            TempGameObject = null;
        }
        if (x == "4")
        {
            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall = TempGameObject.GetComponent<AI>();
            TempAICall.InitializeVariables(1, 2, 5, 5, "AI1");
            TempAICall.enabled = true;


            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(-5, 0, 0), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall2 = TempGameObject.GetComponent<AI>();
            TempAICall2.InitializeVariables(2, 2, 5, 5, "AI2");
            TempAICall2.enabled = true;

            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(5, 0, 8), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall3 = TempGameObject.GetComponent<AI>();
            TempAICall3.InitializeVariables(3, 2, 5, 5, "AI3");
            TempAICall3.enabled = true;

            TempGameObject = GameObject.Instantiate(AIprefab, new Vector3(-5, 0, 8), Quaternion.identity); //TempGameObject is set to find the Player1 object in the scene      (GameObject.find() Can only find active objects in scene)
            AI TempAICall4 = TempGameObject.GetComponent<AI>();
            TempAICall4.InitializeVariables(4, 2, 5, 5, "AI3");
            TempAICall4.enabled = true;


            TempGameObject = null;
        }
    }

    public void WriteString(string input)
    {
        path = "Assets/TextFiles/SceneChange.txt";   //Finds the correct text file

        using (var stream = new FileStream(path, FileMode.Truncate))  //This takes a file path and uses the FileMode Truncate. This puts the file back down to 0 bytes, erasing its contents when it is opened
        {
            using (var writer = new StreamWriter(stream))  //This then opens a writer line into the text file
            {
                writer.Write(input); //This writes the inputted string into the file
            }
        }//Does the stream and writer need to be closed after use, I have literally no idea?
    }



}
