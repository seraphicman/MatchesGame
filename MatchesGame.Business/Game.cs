using System;

namespace MatchesGame.Business
{
    using System.Linq;

    using MatchesGame.Business.IBusiness;

    public class Game : IGame
    {
        private readonly int[] originalState;
        private readonly int[] currentState;
        private int currentPlayer;
        private int currentLine;
        private int winner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class. 
        /// </summary>
        /// <param name="initialState">
        /// initial item state
        /// </param>
        /// <param name="startPlayer">
        /// start from 2P
        /// </param>
        public Game(int[] initialState, int startPlayer)
        {
            Game.CheckInitialState(initialState);
            Game.CheckStartPlayer(startPlayer);

            this.currentPlayer = startPlayer;
            this.currentState = initialState;
            this.originalState = new int[initialState.Length];
            Array.Copy(this.currentState, this.originalState, this.currentState.Length);
        }

        public bool Choose(int line, int count, out string errorMsg)
        {
            errorMsg = string.Empty;

            if (this.GameOver())
            {
                errorMsg = $"游戏已结束，胜利者为：{this.Winner()}";
                return false;
            }

            if (line <= 0 || line > this.currentState.Length)
            {
                errorMsg = $"行号需要在[1~{this.currentState.Length}]之间（您选择行号为：{line}）";
                return false;
            }

            // 多次选择的话，需要保证选择是在同一行进行的
            if (this.currentLine > 0 && line != this.currentLine)
            {
                errorMsg = $"已经在第{this.currentLine}进行了选择，不可以再选其他行的物品了。（您选择行号为：{line}）";
                return false;
            }

            if (count <= 0)
            {
                errorMsg = $"选取的数量必须大于0（您选取数量为：{count}）";
                return false;
            }

            if (count > this.currentState[line - 1])
            {
                errorMsg = $"当前行最多可以选取的数量为{this.currentState[line - 1]}（您选取数量为：{count}）";
                return false;
            }

            this.currentState[line - 1] -= count;
            this.currentLine = line;

            return true;
        }

        public bool Confirm(out string errorMsg)
        {
            errorMsg = string.Empty;

            if (this.GameOver())
            {
                errorMsg = $"游戏已结束，胜利者为：{this.Winner()}";
                return false;
            }

            // 未做任何选择，不可以确认
            if (this.currentLine == 0)
            {
                errorMsg = "还未做任何选择，不可以确认选择操作";
                return false;
            }

            if (Game.TotalCount(this.currentState) == 0)
            {
                this.winner = this.currentPlayer == 1 ? 2 : 1;
            }
            else if (Game.TotalCount(this.currentState) == 1)
            {
                this.winner = this.currentPlayer;
            }

            this.currentPlayer = this.currentPlayer == 1 ? 2 : 1;
            this.currentLine = 0;
            Array.Copy(this.currentState, this.originalState, this.currentState.Length);

            return true;
        }

        public bool Reset(out string errorMsg)
        {
            errorMsg = string.Empty;
            
            if (this.GameOver())
            {
                errorMsg = $"游戏已结束，胜利者为：{this.Winner()}";
                return false;
            }

            Array.Copy(this.originalState, this.currentState, this.currentState.Length);

            return true;
        }

        public bool GameOver()
        {
            return this.winner > 0;
        }

        public int Winner()
        {
            return this.winner;
        }

        public int[] CurrentState()
        {
            return this.currentState;
        }

        public int CurrentPlayer()
        {
            return this.currentPlayer;
        }

        /// <summary>
        /// 获得物品总数量
        /// </summary>
        /// <param name="state">物品状态</param>
        /// <returns>物品数量</returns>
        private static int TotalCount(int[] state)
        {
            if (state == null)
            {
                return 0;
            }

            return state.Sum(c => c);
        }

        /// <summary>
        /// 检查物品初始状态
        /// </summary>
        /// <param name="initialState">物品初始状态</param>
        /// <exception cref="Exception">错误信息</exception>
        private static void CheckInitialState(int[] initialState)
        {
            // 确保初始化参数
            if (initialState == null || initialState.Length == 0)
            {
                throw new Exception("初始状态不能为空");
            }

            for (var i = 0; i < initialState.Length; i++)
            {
                if (initialState[i] <= 0)
                {
                    throw new Exception($"第{i}行物品数量不能为0或负数（当前数量：{initialState[i]})");
                }
            }

            var totalCount = Game.TotalCount(initialState);
            if (totalCount == 1)
            {
                throw new Exception($"物品总数量必须大于1（当前总数量：{totalCount}）");
            }
        }

        /// <summary>
        /// 检查开始玩家编号
        /// </summary>
        /// <param name="startPlayer">开始玩家编号</param>
        /// <exception cref="Exception">错误信息</exception>
        private static void CheckStartPlayer(int startPlayer)
        {
            if (startPlayer != 1 && startPlayer != 2)
            {
                throw new Exception("开始玩家只能是1或者2");
            }
        }
    }
}
