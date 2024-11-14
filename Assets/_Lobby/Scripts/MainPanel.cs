using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class MainPanel : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject createRoomPanel;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_InputField maxPlayerInputField;


    private void OnEnable()
    {
        createRoomPanel.SetActive(false);
    }

    public void CreateRoomMenu()
    {
        createRoomPanel.SetActive(true);

        // ���� ���� ��, �ʱ� �������� ���� �̸��� �������� ���Ѵ�.
        roomNameInputField.text = $"Room {Random.Range(1000, 10000)}";
        // �÷��̾��� ���� �ִ� �ο��� 8�� ���Ѵ�.
        maxPlayerInputField.text = "8";

        // �� ���� ���Ƿ� ������ ������, ������ �����ϴ�.
    }

    public void CreateRoomConfirm()
    {
        string roomName = roomNameInputField.text;

        if (roomName == "") return;

        int maxPlayer = int.Parse(maxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);
        // �ο��� ������ �ʿ��� ������ ������ ��,
        // ���Ƿ� MaxPlayer�� ���� ���ϰ�, �ش� ���� �Ѵ� ���� �ڵ����� ������ Max������ �ٲ��ִ� ����� ������ �߰��� �� ���� �� �ϴ�.

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void CreateRoomCancel()
    {
        createRoomPanel.SetActive(false);
    }

    public void RandomMatching()
    {
        Debug.Log("���� ��Ī ��û");

        // ����ִ� ���� ���� ��, ���� �ʴ� ���
        //PhotonNetwork.JoinRandomRoom(); 

        // ����ִ� ���� ���� ��, ������ ���� ����� ���� ���
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions() { MaxPlayers = 8 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: options);
    }

    public void JoinLobby()
    {
        Debug.Log("�κ� ���� ��û");
        PhotonNetwork.JoinLobby();
    }

    public void Logout()
    {
        Debug.Log("�α׾ƿ� ��û");
        PhotonNetwork.Disconnect();
    }
}