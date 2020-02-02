using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.IO;


public class ChangeScene : MonoBehaviour
{
    public string path;
    public string tempRead;
    public string readOut;

    public GameObject TempGameObject;
    // Start is called before the first frame update
    void Start()
    {
       
        path = Application.dataPath.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("l"))
        {
            ReadString("");
        }
    }

    public void ReadString(string input)
    {

        path = "Assets/TextFiles/SceneChange.txt";
        StreamReader reader = new StreamReader(path);
        readOut = reader.ReadLine();
        reader.Close();
        tempRead = readOut[0].ToString();
        print(readOut);
        print(tempRead);
        InitializeObjects(readOut);
    }

    public void InitializeObjects(string input)
    {
        string x = input;

        x = input[0].ToString();
        ActivatePlayers(x);

        x = input[1].ToString();
        ActivateAI(x);

    }


    public void ActivatePlayers(string input)
    {
        string x = input;

        if(x == "0")
        {
            return;
        }
        if (x == "1")
        {
            TempGameObject = GameObject.Find("Player1");
            TempGameObject.gameObject.SetActive(true);
            TempGameObject = null;
        }
        if (x == "2")
        {
            TempGameObject = GameObject.Find("Player1");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = GameObject.Find("Player2");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = null;
        }

        if (x == "3")
        {
            TempGameObject = GameObject.Find("Player1");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = GameObject.Find("Player2");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = GameObject.Find("Player3");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = null;
        }

        if (x == "4")
        {
            TempGameObject = GameObject.Find("Player1");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = GameObject.Find("Player2");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = GameObject.Find("Player3");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = GameObject.Find("Player4");
            TempGameObject.gameObject.SetActive(true);

            TempGameObject = null;
        }

    }


    public void ActivateAI(string input)
    {
        string x = input;

        if(x == "0")
        {
            TempGameObject = GameObject.Find("AI1");
            TempGameObject.SetActive(false);

            TempGameObject = GameObject.Find("AI2");
            TempGameObject.SetActive(false);

            TempGameObject = GameObject.Find("AI3");
            TempGameObject.SetActive(false);

            TempGameObject = GameObject.Find("AI4");
            TempGameObject.SetActive(false);
            return;
        }
        if(x == "1")
        {
            TempGameObject = GameObject.Find("AI1");
            TempGameObject.transform.position = new Vector3(6, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI2");
            TempGameObject.SetActive(false);
            

            TempGameObject = GameObject.Find("AI3");
            TempGameObject.SetActive(false);

            TempGameObject = GameObject.Find("AI4");
            TempGameObject.SetActive(false);

            TempGameObject = null;
        }
        if (x == "2")
        {
            TempGameObject = GameObject.Find("AI1");
            TempGameObject.transform.position = new Vector3(6, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI2");
            TempGameObject.transform.position = new Vector3(-5, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI3");
            TempGameObject.SetActive(false);

            TempGameObject = GameObject.Find("AI4");
            TempGameObject.SetActive(false);

            TempGameObject = null;
        }
        if (x == "3")
        {
            TempGameObject = GameObject.Find("AI1");
            TempGameObject.transform.position = new Vector3(6, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI2");
            TempGameObject.transform.position = new Vector3(-5, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI3");
            TempGameObject.transform.position = new Vector3(5, 0, 8);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI4");
            TempGameObject.SetActive(false);

            TempGameObject = null;
        }
        if (x == "4")
        {
            TempGameObject = GameObject.Find("AI1");
            TempGameObject.transform.position = new Vector3(6, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI2");
            TempGameObject.transform.position = new Vector3(-5, 0, 0);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI3");
            TempGameObject.transform.position = new Vector3(5, 0, 8);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;

            TempGameObject = GameObject.Find("AI4");
            TempGameObject.transform.position = new Vector3(-5, 0, 8);
            TempGameObject.GetComponentInChildren<AI>().enabled = true;
            TempGameObject.GetComponentInChildren<NavMeshAgent>().enabled = true;
            

            TempGameObject = null;
        }
    }

    public void WriteString(string input)
    {
        path = "Assets/TextFiles/SceneChange.txt";

        using (var stream = new FileStream(path, FileMode.Truncate))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(input);
            }
        }
    }



}
