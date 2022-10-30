using System.Text;

namespace Chuss2;

public class Point
{
    
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    // Main constructor for Point, just takes an X and Y value
    // X and Y may also be known as "column" and "row"
    {

        X = x;
        Y = y;

    }

    public string ToAlgebraicNotation()
    // Returns coordinate in algebraic notation
    {

        StringBuilder algebraicCoord = new StringBuilder();
        algebraicCoord.Append(Y + 'a');
        algebraicCoord.Append(X + 1);

        return algebraicCoord.ToString();

    }

    public bool IsOutOfBounds()
    // Returns true if the Point is outside of the normal 8x8 chess board boundaries
    // Not built into the accessors or constructor for flexibility purposes
    {

        return ((X is < 0 or > 7) || (Y is < 0 or > 7));

    }
    
}
