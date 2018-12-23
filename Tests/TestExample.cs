using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace Tests
{
    [UseReporter(typeof(DiffReporter))]
    [TestClass]
    public class TestExample
    {
        [TestMethod]
        public void TestMethod1()
        {
            Approvals.Verify(1);
        }
    }
}
