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

        public int CurrentWave
        { 
            get => currentWave; 
            set => currentWave = value; 
        }

        private int currentWave = 0;

        public void SelectNextRoom()
        {
            //TODO
            ActiveRoomGrid = levelRooms.First();
            NextRoomSelected?.Invoke();
        }

        public void IncreaseWave() => CurrentWave++;

        public void ResetWave()
        {
            
        }
    }
}