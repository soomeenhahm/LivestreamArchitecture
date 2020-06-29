using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WoldAgentDay3 : MonoBehaviour
{
    public float worldBounds;
    public int growthGens;    

    //for linking to text UI elements
    public string commandText;
    public string userText;
    public string nnText;

    private Vector3 dlaTarget;

    private GameObject curObj;
    private List<GameObject> objL;
    private List<List<GameObject>> categorizedList;
    private List<GameObject> currentObjList;
    private List<Material> matL;
    private List<Material> currentMaterialList;
    private List<GameObject> activeList;
    private List<float> nnVals;


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tempA = Resources.LoadAll<GameObject>("PartPrefabs");
        objL = new List<GameObject>(tempA);
        categorizedList = new List<List<GameObject>>();
        categorizeObjects();
        currentObjList = new List<GameObject>();
        Material[] tempM = Resources.LoadAll<Material>("PartMaterials");
        matL = new List<Material>(tempM);
        currentMaterialList = new List<Material>();
        activeList = new List<GameObject>();
    }


    public void stringSort(string serverText)//string inString
    {
        //split the incoming string from the server
        userText = serverText.Split(':')[0];
        commandText = serverText.Split(':')[1];
        var nnText = serverText.Split(':')[2];
        string2floatList(nnText);
        sortObjects();

        commandText = commandText.ToLower();

        

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
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), Random.Range(0, worldBounds), Random.Range(-worldBounds, -worldBounds / 2));
            }
            else
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds , worldBounds ), Random.Range(0, worldBounds), Random.Range(-worldBounds, worldBounds ));
            }

                       
            
            sortMaterials(commandText);
            loadWorld();
        }

    }

    private void categorizeObjects()
    {
        var catList = new List<string>(new string[] { "anger","anticipation", "disgust" ,"fear", "joy", "sadness", "surprise" });
        for (int i = 0; i < catList.Count; i++)
        {
            var tempList = new List<GameObject>();
            for (int j = 0; j < objL.Count; j++)
            {
                var objInfo = objL[j].GetComponent<TPA_Info>();
                if (objInfo.keyWords.Contains(catList[i]))
                {
                    tempList.Add(objL[j]);

                }
            }
            categorizedList.Add(tempList);
        }
    }

    private void sortObjects()
    {
        if (currentObjList != null)
        {
            currentObjList.Clear();
        }
        
        for(int i = 0; i < nnVals.Count; i++)
        {
            var catTotal =  nnVals[i] * 10.0f;
            //Debug.Log("current Cat: " + i);
            //Debug.Log("current CatTot: " + catTotal);

            if (catTotal > 0)
            {
                
                for (int j = 0; j < catTotal; j++)
                {
                    if (categorizedList[i].Count > 1)
                    {
                        currentObjList.Add(categorizedList[i][(int)Mathf.Floor(Random.Range(0, categorizedList[i].Count))]);
                    }
                    else
                    {
                        currentObjList.Add(categorizedList[i][0]);
                    }
                }
            }            
           
        }
        if (currentObjList.Count == 0)
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

    private void string2floatList(string inText)
    {
        var sVals = inText.Split(' ');
        //Debug.Log("length of sVals: " + sVals.Length);
        nnVals = new List<float>();
        float x = 0;
        for (int i = 0; i < sVals.Length; i++)
        {
            float.TryParse(sVals[i], out x);
            if (x > 0)
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
