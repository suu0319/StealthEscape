using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [System.Serializable]
    public class Pool
    {
        public string Name;
        public int Amount;
       
        public GameObject Prefab;
        public GameObject ParentObj;
    }

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;

        [SerializeField]
        private List<Pool> poolList;
        private Dictionary<string, Queue<GameObject>> poolDict;

        private void Awake()
        {
            InitSingleton();
        }

        /// <summary>
        /// Singleton初始化
        /// </summary>
        private void InitSingleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 初始化物件池
        /// </summary>
        internal void InitObjectPool()
        {
            poolDict = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in poolList)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.Amount; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.transform.parent = pool.ParentObj.transform;
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDict.Add(pool.Name, objectPool);
            }
        }

        /// <summary>
        /// 生成物件
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="position">座標</param>
        /// <param name="rotation">旋轉</param>
        /// <returns></returns>
        internal GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation)
        {
            if (!poolDict.ContainsKey(name))
            {
                Debug.LogError("Pool with name doest't exist");
                return null;
            }

            if (poolDict[name].Count == 0)
            {
                foreach (Pool pool in poolList)
                {
                    if (pool.Name == name) 
                    {
                        GameObject obj = Instantiate(pool.Prefab);
                        obj.transform.parent = pool.ParentObj.transform;
                        obj.SetActive(false);
                        poolDict[name].Enqueue(obj);
                        pool.Amount++;
                    }
                }
            }

            GameObject objectToSpawn = poolDict[name].Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }

        /// <summary>
        /// 回收物件
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="gameObject">物件</param>
        internal void RecycleToPool(string name, GameObject gameObject)
        {
            poolDict[name].Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}