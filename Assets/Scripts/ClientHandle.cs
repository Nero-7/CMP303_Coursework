using NetworkServer;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Windows;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _id = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        MyClient.instance.myId = _id;
        ClientSend.WelcomeReceived();

        MyClient.instance.udp.Connect(((IPEndPoint)MyClient.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        if(_packet.UnreadLength()>0)
        {
            for (int i = 0; i < GameManager.objects.Count; i++)
            {
                Vector3 _obPos = _packet.ReadVector3();
                Quaternion _obRot = _packet.ReadQuaternion();
                GameManager.instance.ObjectPositionUpdate(i, _obPos, _obRot);
            }       
            
        }
        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector2 _input = _packet.ReadVector2();
        Vector3 _position = _packet.ReadVector3();
        bool _grounded = _packet.ReadBool();

        //GameManager.players[_id].transform.GetChild(0).transform.position = _position;

        //Local player
        if(_id== MyClient.instance.myId)
        {
            GameManager.players[_id].GetComponentInChildren<ThirdPersonController>()._serverInput = _input;
        }
        else
        {
            GameManager.players[_id].GetComponentInChildren<OnlinePlayer>().Grounded = _grounded;
            //If character desyncs
            if (Vector3.Distance(GameManager.players[_id].GetComponentInChildren<OnlinePlayer>().transform.position, _position) > 0.5f)
            {
                GameManager.players[_id].GetComponentInChildren<OnlinePlayer>()._controller.enabled = false;
                GameManager.players[_id].GetComponentInChildren<OnlinePlayer>().transform.position = _position;
                GameManager.players[_id].GetComponentInChildren<OnlinePlayer>()._controller.enabled = true;
            }
            else
            {
                GameManager.players[_id].GetComponentInChildren<OnlinePlayer>()._serverInput = _input;
            }
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        Vector3 _camRotation = _packet.ReadVector3();
        if (_id == MyClient.instance.myId)
        {
            GameManager.players[_id].transform.GetChild(0).transform.rotation = _rotation;
        }
       else
        {
            GameManager.players[_id].transform.GetChild(0).transform.rotation = _rotation;
            GameManager.players[_id].GetComponentInChildren<OnlinePlayer>()._pseudoCamRot = _camRotation;
        }
    }

    public static void PlayerSprint(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _sprint = _packet.ReadBool();

        //Local player
        if (_id == MyClient.instance.myId)
        {
            GameManager.players[_id].GetComponentInChildren<ThirdPersonController>()._isSprinting = _sprint;
        }
        else
        {
            GameManager.players[_id].GetComponentInChildren<OnlinePlayer>()._isSprinting = _sprint;
        }
    }

    public static void PlayerJump(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _jump = _packet.ReadBool();
        
        //Local player
        if (_id == MyClient.instance.myId)
        {
            GameManager.players[_id].GetComponentInChildren<ThirdPersonController>()._isJumping = _jump;
        }
        else
        {
            GameManager.players[_id].GetComponentInChildren<OnlinePlayer>()._isJumping = _jump;
           
        }
    }

    public static void PlayerDisconnect(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.instance.DestroyClient(_id);
    }

    public static void ObjectPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _pos = _packet.ReadVector3();
        Quaternion _rot = _packet.ReadQuaternion();

        GameManager.instance.ObjectPositionUpdate(_id, _pos, _rot);
    }

    public static void SpawnObjectPosition(Packet _packet)
    {
        int _count = _packet.ReadInt();
        for (int i = 0; i < _count; i++)
        {
            Vector3 _pos = _packet.ReadVector3();
            Quaternion _rot = _packet.ReadQuaternion();
            GameManager.instance.ObjectFirstUpdate(i, _pos, _rot);
        }
    }
}
