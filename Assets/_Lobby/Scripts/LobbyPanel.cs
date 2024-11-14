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
        Debug.Log("로비 퇴장 요청");
        PhotonNetwork.LeaveLobby();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // 1. 방이 사라진 경우 + 방이 비공개인 경우 + 입장이 불가능한 방인 경우(선택사항)
            if (info.RemovedFromList == true || info.IsVisible == false || info.IsOpen == false)
            {
                // 예외 상황 : 로비에 들어가자 마자 사라지는 방인 경우,
                if (roomDictionary.ContainsKey(info.Name) == false) continue;

                // 방 리스트를 삭제한다.
                Destroy(roomDictionary[info.Name].gameObject);
                roomDictionary.Remove(info.Name);
            }

            // 2. 새로운 방이 생성된 경우
            else if (roomDictionary.ContainsKey(info.Name) == false)
            {
                // 방의 리스트를 생성한다.
                RoomEntry roomEntry = Instantiate(roomEntryPrefab, roomContent);
                roomDictionary.Add(info.Name, roomEntry);
                roomEntry.SetRoomInfo(info);
                // TODO : 방 정보 설정
            }

            // 3. 방의 정보가 변경된 경우
            else if (roomDictionary.ContainsKey((string)info.Name) == true)
            {
                // 방 리스트에 입력된 값을 변경해준다.
                RoomEntry roomEntry = roomDictionary[info.Name];
                roomEntry.SetRoomInfo(info);
                // TODO : 방 정보 설정
            }

        }
    }

    public void ClearRoomEntries()
    {
        // 플레이어가 로비에서 나갈 때, 로비 내 방 리스트는 삭제된다.
        foreach (string name in roomDictionary.Keys)
        {
            Destroy(roomDictionary[name].gameObject);
        }

        roomDictionary.Clear();
    }
}
