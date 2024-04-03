using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Use a singleton pattern to keep the only one instance of the player manager across several scenes
    protected static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(PlayerManager).Name;
                    instance = obj.AddComponent<PlayerManager>();
                }
            }
            return instance;
        }
    }

    private string _nickname;

    public string Nickname 
    { 
        get 
        {
            return _nickname;
        } 
        set 
        { 
            _nickname = value;
        } 
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Nickname:: {Nickname}");
        }
    }
}
