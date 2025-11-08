using UnityEngine;
using DG.Tweening;

namespace AutoBattler
{

    public class UnitEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject effectObject;

        [SerializeField]
        private float duration = 5f;

        public GameObject EffectObject => effectObject;

        public void Play(float delay = 0f)
        {
            if (!EffectObject)
            {
                return;
            }

            var timeToTurnOff = duration;

            if (delay > 0f)
            {
                timeToTurnOff += delay;
                DOVirtual.DelayedCall(delay, () => SetEffectActive(true));
            }
            else
            {
                SetEffectActive(true);
            }

            DOVirtual.DelayedCall(timeToTurnOff, () => SetEffectActive(false));
        }

        public void SetEffectActive(bool isActive)
        {
            if (effectObject)
            {
                effectObject.SetActive(isActive);
            }
        }
    }
}
