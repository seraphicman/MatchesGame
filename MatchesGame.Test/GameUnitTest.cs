using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MatchesGame.Test
{
    using MatchesGame.Business;
    using MatchesGame.Business.IBusiness;

    [TestClass]
    public class GameUnitTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            try
            {
                new Game(null, 0);
                Assert.Fail("创建实体类应该失败");
            }
            catch
            {
                // 正常
            }

            try
            {
                new Game(new int[0], 0);
                Assert.Fail("创建实体类应该失败");
            }
            catch
            {
                // 正常
            }

            try
            {
                new Game(new[] { 1 }, 0);
                Assert.Fail("创建实体类应该失败");
            }
            catch
            {
                // 正常
            }

            try
            {
                new Game(new[] { 1, -1 }, 0);
                Assert.Fail("创建实体类应该失败");
            }
            catch
            {
                // 正常
            }

            try
            {
                new Game(new[] { 1, 1 }, 0);
                Assert.Fail("创建实体类应该失败");
            }
            catch
            {
                // 正常
            }

            try
            {
                new Game(new[] { 1, 1 }, 1);
            }
            catch
            {
                Assert.Fail("创建实体类应该成功");
            }

            try
            {
                new Game(new[] { 1, 1 }, 2);
            }
            catch
            {
                Assert.Fail("创建实体类应该成功");
            }
        }

        [TestMethod]
        public void ChooseTest()
        {
            IGame game = new Game(new[] {1, 1}, 1);
            string errorMsg;

            Assert.IsFalse(game.Choose(0, 0, out errorMsg));
            Assert.IsFalse(game.Choose(3, 0, out errorMsg));
            Assert.IsFalse(game.Choose(1, 0, out errorMsg));
            Assert.IsFalse(game.Choose(1, 2, out errorMsg));
            Assert.IsTrue(game.Choose(1, 1, out errorMsg));
            Assert.AreEqual(0, game.CurrentState()[0]);
            Assert.IsFalse(game.Choose(2, 1, out errorMsg));
        }

        [TestMethod]
        public void ConfirmTest()
        {
            IGame game = new Game(new[] { 2, 2 }, 1);
            string errorMsg;

            Assert.IsFalse(game.Confirm(out errorMsg));
            Assert.IsTrue(game.Choose(1, 2, out errorMsg));
            Assert.IsTrue(game.Confirm(out errorMsg));
            Assert.AreEqual(0, game.Winner());
            Assert.AreEqual(2, game.CurrentPlayer());
            Assert.IsTrue(game.Choose(2, 1, out errorMsg));
            Assert.IsTrue(game.Confirm(out errorMsg));
            Assert.IsTrue(game.GameOver());
            Assert.AreEqual(2, game.Winner());

            game = new Game(new[] { 2 }, 1);
            Assert.IsTrue(game.Choose(1, 2, out errorMsg));
            Assert.IsTrue(game.Confirm(out errorMsg));
            Assert.IsTrue(game.GameOver());
            Assert.AreEqual(2, game.Winner());

            game = new Game(new[] { 2 }, 2);
            Assert.IsTrue(game.Choose(1, 2, out errorMsg));
            Assert.IsTrue(game.Confirm(out errorMsg));
            Assert.IsTrue(game.GameOver());
            Assert.AreEqual(1, game.Winner());
        }

        [TestMethod]
        public void ResetTest()
        {
            IGame game = new Game(new[] { 2, 2 }, 1);
            string errorMsg;

            Assert.IsTrue(game.Choose(1, 2, out errorMsg));
            Assert.AreEqual(0, game.CurrentState()[0]);
            game.Reset();
            Assert.AreEqual(2, game.CurrentState()[0]);

            Assert.IsTrue(game.Choose(1, 2, out errorMsg));
            Assert.IsTrue(game.Confirm(out errorMsg));
            Assert.IsTrue(game.Choose(2, 2, out errorMsg));
            Assert.AreEqual(0, game.CurrentState()[1]);
            game.Reset();
            Assert.AreEqual(2, game.CurrentState()[1]);

        }
    }
}
