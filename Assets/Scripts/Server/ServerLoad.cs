using UnityEngine;
using Photon.Pun;

/// <summary>
/// Script for connecting to the server.
/// </summary>
public class ServerLoad : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject serverLoadingScreen;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        serverLoadingScreen.SetActive(false);
    }
}
