using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class clientUtil : MonoBehaviour
{
    internal Boolean outSocketReady = false;
    public WoldAgentDay3 myCreate;
    public string curString;    

    public int outPort = 5505;
    

    private string myMsg;
    TcpClient outSocket;
    private Thread clientReceiveThread;
    private bool newText;
    private bool connectTry = true;
    NetworkStream outStream;

    StreamWriter theWriter;

    String Host = "127.0.0.1";

    // Start is called before the first frame update
    void Start()
    {
        curString = "kitty";
       
        newText = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (newText)
        {
            myCreate.stringSort(curString);
            
            newText = false;
        }
      
    }

    public void connectServer()
    {
        if (connectTry)
        {
            setupOutSocket(outPort);
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
        closeOutSocket();

    }

    public void setupOutSocket(Int32 oP)
    {
        try
        {
            outSocket = new TcpClient(Host, oP);
            outStream = outSocket.GetStream();              
            outSocketReady = true;
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: can't connect out");
        }
    }



    public void closeOutSocket()
    {
        if (!outSocketReady)
            return;
        theWriter.Close();

        outSocket.Close();

        outSocketReady = false;
    }

    public void ListenForData()
    {
        try
        {

            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = outSocket.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        Debug.Log("server message received as: " + serverMessage);
                        curString = serverMessage;
                        newText = true;
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
