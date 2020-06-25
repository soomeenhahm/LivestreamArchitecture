using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAgentDay1 : MonoBehaviour
{
    public float worldBounds;
    public int growthGens;
    
    private Vector3 dlaTarget;

    private GameObject curObj;
    private List<GameObject> objL;
    private List<GameObject> currentObjList;
    private List<Material> matL;
    private List<Material> currentMaterialList;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tempA = Resources.LoadAll<GameObject>("PartPrefabs");
        objL = new List<GameObject>(tempA);
        currentObjList = new List<GameObject>();
        Material[] tempM = Resources.LoadAll<Material>("PartMaterials");
        matL = new List<Material>(tempM);
        currentMaterialList = new List<Material>();
    }


    public void stringSort()//string inString
    {
       
        for (int i = 0; i < growthGens; i++)
        {
           
           dlaTarget = new Vector3(Random.Range(-worldBounds, worldBounds), Random.Range(0, worldBounds/2), Random.Range(-worldBounds, worldBounds));
                   
           sortObjects();
           loadWorld();
        }

    }

    private void sortObjects()
    {        
        currentObjList.Clear();
        var j = (int)Mathf.Floor(Random.Range(0, objL.Count));
        currentObjList.Add(objL[j]);
    }

    public void loadWorld()
    {   
        var startDla = new Vector3();
        var orient = new Vector3() ;
        if ( curObj != null)
        {           
            startDla = dlaTarget - curObj.transform.position;
            startDla = forceOrtho(startDla);
            orient = startDla;
            startDla = startDla + curObj.transform.position;
        }
        else
        {
            startDla = dlaTarget;
            startDla = forceOrtho(startDla);
            orient = startDla;
            startDla.y += .5f;
        }        

        var tempObj = Instantiate(currentObjList[0], transform);       
        tempObj.transform.position = startDla;
        tempObj.transform.rotation = Quaternion.LookRotation(orient, Vector3.up);
        var info = tempObj.GetComponent<TPA_Info>();                   
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
