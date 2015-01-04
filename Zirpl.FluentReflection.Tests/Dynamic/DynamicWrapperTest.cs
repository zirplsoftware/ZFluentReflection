using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;

namespace Zirpl.FluentReflection.Tests
{
    [TestFixture]
    public class DynamicWrapperTest
    {
        #region Public Methods

        [Test]
        public void Static_Memebr()
        {
            Assert.AreEqual(0, StaticTest.Value);
            dynamic wrapper = new DynamicWrapper<StaticTest>();

            wrapper._value = 10;
            Assert.AreEqual(10, StaticTest.Value);
            Assert.AreEqual(10, wrapper.Value.ToStatic());

            Assert.AreEqual(2, wrapper.Method().ToStatic());
        }

        [Test]
        public void TryGetIndex_TrySetIndex_From_Type()
        {
            BaseTest @base = new BaseTest();
            Assert.AreEqual("0", @base[5, 5]);
            dynamic wrapper = @base.ToDynamic();
            wrapper[5, 5] = "10";
            Assert.AreEqual("10", @base[5, 5]);
            Assert.AreEqual("10", wrapper[5, 5].ToStatic());
        }

        [Test]
        public void TryGetMember_TryInvokeMember_From_Base()
        {
            Assert.AreEqual(0, new DerivedTest().ToDynamic()._array[6, 6].ToStatic());

            int x = new DerivedTest().ToDynamic()._array[6, 6];
            Assert.AreEqual(0, x);

            Assert.AreEqual(0, new DerivedTest().ToDynamic()._array.ToStatic()[6, 6]);
        }

        //[TestMethod()]
        //public void TryGetMember_TryInvokeMember_TryConvert_From_Type()
        //{
        //    using (NorthwindDataContext database = new NorthwindDataContext())
        //    {
        //        IQueryable<Product> query = database.Products.Where(product => product.ProductID > 0)
        //                                                     .OrderBy(p => p.ProductName).Take(2);
        //        IEnumerable<Product> results =
        //            database.ToDynamic().Provider.Execute(query.Expression).ReturnValue;
        //        Assert.IsTrue(results.Count() > 0);
        //    }
        //}

        [Test]
        public void Value_Type()
        {
            StructTest test2 = new StructTest(10);
            dynamic wrapper2 = new DynamicWrapper<StructTest>(ref test2);
            wrapper2._value = 20;
            Assert.AreEqual(20, wrapper2._value.ToStatic());
            Assert.AreNotEqual(20, test2.Value);
        }

        [Test]
        [ExpectedException(typeof(MissingMemberException))]
        public void Value_Type_Property()
        {
            StructTest test2 = new StructTest(10);
            dynamic wrapper2 = new DynamicWrapper<StructTest>(ref test2);

            wrapper2.Value = 30;
        }

        #endregion
    }
}