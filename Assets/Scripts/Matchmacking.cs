using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Matchmaking : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loadPanel;
    private new PhotonView photonView;
    private int rating;
    public void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            loadPanel.SetActive(false);
            photonView = GetComponent<PhotonView>();
            rating = 500;
            if (!photonView.IsMine) return;
            photonView.RPC("Data", RpcTarget.All, PlayerPrefs.GetString("name"), rating);
            PhotonNetwork.LocalPlayer.CustomProperties.Add("rating", 500);
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                player.CustomProperties.TryGetValue("rating", out object val);
                string val2 = System.Convert.ToString(val);
                Debug.Log(player.NickName + ":" + val2);
            }
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Matchmacking", new RoomOptions(), TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Matchmacking");
    }
    [PunRPC]
    private void Data(string nickname, int points)
    {

    }
}
