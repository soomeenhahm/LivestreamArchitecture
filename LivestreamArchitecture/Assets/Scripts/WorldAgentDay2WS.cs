using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldAgentDay2WS : MonoBehaviour
{
    public float worldBounds;
    public int growthGens;
    public TMP_InputField textInput;

    private Vector3 dlaTarget;

    private GameObject curObj;
    private List<GameObject> objL;
    private List<GameObject> currentObjList;
    private List<Material> matL;
    private List<Material> currentMaterialList;
    private List<GameObject> activeList;


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
    }


    public void stringSort()//string inString
    {
        var commandText = textInput.text;

        commandText = commandText.ToLower();

        for (int i = 0; i < growthGens; i++)
        {
            if (commandText.Contains("up"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), Random.Range(worldBounds/2, worldBounds), Random.Range(-worldBounds / 2, worldBounds / 2));
            }
            else if (commandText.Contains("down"))
            {
                dlaTarget = new Vector3(Random.Range(-worldBounds / 2, worldBounds / 2), 0, Random.Range(-worldBounds / 2, worldBounds / 2));

            }
            else if (commandText.Contains("right"))
            {
                dlaTarget = new Vector3(Random.Range(worldBounds/2, worldBounds), Random.Range(0, worldBounds), Random.Range(-worldBounds / 2, worldBounds / 2));
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




            sortObjects();
            sortMaterials(commandText);
            loadWorld();
        }

    }

    private void sortObjects()
    {
        currentObjList.Clear();
        var j = (int)Mathf.Floor(Random.Range(0, objL.Count));
        currentObjList.Add(objL[j]);
    }

    private void sortMaterials(string inText)
    {
        currentMaterialList.Clear();
        //loop through the materials and check by name against the command text
        for(int i = 0; i < matL.Count; i++)
        {
            var matName = matL[i].name;
            matName = matName.ToLower();
            var nameList = matName.Split('_');
            for(int j = 0; j < nameList.Length; j++)
            {
                if (inText.Contains(nameList[j]))
                {
                    currentMaterialList.Add(matL[i]);
                }
            }
        }

        if(currentMaterialList.Count == 0)
        {
            currentMaterialList.Add(matL[(int) Mathf.Floor(Random.Range(0,matL.Count))]);
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
        
       

        var tempObj = Instantiate(currentObjList[0], transform);
        tempObj.transform.position = startDla;
        if (orient.y != 0)
        {
         if(curObj != null)
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
        var mat = currentMaterialList[(int) Mathf.Floor(Random.Range(0,currentMaterialList.Count))];
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
