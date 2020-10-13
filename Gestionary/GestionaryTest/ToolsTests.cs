using NUnit.Framework;

namespace GestionaryTest
{
    class ToolsTests
    {
        [Test]
        public void ToolsHashPass()
        {
            //check if it crashes
            GestionaryWebsite.Tools.Hash("test");
        }

        [Test]
        public void ToolsRetrievePassFromHash()
        {
            var res = GestionaryWebsite.Tools.CheckPassword("7.kYTCfqouAyxDUSnqJ2q/ag ==.UvaQe2g7gU/VZ3d56hH7hrrgwOOEfi3JRc56XM3InI8=", "test");
            Assert.IsTrue(res);
        }

        [Test]
        public void ToolsRetrievePassFromWrongPass()
        {
            var res = GestionaryWebsite.Tools.CheckPassword("7.kYTCfqouAyxDUSnqJ2q/ag ==.UvaQe2g7gU/VZ3d56hH7hrrgwOOEfi3JRc56XM3InI8=", "admin");
            Assert.IsFalse(res);
        }
    }
}
