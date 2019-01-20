using System.Collections.Generic;

namespace Helpers.Extensions
{
    public static class ArrayExtensions
    {
        public static List< T > ToList< T > ( this T[,] array )
        {
            var rowCount = array.GetLength( 0 );
            var columnCount = array.GetLength( 1 );

            var list = new List<T>( rowCount * columnCount );

            for ( int i = 0; i < rowCount; i++ ) {
                for ( int j = 0; j < columnCount; j++ ) {
                    list.Add( array[ i, j ] );
                }
            }

            return list;
        }
    }
}
