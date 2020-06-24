using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class WorldAgentCreate : MonoBehaviour
{
    public int worldCnt;
    public float worldBounds;
    public float worldScale;
    public float spawnDis;
    private GameObject curObj;


    public TMP_InputField tempTextInput;

    public Text myText;
    public Text myCommandText;
    public string currentCommands;
    private List<GameObject> objL;
    private List<GameObject> currentObjList;
    private List<Material> matL;
    private List<Material> currentMaterialList;
    private List<Vector3> tickList;
    private bool planCheck;
    private bool switchOn;
    private float planTimer = 0.0f;
    private float waitTime = 5.0f;
    private bool trigger;


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
        Vector3[] tempV = { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(0, 0, 1), new Vector3(0, 1, 0) };
        tickList = new List<Vector3>(tempV);
       
        switchOn = false;
        trigger = false;
        currentCommands = "pending";
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
        var uname = inString.Split(':')[0];

        inString = inString.ToLower();
        var curVec = tickList[0];
        if (inString.Contains("right"))
        {
            curVec = tickList[1];
        }
        else if (inString.Contains("left"))
        {
            curVec = tickList[0];
        }
        else if (inString.Contains("forward"))
        {
            curVec = tickList[2];
        }
        else if (inString.Contains("back"))
        {
            curVec = tickList[3];
        }
        else if (inString.Contains("up"))
        {
            curVec = tickList[4];
        }

       
        sortMaterials(inString);
        sortObjects(inString);
        loadWorld(curVec);
       
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

    public void loadWorld(Vector3 growDir)
    {
        

            //var wAB = GetComponent<WorldAgentBehavior>();        





            var start = growDir * spawnDis;
            if (switchOn && curObj != null)//switchOn &&
            {
                start = start + curObj.transform.position;
            }

            start.y = start.y + .75f;

            var tempObj = Instantiate(currentObjList[0], transform);
            //var renderer = tempObj.GetComponentInChildren<Renderer>();
            //var matIndex = (int)Mathf.Floor(Random.Range(0, currentMaterialList.Count));
            //renderer.material = currentMaterialList[matIndex];
            tempObj.transform.position = start;
            //tempObj.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
            tempObj.layer = 10;
            var info = tempObj.GetComponent<TPA_Info>();
            info.lifeSpan = (int)Mathf.Floor(Random.Range(2500, 4000));
            //wAB.WorldAgentList.Add(tempObj);               
            curObj = tempObj;



            switchOn = true;

        

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


