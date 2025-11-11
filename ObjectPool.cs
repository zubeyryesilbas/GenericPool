 public class ObjectPool<T> where T : Component
    {
        private readonly Queue<T> _pool = new();
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, Transform parent, int initialSize = 0)
        {
            _prefab = prefab;
            _parent = parent;

            // Prewarm (optional)
            for (int i = 0; i < initialSize; i++)
            {
                var instance = CreateNew();
                ReturnToPool(instance);
            }
        }

        private T CreateNew()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(false);
            return obj;
        }

        public T Get()
        {
            var obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNew();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parent, false);
            _pool.Enqueue(obj);
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
