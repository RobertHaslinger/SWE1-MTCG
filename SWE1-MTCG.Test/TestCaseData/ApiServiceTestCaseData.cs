using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG.Api;
using SWE1_MTCG.Server;

namespace SWE1_MTCG.Test.TestCaseData
{
    public class ApiServiceTestCaseData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new NUnit.Framework.TestCaseData("/messages/").Returns(typeof(MessageApi));
                yield return new NUnit.Framework.TestCaseData("/battles").Returns(typeof(BattleApi));
                yield return new NUnit.Framework.TestCaseData("/stats").Returns(typeof(StatisticApi));
            }
        }

        public static IEnumerable ExceptionalTestCases
        {
            get
            {
                yield return new NUnit.Framework.TestCaseData("users");
                yield return new NUnit.Framework.TestCaseData("/userslol");
                yield return new NUnit.Framework.TestCaseData("/notExistingApi");
            }
        }
    }
}
