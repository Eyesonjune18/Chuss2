namespace Chuss2;

public class Move
{
    
    public Point Source { get; set; }
    public Point Destination { get; set; }

    public Move(Point source, Point destination)
    {

        Source = source;
        Destination = destination;

    }
    
}