using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    [System.Serializable]
    public class Pool
    {
        public string Name;
        public int OriginAmount;
       
        public GameObject Prefab;
        public GameObject ParentObj;

        public Queue<GameObject> ObjQueue;
    }

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance;

        [SerializeField]
        private List<Pool> poolList;
        private Dictionary<string, Pool> poolDict;

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
            poolDict = new Dictionary<string, Pool>();

            foreach (Pool pool in poolList)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.OriginAmount; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.transform.parent = pool.ParentObj.transform;
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                pool.ObjQueue = objectPool;

                poolDict.Add(pool.Name, pool);
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

            GameObject obj;

            if (poolDict[name].ObjQueue.Count == 0)
            {
                obj = Instantiate(poolDict[name].Prefab);
                obj.transform.parent = poolDict[name].ParentObj.transform;
            }
            else 
            {
                obj = poolDict[name].ObjQueue.Dequeue();           
            }

            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }

        /// <summary>
        /// 回收物件
        /// </summary>
        /// <param name="name">物件名稱</param>
        /// <param name="gameObject">物件</param>
        internal void RecycleToPool(string name, GameObject gameObject)
        {
            poolDict[name].ObjQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
    }
}