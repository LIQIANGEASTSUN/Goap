using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间
/// </summary>
public class Room
{
    public static Room Current = null;

    private List<RoomPlayer> roomPlayerList = new List<RoomPlayer>();

    public static void CreateRoom()
    {
        if (Current != null)
        {
            Release();
        }

        Current = new Room();
    }

    public static void Release()
    {
        Current = null;
    }

    public Room()
    {
        CreatePlayer();
    }

    public void OnFrame()
    {
        for (int i = 0; i < roomPlayerList.Count; ++i)
        {
            roomPlayerList[i].OnFrame();
        }
    }

    private void CreatePlayer()
    {
        roomPlayerList.Clear();

        for (int i = 0; i < 2; ++i)
        {
            Camp camp = (Camp)(1 << i);
            RoomPlayer roomPlayer = new RoomPlayer(camp);
            roomPlayerList.Add(roomPlayer);
        }
    }
	
    public RoomPlayer GetRoomPlayer(int index)
    {
        if (index < roomPlayerList.Count)
        {
            return roomPlayerList[index];
        }

        return null;
    }

    public RoomPlayer GetRoomPlayer(Camp camp)
    {
        for (int i = 0; i < roomPlayerList.Count; ++i)
        {
            RoomPlayer roomPlayer = roomPlayerList[i];
            if (roomPlayer.Camp == camp)
            {
                return roomPlayer;
            }
        }

        return null;
    }
}