using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UIService : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        private ServiceLocator serviceLocator;
        private Dictionary<UIViewType, UIView> activeViews = new Dictionary<UIViewType, UIView>();

        public void Initialize(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public bool TryGetView<T>(UIViewType viewType, out T view)
            where T: UIView
        {
            if (activeViews.TryGetValue(viewType, out var value))
            {
                view = value.GetComponent<T>();

                if (view)
                {
                    return true;
                }
                else
                {
                    Debug.LogError($"{nameof(UIService)} : could not get component of view with type {viewType}");
                }
            }

            view = default;
            return false;
        }

        public T CreateOrShowView<T>(UIViewType viewType, Transform parent = default)
            where T: UIView
        {
            if (activeViews.TryGetValue(viewType, out var view))
            {
                if (view.TryGetComponent<T>(out var viewComponent))
                {
                    view.gameObject.SetActive(true);

                    return view.GetComponent<T>();
                }

                Debug.LogError($"{nameof(UIService)} : could not get component of view with type {viewType} in dictionary; Replacing");
                Destroy(view.gameObject);
                activeViews.Remove(viewType);
            }

            var viewPrefab = serviceLocator.UIPrefabsService.GetUIPrefabByType(viewType);

            if (!viewPrefab)
            {
                return default;
            }

            var viewInstance = Instantiate(viewPrefab).GetComponent<T>();

            if (!viewInstance)
            {
                Debug.LogError($"{nameof(UIService)} : could not get component of view with type {viewType}");
                Destroy(viewInstance);

                return default;
            }

            var viewParent = parent ? parent : canvas.transform;

            viewInstance.transform.SetParent(viewParent, false);
            viewInstance.gameObject.SetActive(true);

            activeViews.Add(viewType, viewInstance);

            return viewInstance;
        }


        public void HideView(UIViewType viewType)
        {
            if (activeViews.TryGetValue(viewType, out var view))
            {
                view.gameObject.SetActive(false);
                return;
            }

            Debug.LogError($"{nameof(UIService)} : could not get find active view with type {viewType}; nothing to hide");
        }

        public void DestroyView(UIViewType viewType)
        {
            if (activeViews.TryGetValue(viewType, out var view))
            {
                Destroy(view.gameObject);
                activeViews.Remove(viewType);

                return;
            }

            Debug.LogError($"{nameof(UIService)} : could not get find active view with type {viewType}; nothing to destroy");
        }
    }
}