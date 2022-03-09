namespace MatchesGame.Business.IBusiness
{
    /// <summary>
    /// 游戏接口
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// 选择物品
        /// </summary>
        /// <param name="line">行号</param>
        /// <param name="count">选择数量</param>
        /// <param name="errorMsg">错误信息</param>
        /// <returns>选择是否成功</returns>
        bool Choose(int line, int count, out string errorMsg);

        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        /// <returns>选择是否成功</returns>
        bool Confirm(out string errorMsg);

        /// <summary>
        /// 重置游戏状态
        /// </summary>
        /// <param name="errorMsg">错误信息</param>
        /// <returns>选择是否成功</returns>
        bool Reset(out string errorMsg);

        /// <summary>
        /// 游戏是否结束
        /// </summary>
        /// <returns>游戏是否结束</returns>
        bool GameOver();

        /// <summary>
        /// 胜利者
        /// </summary>
        /// <returns>胜利者编号，0表示未分胜负，1表示P1胜利，2表示P2胜利</returns>
        int Winner();

        /// <summary>
        /// 当前物品状态
        /// </summary>
        /// <returns>物品状态</returns>
        int[] CurrentState();

        /// <summary>
        /// 当前玩家
        /// </summary>
        /// <returns>玩家编号，1表示P1，2表示P2</returns>
        int CurrentPlayer();
    }
}
