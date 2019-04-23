using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore3Sample.Tests
{
    [TestClass()]
    public class DistRuleDefDBTests
    {
        [TestMethod()]
        public void SelectTest()
        {
            AppConfigDAL dal = new AppConfigDAL();
            AppConfig config = dal.Select(1, "CONFIG_UPD");

            if (config == null)
                Assert.Fail("Record does not exist");
            else
                Assert.AreEqual(config.ConfigValue, "N");
        }
    }
}
