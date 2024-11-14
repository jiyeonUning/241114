using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameScene : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // ���� �κ� �ڵ� : �÷��̾� ����
        PlayerSpawn();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        // �������� ������ ������ �� ȣ��Ǵ� �Լ�
        PhotonNetwork.LoadLevel("Scene");
    }

    public override void OnLeftRoom()
    {
        // ���� �����ϸ� ȣ��Ǵ� �Լ�
        PhotonNetwork.LoadLevel("Scene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }



    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 1, Random.Range(-5f, 5f));

        // �÷��̾� ���� �� ������ �����ϰ� ������ �� �ֵ��� �ϴ� �ڵ�
        Color color = Random.ColorHSV();
        object[] data = { color.r, color.g, color.b };

        // ���� �κ� �ڵ� : �÷��̾� ����
        PhotonNetwork.InstantiateRoomObject("Player", randomPos, Quaternion.identity, data : data);
    }
}
