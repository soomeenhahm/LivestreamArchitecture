using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using TMPro;

public class serverManager : MonoBehaviour {
    public WorldAgentCreate myCreate;
    public int outPort = 5500;
    public int inPort = 5501;
    private int readCnt = 0;
    //public TMP_InputField inputField;
    public static string inputText;

    private bool connectTry = true;
    public clientUtil myUtil;
    // Use this for initialization
    

	
	// Update is called once per frame
	

    public void connectServer()
    {
        if (connectTry)
        {
            myUtil.setupOutSocket(outPort);
            Debug.Log("trying to connect");
            connectTry = false;
        }
        else
        {
            CloseServer();
        }
    }

    public void CloseServer()
    {
        myUtil.closeOutSocket();
        
    }
    public void sendPts()
    {

        //string input = inputField.text;
        
       // Debug.Log("i'm sending this words: " + input);   
            
       // myUtil.writeSocket(input, false);
        
        
    }
    
    public void sendPtsIn()
    {
        var curFloats = new List<float> { UnityEngine.Random.Range(0.0f,1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f), UnityEngine.Random.Range(0.0f, 1f) };
        //tvs.importObjFromText(curFloats,"just having some fun");
        //oldFloat = curFloats[0];
    }
      
    
}
