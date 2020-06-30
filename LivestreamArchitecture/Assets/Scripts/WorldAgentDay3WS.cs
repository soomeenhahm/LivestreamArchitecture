using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldAgentDay3WS : MonoBehaviour
{
    public float worldBounds;
    public int growthGens;
    public string commandText;
    public string userText;
    public string nnText;

    private Vector3 dlaTarget;

    private GameObject curObj;
    private List<GameObject> objL;
    private List<GameObject> currentObjList;
    private List<Material> matL;
    private List<Material> currentMaterialList;
    private List<GameObject> activeList;
    private List<List<GameObject>> catogorizedList;
    private List<float> nnVals;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tempA = Resources.LoadAll<GameObject>("PartPrefabs");
        objL = new List<GameObject>(tempA);
        currentObjList = new List<GameObject>();
        Material[] tempM = Resources.LoadAll<Material>("PartMaterials");
        matL = new List<Material>(tempM);
        currentMaterialList = new List<Material>();
        activeList = new List<GameObject>();
        nnVals = new List<float>();
        catogorizedList = new List<List<GameObject>>();
        catogorizeObjs();
    }


    public void stringSort(string serverText)//string inString
    {
        userText = serverText.Split(':')[0];
        commandText = serverText.Split(':')[1];
        commandText = commandText.ToLower();
        nnText = serverText.Split(':')[2];
        string2floatList(nnText);

        Debug.Log("user: " + userText);
        Debug.Log("command: " + commandText);
        Debug.Log("scores: " + nnText);

        sortObjects();

        for (int i = 0; i < growthGens; i++)
        {
            if (commandText.Contains("up"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), Random.Range(worldBounds / 2, worldBounds), Random.Range(-worldBounds / 2, worldBounds / 2));
            }
            else if (commandText.Contains("down"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), 0, Random.Range(-worldBounds / 2, worldBounds / 2));

            }
            else if (commandText.Contains("right"))
            {
                dlaTarget = new Vector3(Random.Range(worldBounds / 2, worldBounds), Random.Range(0, worldBounds), Random.Range(-worldBounds / 2, worldBounds / 2));
            }
            else if (commandText.Contains("left"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds, -worldBounds / 2), Random.Range(0, worldBounds), Random.Range(-worldBounds / 2, worldBounds / 2));
            }
            else if (commandText.Contains("forward"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), Random.Range(0, worldBounds), Random.Range(worldBounds / 2, worldBounds));
            }
            else if (commandText.Contains("back"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), Random.Range(0, worldBounds), -Random.Range(-worldBounds, -worldBounds / 2));
            }
            
            sortMaterials(commandText);
            loadWorld();
        }
    }

    private void catogorizeObjs()
    {
        var cats = new string[] { "anger", "anticipation", "disgust", "fear", "joy", "sadness", "surprise" };
        for(int i = 0; i < cats.Length; i++)
        {
            var tempList = new List<GameObject>();
            for(int j = 0; j < objL.Count; j++)
            {
                var info = objL[j].GetComponent<TPA_Info>();
                if (info.keyWords.Contains(cats[i])){
                    tempList.Add(objL[j]);
                }
            }
            catogorizedList.Add(tempList);
            Debug.Log("cat count: " + cats[i] + tempList.Count);
        }
    }

    private void sortObjects()
    {
        var cats = new string[] { "anger", "anticipation", "disgust", "fear", "joy", "sadness", "surprise" };
        currentObjList = new List<GameObject>();
        for(int i = 0; i < nnVals.Count; i++)
        {
            var nnCount = 10 * nnVals[i];

            Debug.Log("Cat: " + cats[i] + nnCount);
            if(nnCount > 1)
            {
                for(int j = 0; j < nnCount; j++)
                {
                    if (catogorizedList[i].Count > 1)
                    {
                        var gO = catogorizedList[i][(int)Mathf.Floor(Random.Range(0, catogorizedList[i].Count))];
                        currentObjList.Add(gO);
                    }
                    else
                    {
                        var gO = catogorizedList[i][0];
                        currentObjList.Add(gO);
                    }
                }
            }
        }
        if(currentObjList.Count > 0)
        {
            currentObjList.Add(objL[(int)Mathf.Floor(Random.Range(0, objL.Count))]);
        }
    }

    private void sortMaterials(string inText)
    {
        currentMaterialList.Clear();
        //loop through the materials and check by name against the command text
        for (int i = 0; i < matL.Count; i++)
        {
            var matName = matL[i].name;
            matName = matName.ToLower();
            var nameList = matName.Split('_');
            for (int j = 0; j < nameList.Length; j++)
            {
                if (inText.Contains(nameList[j]))
                {
                    currentMaterialList.Add(matL[i]);
                }
            }
        }

        if (currentMaterialList.Count == 0)
        {
            currentMaterialList.Add(matL[(int)Mathf.Floor(Random.Range(0, matL.Count))]);
        }
    }

    public void loadWorld()
    {
        var startDla = new Vector3();
        var orient = new Vector3();
        if (curObj != null)
        {
            startDla = dlaTarget - curObj.transform.position;
            startDla = forceOrtho(startDla);
            orient = startDla;
            //comment out detect collide if you want parts to overlay
            detectCollide(curObj.transform.position, orient);
            startDla = startDla + curObj.transform.position;
        }
        else
        {
            orient = dlaTarget;
            orient = forceOrtho(startDla);

            startDla = new Vector3(0, .5f, 0);
        }



        var tempObj = Instantiate(currentObjList[(int) Mathf.Floor(Random.Range(0,currentObjList.Count))], transform);
        tempObj.transform.position = startDla;
        if (orient.y != 0)
        {
            if (curObj != null)
            {
                tempObj.transform.rotation = curObj.transform.rotation;
            }
            else
            {
                tempObj.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
            }
        }
        else
        {
            tempObj.transform.rotation = Quaternion.LookRotation(orient, Vector3.up);
        }

        //random select a material from the current material list and assign it to our instance
        var mat = currentMaterialList[(int)Mathf.Floor(Random.Range(0, currentMaterialList.Count))];
        var render = tempObj.GetComponentInChildren<MeshRenderer>();
        render.material = mat;

        var info = tempObj.GetComponent<TPA_Info>();
        curObj = tempObj;
    }

    private void detectCollide(Vector3 p, Vector3 dir)
    {
        Ray myRay = new Ray(p, dir);
        RaycastHit hit;
        if (Physics.Raycast(myRay, out hit, 1))
        {
            var colObj = hit.collider.gameObject;
            Destroy(colObj);

        }
    }

    private void string2floatList(string inString)
    {
        var sVals = inString.Split(' ');
        nnVals = new List<float>();
        for(int i = 0; i < sVals.Length; i++)
        {
            float x = 0;
            float.TryParse(sVals[i], out x);
            if(x > 0)
            {
                nnVals.Add(x);
            }
            else
            {
                nnVals.Add(0.0f);
            }
        }
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
