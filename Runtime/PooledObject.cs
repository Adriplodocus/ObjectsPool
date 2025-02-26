using UnityEngine;
using UnityEngine.Pool;

namespace Game.ObjectPool
{
    public abstract class PooledObject<T, INFO> : MonoBehaviour where T : Component where INFO : class
    {
        private ObjectPool<T> m_originPool;
        public INFO info { get; private set; }

        public enum State
        {
            Released,
            Got,
        }

        public State state { get; private set; }

        public void ProvidePool(ObjectPool<T> pool)
        {
            m_originPool = pool;
        }

        public void ProvideInfo(INFO _info)
        {
            info = _info;
        }

        public virtual void Generated()
        {
        }

        protected void Release()
        {
            ResetValues();

            if (m_originPool != null)
                m_originPool.Release(this as T);
            else
                Destroy(gameObject);
        }

        /// <summary>
        /// As a pooled object, you should reset all values to default.
        /// This method is called when releasing the PooledObject to the pool.
        /// </summary>
        public abstract void ResetValues();

        public void SetState(State _state)
        {
            state = _state;
        }
    }
}