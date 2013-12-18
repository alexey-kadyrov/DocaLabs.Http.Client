using System;
using DocaLabs.Http.Client.Utils;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [TestClass]
    public class when_checking_whenever_a_null_value_is_null
    {
        static NullComparerOverride _comparer;
        static bool _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _comparer = new NullComparerOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _comparer.IsNull(null);
        }

        [TestMethod]
        public void it_should_return_true()
        {
            _result.ShouldBeTrue();
        }
    }

    [TestClass]
    public class when_checking_whenever_the_db_null_value_is_null
    {
        static NullComparerOverride _comparer;
        static bool _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _comparer = new NullComparerOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _comparer.IsNull(DBNull.Value);
        }

        [TestMethod]
        public void it_should_return_true()
        {
            _result.ShouldBeTrue();
        }
    }

    [TestClass]
    public class when_checking_whenever_a_non_null_value_is_null
    {
        static NullComparerOverride _comparer;
        static bool _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _comparer = new NullComparerOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _comparer.IsNull(new object());
        }

        [TestMethod]
        public void it_should_return_false()
        {
            _result.ShouldBeFalse();
        }
    }

    [TestClass]
    public class when_checking_whenever_a_value_of_value_type_is_null
    {
        static NullComparerOverride _comparer;
        static bool _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _comparer = new NullComparerOverride();

            BecauseOf();
        }

        static void BecauseOf()
        {
            _result = _comparer.IsNull(42);
        }

        [TestMethod]
        public void it_should_return_false()
        {
            _result.ShouldBeFalse();
        }
    }
}
