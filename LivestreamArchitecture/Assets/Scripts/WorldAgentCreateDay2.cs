using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class WorldAgentCreateDay2 : MonoBehaviour
{

    public float worldBounds;
    public float growthGens;
    public TMP_InputField tempTextInput;
    public Text userText;
    public Text myCommandText;
    public string currentCommands;

    private GameObject curObj;    
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
        for(int i = 0; i < activeList.Count; i++)
        {
            var info = activeList[i].GetComponent<TPA_Info>();
            info.lifeSpan += -1;
            if (info.lifeSpan < 0 && activeList.Count > 0)
            {
                var wa = activeList[i];
                Destroy(wa);
                activeList.Remove(wa);
            }
        }
        
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
            else if (inString.Contains("down"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds, worldBounds), 0.0f, Random.Range(-worldBounds, worldBounds));

            }


            sortMaterials(inString);
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
        for (int i = 0; i < matL.Count; i++)
        {
            var nam = matL[i].name;
            nam = nam.ToLower();
            var nams = nam.Split('_');
            for(int j = 0; j < nams.Length; j++)
            {
                if (testString.Contains(nams[j]))
                {
                    currentMaterialList.Add(matL[i]);
                }
            }
        }
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
        var renderer = tempObj.GetComponentInChildren<Renderer>();
        var matIndex = (int)Mathf.Floor(Random.Range(0, currentMaterialList.Count));
        renderer.material = currentMaterialList[matIndex];
        tempObj.transform.position = startDla;
        tempObj.transform.rotation = Quaternion.LookRotation(orient, Vector3.up);        
        var info = tempObj.GetComponent<TPA_Info>();
        info.lifeSpan = (int)Mathf.Floor(Random.Range(15000, 30000));
        activeList.Add(tempObj);               
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


