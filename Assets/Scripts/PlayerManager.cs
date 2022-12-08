using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public bool local;

    private void FixedUpdate()
    {
        if(local)
            SendInputToServer();
    }

    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Keyboard.current.wKey.isPressed,
            Keyboard.current.aKey.isPressed,
            Keyboard.current.sKey.isPressed,
            Keyboard.current.dKey.isPressed,
            Keyboard.current.leftShiftKey.isPressed,
            Keyboard.current.spaceKey.isPressed,
            transform.GetChild(0).GetComponent<ThirdPersonController>().Grounded
        };
        Vector3 _pos = transform.GetChild(0).transform.position;
        Quaternion _rot = transform.GetChild(0).transform.rotation;
        Vector3 _camRot = transform.GetChild(1).transform.eulerAngles;

        ClientSend.PlayerMovement(_inputs, _pos, _rot, _camRot);
    }
}
