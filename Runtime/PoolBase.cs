using UnityEngine;

namespace Game.ObjectPool
{
    public class PoolBase : MonoBehaviour
    {
        [SerializeField] private bool m_fixedReleasedContainer = true;
        [SerializeField] private Transform m_onReleasedContainer;
        public Transform onReleasedContainer => m_fixedReleasedContainer ? m_onReleasedContainer : null;

        [SerializeField] private bool m_fixedGetContainer = true;
        [SerializeField] private Transform m_onGetContainer;
        public Transform onGetContainer => m_fixedGetContainer ? m_onGetContainer : null;

        /// <summary>
        /// Set a custom and permanent container for released objects.
        /// </summary>
        public void CustomReleasedContainer(Transform container)
        {
            m_fixedReleasedContainer = true;
            m_onReleasedContainer = container;
        }

        /// <summary>
        /// Set a custom and permanent container for objects that are got from the pool.
        /// </summary>
        /// <param name="container"></param>
        public void CustomGetContainer(Transform container)
        {
            m_fixedGetContainer = true;
            m_onGetContainer = container;
        }
    }
}