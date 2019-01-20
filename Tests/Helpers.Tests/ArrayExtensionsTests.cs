using System;
using System.Collections.Generic;
using Helpers.Extensions;
using NUnit.Framework;

namespace Helpers.Tests
{

    [ TestFixture ]
    public class ArrayExtensionsTests
    {
        [ Test ]
        public void ToList_ArrayIsNull_Throws ()
        {
            var array = ( int[,] )null;

            Assert.Catch< NullReferenceException >( () => array.ToList() );
        }

        [ Test ]
        public void ToList_ArrayIsEmpty_ReturnsEmptyList ()
        {
            var array = new int[0, 0];

            var list = array.ToList();

            Assert.That( 0 == list.Count );
        }

        [ Test ]
        public void ToList_ArrayIsFilled_ReturnsExpected ()
        {
            var array = new[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var expectedList = new List< int > { 1, 2, 3, 4, 5, 6 };

            var list = array.ToList();

            Assert.That( array, Is.EquivalentTo( expectedList ) );
        }
    }
}
