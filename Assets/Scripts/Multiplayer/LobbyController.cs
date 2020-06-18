using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRoom("Lobby");
        Debug.Log("start");
    }

    public override void OnJoinRoomFailed(short returnCode, string message){
        Debug.Log("Failed to join Lobby");
        Debug.Log(message);
        CreateRoom("Lobby");
    }

    void CreateRoom(string roomname){
       Debug.Log("Creating room " + roomname);
       RoomOptions roomOps = new RoomOptions(){IsVisible = true, IsOpen = true, MaxPlayers = (byte)25};
       PhotonNetwork.CreateRoom("Lobby",roomOps);
       Debug.Log("Room " + roomname + " was created!");
    }

}
