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

        // 방을 만들 때, 초기 설정으로 방의 이름을 랜덤으로 정한다.
        roomNameInputField.text = $"Room {Random.Range(1000, 10000)}";
        // 플레이어의 수의 최대 인원을 8로 정한다.
        maxPlayerInputField.text = "8";

        // 위 값은 임의로 설정된 것으로, 수정이 가능하다.
    }

    public void CreateRoomConfirm()
    {
        string roomName = roomNameInputField.text;

        if (roomName == "") return;

        int maxPlayer = int.Parse(maxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);
        // 인원의 제한이 필요한 게임을 제작할 때,
        // 임의로 MaxPlayer의 값을 정하고, 해당 값이 넘는 수는 자동으로 정해진 Max값으로 바꿔주는 기능의 구현을 추가할 수 있을 듯 하다.

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
        Debug.Log("랜덤 매칭 요청");

        // 비어있는 방이 없을 때, 들어가지 않는 방식
        //PhotonNetwork.JoinRandomRoom(); 

        // 비어있는 방이 없을 때, 스스로 방을 만들어 들어가는 방식
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions() { MaxPlayers = 8 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: options);
    }

    public void JoinLobby()
    {
        Debug.Log("로비 접속 신청");
        PhotonNetwork.JoinLobby();
    }

    public void Logout()
    {
        Debug.Log("로그아웃 요청");
        PhotonNetwork.Disconnect();
    }
}
