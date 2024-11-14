using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
    [SerializeField] TMP_Text readyText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Button readyButton;

    public void SetPlayer(Player player)
    {
        // 내가 방장일 경우, 나의 이름 앞에 Master 호칭이 붙는다.
        if (player.IsMasterClient) nameText.text = $"Master\n{player.NickName}";
        else nameText.text = player.NickName;

        // ready 버튼을 통해 게임 시작 준비를 알린다.
        readyButton.gameObject.SetActive(true);
        // 다른 사람의 ready 버튼은 누를 수 없다.
        readyButton.interactable = player == PhotonNetwork.LocalPlayer;

        if (player.GetReady()) readyText.text = "READY";
        else readyText.text = "";
    }

    public void SetEmpty()
    {
        // 플레이어가 퇴장할 경우, 플레이어가 퇴장한 자리는 NULL을 띄우며 자리를 비운다.
        readyText.text = "";
        nameText.text = "NULL";
        readyButton.gameObject.SetActive(false);
    }

    public void Ready()
    {
        bool ready = PhotonNetwork.LocalPlayer.GetReady();

        if (ready) PhotonNetwork.LocalPlayer.SetReady(false);
        else PhotonNetwork.LocalPlayer.SetReady(true);
    }
}
