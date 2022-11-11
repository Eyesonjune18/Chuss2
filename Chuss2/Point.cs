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

    public bool IsOutOfBounds() => X is < 0 or > 7 || Y is < 0 or > 7;
    // Checks if the Point is outside of the normal 8x8 chess board boundaries
    // Not built into the accessors or constructor for flexibility purposes

    public bool HasSameCoords(Point p) => X == p.X && Y == p.Y;
    // Checks if this Point has the same coordinates as another given Point

}