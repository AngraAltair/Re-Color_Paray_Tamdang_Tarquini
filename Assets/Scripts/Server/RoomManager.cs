using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;

/// <summary>
/// Script for managing the CreateServer scene's UI elements and room joining functions.
/// </summary>
public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("UI Elements")]
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    void Update()
    {
        if (string.IsNullOrEmpty(userNameInputField.text) || string.IsNullOrEmpty(roomNameInputField.text))
        {
            createRoomButton.interactable = false;
            joinRoomButton.interactable = false;
        }
        else
        {
            createRoomButton.interactable = true;
            joinRoomButton.interactable = true;
        }
    }

    public void CreateRoom()
    {
        PhotonNetwork.NickName = userNameInputField.text;
        PhotonNetwork.CreateRoom(roomNameInputField.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.NickName = userNameInputField.text;
        PhotonNetwork.JoinRoom(roomNameInputField.text);
    }

    public void ReturnToMainMenu()
    {
        PhotonNetwork.Disconnect();
        // SceneManager.LoadScene("GameType");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene("GameType");
        Debug.Log("Disconnected from server: " + cause.ToString());
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
        // PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        // PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }
}
