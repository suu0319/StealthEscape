using UnityEngine;

namespace Manager
{
    public class ManagerGenerator : MonoBehaviour
    {
        [Header("GameObject")]
        [SerializeField]
        private GameObject _gameManager;
        [SerializeField]
        private GameObject _canvas;

        private static bool isHaveClone = false;

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初始化生成
        /// </summary>
        private void Init()
        {
            if (!isHaveClone) 
            {
                isHaveClone = true;

                GenerateManager(_gameManager);
                GenerateManager(_canvas);
            }
        }

        /// <summary>
        /// 生成Manager(若不存在)
        /// </summary>
        private void GenerateManager(GameObject gameObject)
        {
            var temp = Instantiate(gameObject);
            temp.name = gameObject.name;
        }
    }
}