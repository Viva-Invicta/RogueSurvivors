using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutoBattler
{
    public class UnitInventoryEntryView : UIView, IBeginDragHandler, IDragHandler
    {
        public event Action DragStarted;

        [SerializeField]
        private UIViewType viewType = UIViewType.UnitInventoryEntry;

        [SerializeField]
        private Image icon;

        public override UIViewType ViewType => viewType;

        public UnitType UnitType { get; private set; }

        public void Initialize(UnitType unitType, Sprite icon)
        {
            UnitType = unitType;
            this.icon.sprite = icon;
        }

        public override void Release()
        {
            DragStarted = null;
        }

        public void OnBeginDrag(PointerEventData _)
        {
            DragStarted?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //NEVER delete this for the sake of unity's developers' dumb ass
        }
    }
}