using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    private Dictionary<string, RoomEntry> roomDictionary = new Dictionary<string, RoomEntry>();

    public void LeaveLobby()
    {
        Debug.Log("�κ� ���� ��û");
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // 1. ���� ����� ��� + ���� ������� ��� + ������ �Ұ����� ���� ���(���û���)
            if (info.RemovedFromList == true || info.IsVisible == false || info.IsOpen == false)
            {
                // ���� ��Ȳ : �κ� ���� ���� ������� ���� ���,
                if (roomDictionary.ContainsKey(info.Name) == false) continue;

                // �� ����Ʈ�� �����Ѵ�.
                Destroy(roomDictionary[info.Name].gameObject);
                roomDictionary.Remove(info.Name);
            }

            // 2. ���ο� ���� ������ ���
            else if (roomDictionary.ContainsKey(info.Name) == false)
            {
                // ���� ����Ʈ�� �����Ѵ�.
                RoomEntry roomEntry = Instantiate(roomEntryPrefab, roomContent);
                roomDictionary.Add(info.Name, roomEntry);
                roomEntry.SetRoomInfo(info);
                // TODO : �� ���� ����
            }

            // 3. ���� ������ ����� ���
            else if (roomDictionary.ContainsKey((string)info.Name) == true)
            {
                // �� ����Ʈ�� �Էµ� ���� �������ش�.
                RoomEntry roomEntry = roomDictionary[info.Name];
                roomEntry.SetRoomInfo(info);
                // TODO : �� ���� ����
            }

        }
    }

    public void ClearRoomEntries()
    {
        // �÷��̾ �κ񿡼� ���� ��, �κ� �� �� ����Ʈ�� �����ȴ�.
        foreach (string name in roomDictionary.Keys)
        {
            Destroy(roomDictionary[name].gameObject);
        }

        roomDictionary.Clear();
    }
}