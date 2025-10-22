using UnityEngine;
using UnityEngine.EventSystems;

namespace AutoBattler.Debugging
{
    public class DragEventLogger : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("PointerDown on " + gameObject.name + " at " + eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag on " + gameObject.name + " delta " + eventData.delta);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("OnDrag on " + gameObject.name);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("OnEndDrag on " + gameObject.name);
        }
    }
}