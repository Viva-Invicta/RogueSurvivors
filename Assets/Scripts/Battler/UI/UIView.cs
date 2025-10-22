using UnityEngine;

namespace AutoBattler
{
    public abstract class UIView : MonoBehaviour
    {
        public abstract UIViewType ViewType { get; }

        public virtual void Initialize(ServiceLocator serviceLocator) { }
        public virtual void Release() { }
    }
}