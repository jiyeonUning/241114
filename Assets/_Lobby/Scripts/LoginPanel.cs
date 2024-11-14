using Photon.Pun;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;

    private void Start()
    {
        // 임의로 플레이어의 이름을 설정한다. 추후 변경이 가능하다.
        idInputField.text = $"Player {Random.Range(1000, 10000)}";
    }

    public void Login()
    {
        if (idInputField.text == "") return;

        // 서버에게 요청할 땐, 항상 PhotonNetwork로 할 수 있다.
        PhotonNetwork.LocalPlayer.NickName = idInputField.text; // 플레이어의 닉네임 설정
        PhotonNetwork.ConnectUsingSettings();                   // 포톤 설정파일을 해당 내용으로 접속을 신청
    }
}
