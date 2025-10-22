using UnityEngine;

namespace AutoBattler
{
    public class UnitBehaviourController : MonoBehaviour
    {
        [field: SerializeField] public UnitConfiguration Configuration;

        private bool isPreview;
        //private CharacterController characterController;

        public bool IsPreview
        {
            get => isPreview;
            set
            {
                var layer = value ? PhysicsLayersUtility.UnitPreviewLayer : PhysicsLayersUtility.UnitLayer;
                gameObject.layer = LayerMask.NameToLayer(layer);
            }
        }

        //private void OnEnable()
        //{
        //    characterController = GetComponent<CharacterController>();
        //}
    }
}