using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject panel;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (!PhotonNetwork.InRoom) return;
        panel.SetActive(false);
        PhotonNetwork.Instantiate("Toad", Vector3.zero, Quaternion.identity);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        RoomOptions rO = new RoomOptions();
        PhotonNetwork.JoinOrCreateRoom("test",rO,TypedLobby.Default);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        PhotonNetwork.LoadLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
