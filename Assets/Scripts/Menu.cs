using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Dropdown dropdown;
    private void Start()
    {
        inputField.text = PlayerPrefs.GetString("name");
        dropdown.value = PlayerPrefs.GetInt("mode");
    }
    public void PlayButton()
    {
        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("name", inputField.text);
        SceneManager.LoadScene("Game");
    }
    public void MmButton()
    {
        PhotonNetwork.NickName = inputField.text;
        PlayerPrefs.SetString("name", inputField.text);
        SceneManager.LoadScene("Matchmacking");
    }
    public void Mode()
    {
        PlayerPrefs.SetInt("mode", dropdown.value);
    }
}
