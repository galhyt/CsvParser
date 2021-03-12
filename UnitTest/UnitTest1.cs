using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvParser
{
    [TestClass]
    public class CsvParserUnitTest
    {
        [TestMethod]
        public void TestCount()
        {
            CsvParser csv = new CsvParser("myfile.csv");
            Assert.AreEqual(4, csv.Count);
        }

        [TestMethod]
        public void TestEmpty()
        {
            CsvParser csv = new CsvParser("myfile.csv");
            Assert.AreEqual(false, csv.IsEmpty);
        }

        [TestMethod]
        public void TestWhereEquals()
        {
            CsvParser csv = new CsvParser("myfile.csv");
            var res = csv.WhereEquals("result", "OK");
            bool match = true;
            if (res != null)
            {
                if (res.Count != 1) match = false;
                if (!IsMatchExpected(res[0], 0))
                    match = false;
            }
            Assert.AreEqual(true, match);
        }

        [TestMethod]
        public void TestWhereGreaterThan()
        {
            CsvParser csv = new CsvParser("myfile.csv");
            var res = csv.WhereGreaterThan("status", 200);
            TestWhereMethod(res, 3, new int[] { 1, 2, 3 });
        }

        [TestMethod]
        public void TestWhereLessThan()
        {
            CsvParser csv = new CsvParser("myfile.csv");
            var res = csv.WhereLessThan("status", 500);
            TestWhereMethod(res, 3, new int[] { 0, 1, 2 });
        }

        private void TestWhereMethod(List<CsvRow> res, int expectedCount, int[] expectedIndxs)
        {
            bool match = true;
            if (res != null)
            {
                if (res.Count != expectedCount)
                    match = false;
                else
                {
                    int i = 0;
                    foreach (CsvRow row in res)
                    {
                        if (!IsMatchExpected(row, expectedIndxs[i]))
                        {
                            match = false;
                            break;
                        }
                        if (i >= expectedIndxs.Length) break;

                        i++;
                    }
                }
            }
            Assert.AreEqual(true, match);
        }

        private bool IsMatchExpected(CsvRow row, int expectedRowIndx)
        {
            return row["time"] == fileExpectedData[expectedRowIndx]["time"] && row["status"] == fileExpectedData[expectedRowIndx]["status"] && row["result"] == fileExpectedData[expectedRowIndx]["result"];
        }

        private List<Dictionary<string, string>> fileExpectedData = new List<Dictionary<string, string>>
        {
            new Dictionary<string,string> { { "time", "18:05" }, { "status",  "200" }, {"result", "OK" } },
            new Dictionary<string,string> { { "time", "18:02" }, { "status", "302" }, {"result", "Moved Permanently" } },
            new Dictionary<string,string> { { "time", "18:02" }, { "status", "404" }, {"result", "Not Found" } },
            new Dictionary<string,string> { { "time", "18:04" }, { "status", "500" }, {"result", "Internal Server Error" } }
        };
    }
}
