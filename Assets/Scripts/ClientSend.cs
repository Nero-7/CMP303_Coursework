using NetworkServer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class ClientSend : MonoBehaviour
{
   private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        MyClient.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        MyClient.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using(Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(MyClient.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);
            _packet.Write(GameManager.objects.Count);
            for (int i = 0; i < GameManager.objects.Count; i++)
            {
                _packet.Write(GameManager.objects[i].position);
                _packet.Write(GameManager.objects[i].rotation);
            }
            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs, Vector3 _position, Quaternion _rotation, Vector3 _camRotation)
    {
        using(Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(_position);
            _packet.Write(_rotation);
            _packet.Write(_camRotation);
            SendUDPData(_packet);
        }
    }

    public static void ObjectPosition(int _id, Vector3 _position, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.objectPosition))
        {
            _packet.Write(_id);
            _packet.Write(_position);
            _packet.Write(_rotation);
            SendUDPData(_packet);
        }
    }
    #endregion
}
