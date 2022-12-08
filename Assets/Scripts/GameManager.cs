using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ObjectManager> objects = new Dictionary<int, ObjectManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            var ob = FindObjectsOfType<ObjectManager>();
            for (int i = 0; i < ob.Length; i++)
            {
                ob[i].id = i;
                objects.Add(i, ob[i]);
            }
            
        }

        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.escapeKey.isPressed)
            Application.Quit();
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if(_id==MyClient.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponentInChildren<TMP_Text>().text = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void DestroyClient(int _id)
    {
        Destroy(players[_id].transform.gameObject);
        players.Remove(_id);
    }

    public void ObjectFirstUpdate(int _id, Vector3 _pos, Quaternion _rot)
    {
        objects[_id].FirstSpawn(_pos, _rot);
    }

    public void ObjectPositionUpdate(int _id, Vector3 _pos, Quaternion _rot)
    {
        objects[_id].SetData(_pos, _rot);
    }
}
