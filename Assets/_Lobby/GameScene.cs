using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 막힌 부분 코드 : 플레이어 생성
        PlayerSpawn();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        // 서버와의 연결이 끊어질 때 호출되는 함수
        PhotonNetwork.LoadLevel("Scene");
    }

    public override void OnLeftRoom()
    {
        // 방을 퇴장하면 호출되는 함수
        PhotonNetwork.LoadLevel("Scene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }



    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));

        // 플레이어 생성 시 색상을 랜덤하게 선정할 수 있도록 하는 코드
        Color color = Random.ColorHSV();
        object[] data = { color.r, color.g, color.b };

        // 막힌 부분 코드 : 플레이어 생성
        PhotonNetwork.InstantiateRoomObject("Player", randomPos, Quaternion.identity, data : data);
    }
}
