using System;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class RoomsService : MonoBehaviour
    {
        public event Action NextRoomSelected;

        [SerializeField]
        private RoomGrid[] levelRooms;

        public RoomGrid ActiveRoomGrid { get; private set; }

        public void SelectNextRoom()
        {
            //TODO
            ActiveRoomGrid = levelRooms.First();
            NextRoomSelected?.Invoke();
        }
    }
}