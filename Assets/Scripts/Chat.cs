using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Chat : MonoBehaviour
{
    [SerializeField] private GameObject chat;
    [SerializeField] private GameObject mes;
    public float timer;
    private PhotonView photonView;
    public static bool isChating = false;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        timer-=Time.deltaTime;
        if (timer < 1 && timer > 0)
        {
            chat.GetComponent<TextMeshProUGUI>().alpha = timer;
        }
        else
        {
            chat.GetComponent<TextMeshProUGUI>().alpha = 1;
        }
        if (timer < 0)
        {
            chat.SetActive(false);
        }
        else
        {
            chat.SetActive(true);
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            chat.SetActive(true);
            mes.SetActive(!mes.activeInHierarchy);

            if (!mes.activeInHierarchy)
            {
                SendMessage();
            }
            else
            {
                mes.GetComponent<TMP_InputField>().ActivateInputField();
                isChating = true;
                timer = 500;
            }
        }
    }
    public void SendMessage()
    {
        timer = 5;
        photonView.RPC("Chating", RpcTarget.AllBuffered, PhotonNetwork.NickName, mes.GetComponent<TMP_InputField>().text);
        mes.GetComponent<TMP_InputField>().text = string.Empty;
        mes.SetActive(false);
        isChating=false;
    }

    [PunRPC]
    private void Chating(string nick, string message)
    {
        if (message == string.Empty) return;
        if (timer < 5)
        {
            timer = 5;
        }
        chat.GetComponent<TextMeshProUGUI>().text += nick + ": " + message + "\n";
    }
}
