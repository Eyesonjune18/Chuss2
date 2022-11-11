using System.Text;

namespace Chuss2;

public static class Utilities
{
 
    public static List<Point> ParseMove(string m)
    {

        string[] algebraicCoords = m.Split(' ');
        if (algebraicCoords.Length != 2)
            throw new ArgumentException("[ERROR] A move must have a source and a destination, and no other arguments",
                nameof(m));
        
        List<Point> srcAndDest = new List<Point>();
        srcAndDest.Add(FromAlgebraicNotation(algebraicCoords[0]));
        // Add the source Point
        srcAndDest.Add(FromAlgebraicNotation(algebraicCoords[1]));
        // Add the destination Point

        return srcAndDest;

    }
    
    public static Point Translate1DCoordTo2D(int coord)
    // Translates a 1D board coordinate to a 2D board coordinate
    // Top-left to bottom-right
    {
        
        return new Point(coord % 8, 7 - coord / 8);

    }
    
    public static string ToAlgebraicNotation(Point p)
    // Translates a given Point into algebraic notation
    {

        StringBuilder algebraicCoord = new StringBuilder();
        algebraicCoord.Append(p.Y + 'a');
        algebraicCoord.Append(p.X + 1);

        return algebraicCoord.ToString();

    }

    private static Point FromAlgebraicNotation(string a)
    // Translates a given algebraic notation string to a Point
    {
        
        if (a.Length != 2)
            throw new ArgumentException(
                "[ERROR] The supplied algebraic coordinate has an incorrect amount of elements", nameof(a));
        
        char[] aChars = a.ToCharArray();

        if (char.ToLower(aChars[0]) is >= 'a' and <= 'h' && char.ToLower(aChars[1]) is >= '1' and <= '8')
            return new Point(aChars[0] - 'a', aChars[1] - '1');
        // If the characters match up with the expected values, return a Point

        throw new ArgumentException("[ERROR] The supplied algebraic coordinate contains invalid characters", nameof(a));

    }
    
    public static List<Point> GetPointsBetween(Point source, Point destination)
    // Returns a list of all tiles between two given Points,
    // assuming that they are either directly vertical, horizontal, or diagonal
    {
        
        List<Point> pointsBetween = new List<Point>();

        int changeX = destination.X - source.X;
        int changeY = destination.Y - source.Y;
        // Calculate the change between the source and destination

        bool vertical = changeX == 0 && changeY != 0;
        bool horizontal = changeX != 0 && changeY == 0;
        bool diagonal = Math.Abs(changeX) == Math.Abs(changeY);

        if (!vertical && !horizontal && !diagonal)
            throw new ArgumentException("[ERROR] The supplied move is not strictly vertical, horizontal, or diagonal",
                nameof(destination));

        int incrementX = (changeX == 0) ? 0 : changeX / Math.Abs(changeX);
        int incrementY = (changeY == 0) ? 0 : changeY / Math.Abs(changeY);
        // Calculate how much to increment the loops by turning any positive number into 1,
        // any negative number into -1, and leaving any 0 as a 0

        int iX = source.X;
        int iY = source.Y;
        // Store source coordinates as iterators
        
        pointsBetween.Add(new Point(iX, iY));
        // Add the first Point to avoid skipping it due to loop order

        while (iX != destination.X || iY != destination.Y)
        {
            
            iX += incrementX;
            iY += incrementY;
            // Go to the next Point based on the increment direction
            pointsBetween.Add(new Point(iX, iY));
            // Add the next Point

        }

        return pointsBetween;

    }
    
}