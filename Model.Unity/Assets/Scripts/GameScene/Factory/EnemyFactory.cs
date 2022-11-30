using Enemy;

namespace Factory
{
    public abstract class EnemyFactory : BaseFactory
    {
        /// <summary>
        /// 生成(從物件池取出)
        /// </summary>
        internal abstract void SpawnFromPool<T>(T data) where T : EnemyData;
    }
}