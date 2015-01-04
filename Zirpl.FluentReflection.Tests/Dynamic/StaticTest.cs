// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticTest.cs" company="WebOS - http://www.coolwebos.com">
//   Copyright © Dixin 2010 http://weblogs.asp.net/dixin
// </copyright>
// <summary>
//   Defines the StaticTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Zirpl.FluentReflection.Tests
{
    internal class StaticTest
    {
        #region Constants and Fields

        private static int _value;

        #endregion

        #region Properties

        internal static int Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        #endregion

        #region Methods

        internal static int Method()
        {
            return 2;
        }

        #endregion
    }
}