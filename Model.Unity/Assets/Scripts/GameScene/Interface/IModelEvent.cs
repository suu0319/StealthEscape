public interface IModelEvent
{
    /// <summary>
    /// 播放攻擊音效
    /// </summary>
    public void PlayAttackSFX();

    /// <summary>
    /// 播放死亡音效
    /// </summary>
    public void PlayDeathSFX();

    /// <summary>
    /// 判定攻擊(Animation Event)
    /// </summary>
    public void DetectAttack();

    /// <summary>
    /// 判斷死亡
    /// </summary>
    public void DetectDeath();
}