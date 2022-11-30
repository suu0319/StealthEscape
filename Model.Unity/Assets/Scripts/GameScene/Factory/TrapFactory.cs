using Trap;

namespace Factory
{
    public abstract class TrapFactory : BaseFactory
    {
        /// <summary>
        /// 生成(從物件池取出)
        /// </summary>
        internal abstract void SpawnFromPool<T>(T data) where T : TrapData;
    }
}