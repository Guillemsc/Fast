using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptTester : MonoBehaviour
{
    [SerializeField] private bool as_server = false;

    [SerializeField] private TMPro.TextMeshProUGUI messages_text = null;

    private Fast.Networking.Server server = null;
    private Fast.Networking.Client client = null;

    void Start()
    {
        if (as_server)
        {
            Fast.FastInstance.Instance.MApplication.MaxFps = 5;
            Fast.FastInstance.Instance.MApplication.VSync = false;

            server = new Fast.Networking.Server(8184);
            server.Start();

            messages_text.text += "Server started at: " + server.Port; 

            Debug.Log("Server started at: " + server.Port);
        }
        else
        {
            //157.245.79.172

            client = new Fast.Networking.Client("localhost", 8184);
            client.Connect();

            client.OnConnected.Subscribe(delegate ()
            {
                messages_text.text += "Client connected to server at at:" + client.IP + " and: " + client.Port;

                Debug.Log("Client connected to server at at:" + client.IP + " and: " + client.Port);

            });
        }
    }

    void Update()
    {
        if(as_server)
        {
            server.ReadMessages();
        }
        else
        {
            client.ReadMessages();

            List<object> messages = client.PopMessages();

            for(int i = 0; i < messages.Count; ++i)
            {
                string str_msg = (string)messages[i];

                messages_text.text += "Message: " + str_msg;

                Debug.Log("Message: " + str_msg);
            }
        }
    }
}
