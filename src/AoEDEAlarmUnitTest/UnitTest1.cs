﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoEDEAlarmUnitTest {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            string result = AoEDEAlarm.ConstValues.TessdataPath;
            Assert.AreNotEqual("", result);
        }
    }
}
