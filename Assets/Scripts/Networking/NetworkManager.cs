using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string _playerName = "";
    private string _gameVersion = "0.9";
    private List<RoomInfo> _createdRooms = new List<RoomInfo>();
    private string _roomName = "Room 1";
    private bool _joiningRoom = false;
    private bool _isAssassin = false;
    private ExitGames.Client.Photon.Hashtable _customRoomProperties = new ExitGames.Client.Photon.Hashtable();

    private int _maxCops = 2;
    private int _maxAssassin = 1;
    private int _copCount;
    private int _assassinCount;

    [SerializeField] private GameObject _playerAssassin;
    [SerializeField] private GameObject _playerCop;

    [SerializeField] private GameObject _networkUICanvas;
    [SerializeField] private Transform _roomScrollView;
    [SerializeField] private GameObject _roomDetailsPanel;
    [SerializeField] private TMP_InputField _roomNameInput;

    [SerializeField] private List<Transform> _spawnLocations = new List<Transform>();

    private Player _player;

    [SerializeField] private GameObject _door;

    private void Start()
    {
        _copCount = 0;
        _assassinCount = 0;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = _gameVersion;
            Connect();
        }

        _playerName = PlayerManager.Instance.Nickname;

        PlayAsAssassin();
    }

    private void Update()
    {
        //if (_joiningRoom || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        //{
        //    _networkUICanvas.SetActive(false);
        //}
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"OnFailedToConnectToPhoton. Status Code: {cause} Server Address: {PhotonNetwork.ServerAddress}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"OnConectedToMaster\nConnection made to {PhotonNetwork.CloudRegion} server.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"We have recieved the room list");
        _createdRooms = roomList;
    }

    public void CreateRoom()
    {
        _roomName = _roomNameInput.text.Trim();
        if (_roomName != "")
        {
            for (int i = 0; i < _createdRooms.Count; i++)
            {
                string roomName = _createdRooms[i].Name;
                if (string.Equals(roomName, _roomName))
                {
                    Debug.Log($"Room {_roomName} already exists! please use another room name");
                    _roomNameInput.text = "";
                    return;
                }
            }
            _joiningRoom = true;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = (byte)5;
            roomOptions.BroadcastPropsChangeToAll = true;
            roomOptions.CustomRoomProperties = _customRoomProperties;
            string[] customPropertiesForLobby = new String[1]
            {
                "isAssassin"
            };
            roomOptions.CustomRoomPropertiesForLobby = customPropertiesForLobby;

            Debug.Log($"Joining Room {_roomName}");
            PhotonNetwork.JoinOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);

            RefreshRooms();
        }
    }

    public void RefreshRooms()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        foreach (Transform child in _roomScrollView)
        {
            Destroy(child);
        }

        for (int i = 0; i < _createdRooms.Count; i++)
        {
            var panel = Instantiate(_roomDetailsPanel, _roomScrollView);

            string roomName = _createdRooms[i].Name;
            int roomCount = _createdRooms[i].PlayerCount;
            int roomMax = _createdRooms[i].MaxPlayers;
            int roomID = _createdRooms[i].masterClientId;
            bool roomHasAssassin = (bool)_createdRooms[i].CustomProperties["isAssassin"];

            //Set the room name
            panel.transform.GetChild(0).GetComponent<TMP_Text>().text = roomName;

            //Set the room count
            panel.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{roomCount}/{roomMax}";

            //Set join button function
            //panel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { JoinRoom(roomName);
            //});

            //Set Join as Cop
            Button jCopButton = panel.transform.GetChild(2).GetComponent<Button>();
            jCopButton.onClick.AddListener(() => {
                JoinRoomCop(jCopButton);
            });
            //Set Join as Assassin
            Button jAssButton = panel.transform.GetChild(3).GetComponent<Button>();
            jAssButton.onClick.AddListener(() => {
                JoinRoomAssassin(jAssButton);
            });

            if (roomHasAssassin)
            {
                Debug.Log($"Room {roomName} has assassin");
                jAssButton.interactable = false;
            }
            panel.transform.GetChild(4).GetComponent<TMP_Text>().text = $"{roomID}";
        }
    }

    private void JoinRoom(string roomName)
    {
        Debug.Log($"Joining Room {roomName}");
        _joiningRoom = true;
        PhotonNetwork.NickName = _playerName;
        PhotonNetwork.JoinRoom(roomName);
    }
    private void JoinRoomCop(Button button)
    {
        //PlayAsCop();
        GameObject panel = button.transform.parent.gameObject;
        string selectedRoomName = panel.transform.GetChild(0).GetComponent<TMP_Text>().text;
        for (int i = 0; i < _createdRooms.Count; i++)
        {
            string rname = _createdRooms[i].Name;
            if (string.Equals(rname, selectedRoomName))
            {
                Debug.Log($"Joining {selectedRoomName}");
                _roomNameInput.text = "";
                //return;
            }
        }
        _joiningRoom = true;
        _isAssassin = false;
        //PhotonNetwork.NickName = _playerName;
        PhotonNetwork.JoinRoom(selectedRoomName);
    }
    private void JoinRoomAssassin(Button button)
    {
        GameObject panel = button.transform.parent.gameObject;
        string selectedRoomName = panel.transform.GetChild(0).GetComponent<TMP_Text>().text;
        for (int i = 0; i < _createdRooms.Count; i++)
        {
            string rname = _createdRooms[i].Name;
            if (string.Equals(rname, selectedRoomName))
            {
                Debug.Log($"Joining {selectedRoomName}");
                _roomNameInput.text = "";
                //return;
            }

        }
        _joiningRoom = true;
        _isAssassin = true;
        //PhotonNetwork.NickName = _playerName;
        PhotonNetwork.JoinRoom(selectedRoomName);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("===Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("===Connected to Room");
        print(PhotonNetwork.CurrentRoom.Players.Count);


        Player player = PhotonNetwork.CurrentRoom.Players.Last().Value;
        Debug.Log($"Nick {player.NickName}:::Play {_playerName} just joined the lobby");
        bool is_ass = (bool)player.CustomProperties["isAssassin"];

        if (_isAssassin)
        {
            _assassinCount = GetAssassinCount();
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customRoomProperties);
            if (_assassinCount >= _maxAssassin) return;
            PhotonNetwork.Instantiate(_playerAssassin.name, new Vector2(4, 1.5f), Quaternion.identity, 0);
        }
        else
        {
            _assassinCount = (bool)PhotonNetwork.CurrentRoom.CustomProperties["isAssassin"] ? 1 : 0;
            _copCount = PhotonNetwork.CurrentRoom.Players.Count - _assassinCount;
                //GameObject.FindGameObjectsWithTag("Cop").Length;
            print($"Ass {_assassinCount} Before instantiation");
            print($"Cop {_copCount} Before instantiation");
            if (_copCount > _maxCops)
                return;

            Transform sl;
            if (_spawnLocations.Count > 0)
            {
                sl = _spawnLocations[0];
                _spawnLocations.RemoveAt(0);
                PhotonNetwork.Instantiate(_playerCop.name, sl.position, Quaternion.identity, 0);
            }
            else
                PhotonNetwork.Instantiate(_playerCop.name, new Vector2(2f, 26f), Quaternion.identity, 0);

            //_copCount++;
            if (_copCount >= _maxCops)
            {
                //Start game
                _door.SetActive(false);
            }
        }
        _networkUICanvas.SetActive(false);
    }

    public void PlayAsCop()
    {
        _isAssassin = false;
        _customRoomProperties["isAssassin"] = _isAssassin;
        PhotonNetwork.LocalPlayer.CustomProperties = _customRoomProperties;
    }

    public void PlayAsAssassin()
    {
        _isAssassin = true;
        _customRoomProperties["isAssassin"] = _isAssassin;
        PhotonNetwork.LocalPlayer.CustomProperties = _customRoomProperties;
    }

    private int GetAssassinCount()
    {
        return GameObject.FindGameObjectsWithTag("Assassin").Length;
    }
}