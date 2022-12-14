using System.Collections.Generic;

public interface IExposureSubject
{
    public List<IExposureObserver> EnemyList { get; set; }

    /// <summary>
    /// 增加敵人觀察者
    /// </summary>
    /// <param name="observer">觀察者</param>
    public void AddEnemyObserver(IExposureObserver observer);

    /// <summary>
    /// 刪除敵人觀察者
    /// </summary>
    /// <param name="observer">觀察者</param>
    public void RemoveEnemyObserver(IExposureObserver observer);

    /// <summary>
    /// 玩家被發現
    /// </summary>
    public void PlayerExposure();
}