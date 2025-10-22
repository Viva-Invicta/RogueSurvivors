using System.Linq;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] damageAudioClips;

        [SerializeField]
        private AudioClip[] attackAudioClips;

        [SerializeField]
        private AudioClip deathSound;

        [SerializeField]
        private AudioClip dashSound;

        [SerializeField]
        private AudioSource audioSource;

        public void PlayAttackClip()
        {
            if (audioSource && attackAudioClips.Any())
            {
                var attackAudioClip = attackAudioClips[Random.Range(0, attackAudioClips.Length)];
                audioSource.PlayOneShot(attackAudioClip);
            }
        }

        public void PlayDamagedClip()
        {
            if (audioSource && damageAudioClips.Any())
            {
                var damageAudioClip = damageAudioClips[Random.Range(0, damageAudioClips.Length)];
                audioSource.PlayOneShot(damageAudioClip);
            }
        }

        public void PlayDashClip()
        {
            if (audioSource && dashSound)
            {
                audioSource.PlayOneShot(dashSound);
            }
        }

        public void PlayDeathClip()
        {
            if (deathSound && audioSource)
            {
                audioSource.PlayOneShot(deathSound);
            }
        }
    }
}
