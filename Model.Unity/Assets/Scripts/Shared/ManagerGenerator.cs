using UnityEngine;

namespace Manager 
{
    public class ManagerGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameManager;

        [SerializeField]
        private GameObject _canvas;

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初始化生成
        /// </summary>
        private void Init() 
        {
            GenerateManager(_gameManager);
            GenerateManager(_canvas);
        }

        /// <summary>
        /// 生成Manager(若不存在)
        /// </summary>
        private void GenerateManager(GameObject gameObject)
        {
            if (!GameObject.Find(gameObject.name))
            {
                var temp = Instantiate(gameObject);
                temp.name = gameObject.name;
            }
        }
    }
}