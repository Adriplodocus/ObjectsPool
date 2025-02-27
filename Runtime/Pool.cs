using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.ObjectPool
{
    [RequireComponent(typeof(PoolBase))]
    public abstract class Pool<T, INFO> : MonoBehaviour where T : Component where INFO : class
    {
        protected ObjectPool<T> m_pool;
        private PoolBase m_poolBase;

        [SerializeField] private T m_prefab;

        [SerializeField] private bool m_collectionCheck;
        [SerializeField] private int m_size;
        [SerializeField] private int m_maxSize;

        private List<T> m_elementsGot = new List<T>();

        public IReadOnlyList<T> elementsGot => m_elementsGot;

        private void OnValidate()
        {
            GetBasePool();
        }

        private void GetBasePool()
        {
            if (m_poolBase == null)
                m_poolBase = GetComponent<PoolBase>();
        }

        private void Awake()
        {
            GetBasePool();

            m_pool = new ObjectPool<T>(
                Create,
                OnGet,
                OnRelease,
                null,
                m_collectionCheck,
                m_size,
                m_maxSize);

            GenerateInitialElements();
        }

        private void GenerateInitialElements()
        {
            for (int i = 0; i < m_size; i++)
                Release(Create());
        }

        private T Create()
        {
            return Instantiate(m_prefab);
        }

        private void OnGet(T obj)
        {
            if (m_poolBase.onGetContainer)
                obj.gameObject.transform.SetParent(m_poolBase.onGetContainer);

            obj.transform.localPosition = Vector3.zero;
            m_elementsGot.Add(obj);

            PooledObject<T, INFO> pooledObject = obj as PooledObject<T, INFO>;

            if (pooledObject != null)
                pooledObject.SetState(PooledObject<T, INFO>.State.Got);
        }

        private void Release(T obj)
        {
            m_pool.Release(obj);

            PooledObject<T, INFO> pooledObject = obj as PooledObject<T, INFO>;

            if (pooledObject != null)
            {
                pooledObject.SetState(PooledObject<T, INFO>.State.Released);
                pooledObject.ResetValues();
            }
        }

        private void OnRelease(T obj)
        {
            if (m_poolBase.onReleasedContainer)
                obj.gameObject.transform.SetParent(m_poolBase.onReleasedContainer);

            obj.transform.localPosition = Vector3.zero;
            obj.gameObject.SetActive(false);

            m_elementsGot.Remove(obj);
        }

        public T Generate(INFO info, bool active = true)
        {
            T item = m_pool.Get();

            PooledObject<T, INFO> pooledObject = item as PooledObject<T, INFO>;

            if (pooledObject == null)
            {
                Debug.LogError($"Pooled object generation error!");
                return item;
            }

            item.gameObject.SetActive(active);
            pooledObject.ProvidePool(m_pool);
            pooledObject.ProvideInfo(info);

            pooledObject.Generated();

            OnItemGenerated(item);

            return item;
        }

        protected virtual void OnItemGenerated(T item)
        {
        }

        public void ReleaseAll()
        {
            for (int i = m_elementsGot.Count - 1; i >= 0; i--)
            {
                m_pool.Release(m_elementsGot[i]);
            }
        }

        public void SetCustomReleasedContainer(Transform container)
        {
            GetBasePool();
            m_poolBase.CustomReleasedContainer(container);
        }

        public void SetCustomGetContainer(Transform container)
        {
            GetBasePool();
            m_poolBase.CustomGetContainer(container);
        }
    }
}