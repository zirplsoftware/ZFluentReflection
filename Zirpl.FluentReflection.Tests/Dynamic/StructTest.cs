// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructTest.cs" company="WebOS - http://www.coolwebos.com">
//   Copyright © Dixin 2010 http://weblogs.asp.net/dixin
// </copyright>
// <summary>
//   Defines the StructTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Zirpl.FluentReflection.Tests
{
    internal struct StructTest
    {
        #region Constants and Fields

        private int _value;

        #endregion

        #region Constructors and Destructors

        internal StructTest(int value)
        {
            this._value = value;
        }

        #endregion

        #region Properties

        internal int Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;
            }
        }

        #endregion
    }
}