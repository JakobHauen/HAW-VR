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
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("Failed to join a room");
        CreateRoom();
    }

    void CreateRoom(){
       Debug.Log("Creating room...");
       RoomOptions roomOps = new RoomOptions(){IsVisible = true, IsOpen = true, MaxPlayers = (byte)20};
       PhotonNetwork.CreateRoom("Room 1",roomOps);
       Debug.Log("Room was created!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
