﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class WorldAgentCreate : MonoBehaviour
{

    public float worldBounds;
    public float growthGens;


    private GameObject curObj;


    public TMP_InputField tempTextInput;

    public Text userText;
    public Text myCommandText;
    public string currentCommands;
    private List<GameObject> objL;
    private List<GameObject> currentObjList;
    private List<Material> matL;
    private List<Material> currentMaterialList;

    private Vector3 dlaTarget;
    private List<GameObject> activeList;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tempA = Resources.LoadAll<GameObject>("PartPrefabs");
        objL = new List<GameObject>(tempA);
        for (int i = 0; i < objL.Count; i++)
        {
            var infoTag = objL[i].GetComponent<TPA_Info>();
            infoTag.idNumber = i;
        }
        currentObjList = new List<GameObject>();
        Material[] tempM = Resources.LoadAll<Material>("PartMaterials");
        matL = new List<Material>(tempM);
        currentMaterialList = new List<Material>();
        currentCommands = "pending";
        dlaTarget = new Vector3();
        activeList = new List<GameObject>();
    }

    private void Update()
    {


    }

    public void stringSort()//string inString
    {

        //temporary string from textfield
        var inString = tempTextInput.text;
        ////
        currentCommands = "";
        inString = inString.ToLower();
        for (int i = 0; i < growthGens; i++)
        {
            if (inString.Contains("right"))
            {
                dlaTarget = new Vector3(worldBounds, Random.Range(0, worldBounds), Random.Range(-worldBounds, worldBounds));

            }
            else if (inString.Contains("left"))
            {
                dlaTarget = new Vector3(-worldBounds, Random.Range(0, worldBounds), Random.Range(-worldBounds, worldBounds));

            }
            else if (inString.Contains("forward"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds, worldBounds), Random.Range(0, worldBounds), worldBounds);

            }
            else if (inString.Contains("back"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds, worldBounds), Random.Range(0, worldBounds), -worldBounds);

            }
            else if (inString.Contains("up"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds, worldBounds), worldBounds, Random.Range(-worldBounds, worldBounds));

            }


            //sortMaterials(inString);
            sortObjects(inString);
            loadWorld();
        }

    }

    private void sortObjects(string testString)
    {
        List<TPA_Info> sortedList = new List<TPA_Info>();
        currentObjList.Clear();
        var j = (int)Mathf.Floor(Random.Range(0, objL.Count));
        currentObjList.Add(objL[j]);
    }

    private void sortMaterials(string testString)
    {
        if (currentMaterialList.Count > 0)
        {
            currentMaterialList.Clear();
        }
        //for (int i = 0; i < matL.Count; i++)
        //{
        //    var nam = matL[i].name;
        //    nam = nam.ToLower();
        //    var nams = nam.Split('_');
        //    //Debug.Log(nams[0]);
        //    if (testString.Contains(nams[0]))
        //    {
        //        currentMaterialList.Add(matL[i]);
        //        if (!currentCommands.Contains(nams[0]))
        //        {
        //            currentCommands = currentCommands + nams[0] + " ";
        //        }
        //    }
        //    if (testString.Contains(nams[1]))
        //    {
        //        if (!currentCommands.Contains(nams[1]))
        //        {
        //            currentCommands = currentCommands + nams[1] + " ";
        //        }
        //    }
        //}
        if (currentMaterialList.Count < 1)
        {
            currentMaterialList = new List<Material>(matL);

        }

    }

    public void loadWorld()
    {        


        var startDla = new Vector3();
        var orient = new Vector3();

        if (curObj != null)//switchOn &&
        {
            startDla = dlaTarget - curObj.transform.position;
            startDla = forceOrtho(startDla);
            startDla.Normalize();
            startDla = startDla + curObj.transform.position;
        }
        else
        {
            orient = dlaTarget;
            orient = forceOrtho(startDla);

            startDla = new Vector3(0, .5f, 0);
        }



       

        var tempObj = Instantiate(currentObjList[0], transform);
        //var renderer = tempObj.GetComponentInChildren<Renderer>();
        //var matIndex = (int)Mathf.Floor(Random.Range(0, currentMaterialList.Count));
        //renderer.material = currentMaterialList[matIndex];
        tempObj.transform.position = startDla;
        //tempObj.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
        tempObj.layer = 10;
        var info = tempObj.GetComponent<TPA_Info>();
        info.lifeSpan = (int)Mathf.Floor(Random.Range(2500, 4000));
        //wAB.WorldAgentList.Add(tempObj);               
        curObj = tempObj;

    }

    private Vector3 forceOrtho(Vector3 inVec)
    {
        Vector3 temp = inVec;
        if (Mathf.Abs(temp.x) > Mathf.Abs(temp.y) && Mathf.Abs(temp.x) > Mathf.Abs(temp.z))
        {
            temp.y = 0;
            temp.z = 0;
        }
        else if (Mathf.Abs(temp.y) > Mathf.Abs(temp.z))
        {
            temp.z = 0;
            temp.x = 0;
        }
        else
        {
            temp.x = 0;
            temp.y = 0;
        }
        temp.Normalize();
        return temp;
    }
}


