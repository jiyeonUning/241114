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
        // ���� ������ ���, ���� �̸� �տ� Master ȣĪ�� �ٴ´�.
        if (player.IsMasterClient) nameText.text = $"Master\n{player.NickName}";
        else nameText.text = player.NickName;

        // ready ��ư�� ���� ���� ���� �غ� �˸���.
        readyButton.gameObject.SetActive(true);
        // �ٸ� ����� ready ��ư�� ���� �� ����.
        readyButton.interactable = player == PhotonNetwork.LocalPlayer;

        if (player.GetReady()) readyText.text = "READY";
        else readyText.text = "";
    }

    public void SetEmpty()
    {
        // �÷��̾ ������ ���, �÷��̾ ������ �ڸ��� NULL�� ���� �ڸ��� ����.
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
