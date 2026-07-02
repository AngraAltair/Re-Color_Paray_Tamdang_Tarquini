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
    [SerializeField] private TextMeshProUGUI connectionStatusText;

    private bool isConnectedToMaster;

    private void Awake()
    {
        isConnectedToMaster = false;

        if (connectionStatusText != null)
        {
            connectionStatusText.gameObject.SetActive(false);
        }

        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }

    void Update()
    {
        bool hasValidInput = !string.IsNullOrEmpty(userNameInputField.text) && !string.IsNullOrEmpty(roomNameInputField.text);
        bool canInteract = isConnectedToMaster;

        createRoomButton.interactable = canInteract;
        joinRoomButton.interactable = canInteract;
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

    public void SetConnectedToMaster(bool connected)
    {
        isConnectedToMaster = connected;

        if (connectionStatusText != null)
        {
            connectionStatusText.gameObject.SetActive(connected);
            connectionStatusText.text = connected ? "Server connected." : "";
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("M_LevelSelection");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("M_LevelSelection");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
    }
}
