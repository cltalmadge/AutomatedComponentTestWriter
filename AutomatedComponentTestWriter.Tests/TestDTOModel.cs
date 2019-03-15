using System;
using AutomatedComponentTestWriter.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomatedComponentTestWriter.Tests
{
    [TestClass]
    public class TestDTOModel
    {
        [TestMethod]
        public void TestMethodNull()
        {
            ComponentTestDTO dto = new ComponentTestDTO();
            Assert.IsNotNull(dto);
        }
    }
}
