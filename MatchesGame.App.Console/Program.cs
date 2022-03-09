using System;

namespace MatchesGame.App.Console
{
    using MatchesGame.Business;
    using MatchesGame.Business.IBusiness;

    using Console = System.Console;

    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("游戏规则介绍：");
            Console.WriteLine("将15根牙签分成三行，每行自上而下（其实方向不限）分别是3、5、7根");
            Console.WriteLine("安排两个玩家，每人可以在一轮内，在任意行拿任意根牙签，但不能跨行");
            Console.WriteLine("拿最后一根牙签的人即为输家");
            Console.WriteLine();
            Console.WriteLine("游戏正式开始：");

            try
            {
                IGame game = new Game(new[] {3, 5, 7}, 1);

                while (!game.GameOver())
                {
                    var confirmed = false;

                    while (!confirmed)
                    {
                        var chooseSuccess = false;
                        string errorMsg;
                        while (!chooseSuccess)
                        {
                            Console.WriteLine();
                            Console.WriteLine("请玩家{0}开始选择", game.CurrentPlayer());
                            Program.PrintState(game.CurrentState());
                            var line = Program.GetInputNumber($"请输入玩家{game.CurrentPlayer()}选择的行号：");
                            var count = Program.GetInputNumber($"请输入玩家{game.CurrentPlayer()}选择的牙签数量：");

                            chooseSuccess = game.Choose(line, count, out errorMsg);

                            if (!chooseSuccess)
                            {
                                Console.WriteLine("选择失败，请重新选择。失败原因：{0}", errorMsg);
                            }
                        }

                        Program.PrintState(game.CurrentState());
                        Console.Write("是否确认选择（Y/N)：");
                        var confirmStr = Console.ReadLine();

                        if (confirmStr?.ToLower() == "y")
                        {
                            if (!game.Confirm(out errorMsg))
                            {
                                Console.WriteLine("确认失败，请继续选择。失败原因：{0}", errorMsg);
                                continue;
                            }

                            confirmed = true;
                        }
                        else
                        {
                            if (!game.Reset(out errorMsg))
                            {
                                Console.WriteLine("重置失败，请继续选择。失败原因：{0}", errorMsg);
                            }
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("游戏结束，恭喜玩家{0}获得胜利！", game.Winner());
            }
            catch (Exception ex)
            {
                Console.WriteLine("未知错误：" + ex.Message);
            }

            Console.ReadKey();
        }

        private static int GetInputNumber(string hint)
        {
            var success = false;
            var number = 0;

            while (!success)
            {
                Console.Write(hint);
                var str = Console.ReadLine();

                success = int.TryParse(str, out number);

                if (!success)
                {
                    Console.WriteLine("输入内容必须是整数。请重新输入。");
                }
            }

            return number;
        }

        private static void PrintState(int[] state)
        {
            Console.WriteLine();
            Console.WriteLine("现在的牙签状态是：");

            if (state == null)
            {
                return;
            }

            for (var i = 0; i < state.Length; i++)
            {
                Console.Write($"第{i + 1}行：");

                for (var j = 0; j < state[i]; j++)
                {
                    Console.Write("* ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
