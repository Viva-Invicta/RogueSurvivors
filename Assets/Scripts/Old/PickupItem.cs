using DG.Tweening;
using UnityEngine;

namespace DunDungeons
{
    public abstract class PickupItem : MonoBehaviour
    {
        [SerializeField] private Faction targetFaction;
        [SerializeField] private AudioClip pickupClip;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float delayBeforeDestroy = 0.5f;

        private bool isPickedUp = false;

        private void OnTriggerEnter(Collider other)
        {
            if (isPickedUp)
            {
                return; 
            }

            if (!other.TryGetComponent<IHaveFaction>(out var entityWithFaction) || entityWithFaction.Faction != targetFaction)
            {
                return;
            }

            if (CheckPickupConditions(other))
            {
                isPickedUp = true;
                OnAfterPickup(other);

                if (audioSource && pickupClip)
                {
                    audioSource.PlayOneShot(pickupClip);
                }

                transform.DOScale(0, delayBeforeDestroy).OnComplete(() => Destroy(gameObject)).SetEase(Ease.InOutBack);
            }
        }

        protected virtual bool CheckPickupConditions(Collider other)
        {
            return true;

        }
        protected abstract void OnAfterPickup(Collider other);
    }
}