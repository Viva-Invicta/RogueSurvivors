using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace DunDungeons
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField]
        private float minRange;

        [SerializeField]
        private float maxRange;

        [SerializeField]
        private float minIntensity;

        [SerializeField]
        private float maxIntensity;

        [SerializeField]
        private float minDuration;

        [SerializeField]
        private float maxDuration;

        [SerializeField]
        private float randomPositionRadius = 0.1f;

        [SerializeField]
        private bool startNextIntensityTween = true;

        [SerializeField]
        private bool startNextRangeTween = true;

        [SerializeField]
        private bool startNextPositionTween = true;

        private new Light light;
        private Vector3 initialPosition;

        private Tweener rangeTween;
        private Tweener intensityTween;
        private Tweener positionTween;

        private void OnEnable()
        {
            light = GetComponent<Light>();
            initialPosition = transform.localPosition;

            if (!light)
            {
                Debug.LogError("No light on " + gameObject.name);
            }
        }

        private void Start()
        {
            StartCoroutine(WaitBeforeStart());
        }

        private IEnumerator WaitBeforeStart()
        {
            yield return new WaitForSecondsRealtime(1f);
            StartRangeTween();
            StartIntensityTween();
            StartPositionTween();
        }

        private void StartRangeTween()
        {
            if (!startNextRangeTween)
            {
                return;
            }

            rangeTween?.Kill();

            var duration = Random.Range(minDuration, maxDuration);
            var targetRange = Random.Range(minRange, maxRange);

            rangeTween = DOTween.To(
                () => light.range, (float targetValue) => light.range = targetValue, targetRange, duration)
                .OnComplete(() =>
                {
                    StartRangeTween();
                });
        }

        private void StartIntensityTween()
        {
            if (!startNextIntensityTween)
            {
                return;
            }

            intensityTween?.Kill();
            var duration = Random.Range(minDuration, maxDuration);
            var targetIntensity = Random.Range(minIntensity, maxIntensity);

            intensityTween = DOTween.To(
                () => light.range, (float targetValue) => light.intensity = targetValue, targetIntensity, duration)
                .OnComplete( () => 
                {      
                    StartIntensityTween(); 
                });
        }

        private void StartPositionTween()
        {
            if (!startNextPositionTween)
            {
                return;
            }

            positionTween?.Kill();

            var duration = Random.Range(minDuration, maxDuration);
            var xPosition = Random.Range(initialPosition.x - randomPositionRadius / 2, initialPosition.x + randomPositionRadius / 2);
            var yPosition = Random.Range(initialPosition.y - randomPositionRadius / 2, initialPosition.y + randomPositionRadius / 2);
            var targetPosition = new Vector2(xPosition, yPosition);

            positionTween = transform.DOLocalMove(targetPosition, duration)
                .OnComplete(() => {
                    if (startNextPositionTween)
                    {
                        StartPositionTween();
                    }
                });
        }

    }
}