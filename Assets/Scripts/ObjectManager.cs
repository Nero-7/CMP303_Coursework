using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    public bool isHit = false;
    private Rigidbody body;

    private void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(isHit)
        {
            SendTransformToServer();       
        }
    }

    public void FirstSpawn(Vector3 _pos, Quaternion _rot)
    {
        isHit = false;
        transform.position = _pos;
        transform.rotation = _rot;
        position = transform.position;
        rotation = transform.rotation;
        body.velocity = Vector3.zero;
    }

    public void SetData(Vector3 _pos, Quaternion _rot)
    {
        transform.position = Vector3.Lerp(position, _pos, 1);
        transform.rotation = Quaternion.Lerp(rotation, _rot, 1);

        position = transform.position;
        rotation = transform.rotation;
    }

    private void SendTransformToServer()
    {
        position = transform.position;
        rotation = transform.rotation;

        ClientSend.ObjectPosition(id, position, rotation);

        if (body.velocity == Vector3.zero)
            isHit = false;
    }
}
