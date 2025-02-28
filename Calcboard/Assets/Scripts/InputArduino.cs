using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;

public class Input_arduino : MonoBehaviour
{
    Thread IOThread;
    private static SerialPort sp;
    private static string incomingMsg = "";
    private static demoGameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<demoGameManager>();
    }

    void Start()
    {
        IOThread = new Thread(DataThread);
        IOThread.Start();
    }

    void OnDestroy()
    {
        if (IOThread != null && IOThread.IsAlive)
        {
            IOThread.Abort();
        }

        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }

    [System.Serializable]
    public class MessageData
    {
        public string type;
        public string tile;
        public string id;
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(incomingMsg))
        {
            try
            {
                MessageData messageData = JsonUtility.FromJson<MessageData>(incomingMsg);

                Debug.Log($"Received RFID Data: {incomingMsg}");
                gameManager.input = messageData.tile;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"JSON Parsing Error: {ex.Message}\nReceived: {incomingMsg}");
            }

            incomingMsg = ""; // Clear message after processing
        }
    }

    private static void DataThread()
    {
        sp = new SerialPort("COM7", 9600); // Adjust COM port if needed
        sp.Open();

        while (true)
        {
            if (sp.IsOpen)
            {
                try
                {
                    incomingMsg = sp.ReadLine().Trim(); // Read and trim serial input
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Serial Read Error: {ex.Message}");
                }
            }

            Thread.Sleep(100);
        }
    }
}