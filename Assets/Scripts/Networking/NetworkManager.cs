using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private string _playerName = "";
    private string _gameVersion = "0.9";
    private List<RoomInfo> _createdRooms = new List<RoomInfo>();
    private string _roomName = "Room 1";
    private bool _joiningRoom = false;
    private bool _isAssassin = false;
    private ExitGames.Client.Photon.Hashtable _customRoomProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private GameObject _playerAssassin;
    [SerializeField] private GameObject _playerCop;

    [SerializeField] private GameObject _networkUICanvas;
    [SerializeField] private Transform _roomScrollView;
    [SerializeField] private GameObject _roomDetailsPanel;
    [SerializeField] private TMP_InputField _roomNameInput;

    private Player _player;

    private void Start()
    {
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
        _roomName = _roomNameInput.text;
        if (_roomName != "")
        {
            _joiningRoom = true;

            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.IsVisible = true;
            roomOptions.MaxPlayers = (byte)5;
            //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "HasAssassin", true} };

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

            //Set the room name
            panel.transform.GetChild(0).GetComponent<TMP_Text>().text = roomName;

            //Set the room count
            panel.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{roomCount}/{roomMax}";

            //Set join button function
            panel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { JoinRoom(roomName);
            });
        }
    }

    private void JoinRoom(string roomName)
    {
        Debug.Log($"Joining Room {roomName}");
        _joiningRoom = true;
        PhotonNetwork.NickName = _playerName;
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("===Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("===Connected to Room");
        print(PhotonNetwork.CurrentRoom.Players.Count);

        _networkUICanvas.SetActive(false);

        Player player = PhotonNetwork.CurrentRoom.Players.Last().Value;
        Debug.Log($"Nick {player.NickName}:::Play {_playerName} just joined the lobby");
        bool is_ass = (bool)player.CustomProperties["isAssassin"];

        if (is_ass)
        {
            PhotonNetwork.Instantiate(_playerAssassin.name, new Vector2(4, 1.5f), Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate(_playerCop.name, new Vector2(-4f, 1.5f), Quaternion.identity, 0);
        }
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
}
