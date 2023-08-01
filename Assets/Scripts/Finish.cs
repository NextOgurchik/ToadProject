using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Finish : MonoBehaviour
{
    private PhotonView photonView;
    public static float timer;
    private string textScores;
    [SerializeField] private TextMeshProUGUI scores;
    [SerializeField] private TextMeshProUGUI time;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        time.text = timer.ToString();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            photonView.RPC("Data", RpcTarget.AllViaServer, PhotonNetwork.NickName, timer);
            timer = 0;
        }
    }
    [PunRPC]
    void Data(string nick, float time)
    {
        scores.text += nick + " - " + time.ToString()+"\n";
    }
}
