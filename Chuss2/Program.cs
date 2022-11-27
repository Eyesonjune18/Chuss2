using Chuss2;

// Gamestate g = new Gamestate("k7/8/8/8/8/pp6/P7/7K w - - 0 0");
Gamestate g = new Gamestate();
g.PrintBoard();
Console.WriteLine(g.GenerateCurrentFen());

while (true)
{
    
    Console.Write("Move: ");
    string? m = Console.ReadLine();
    if (m != null)
    {
        
        Move move = Utilities.ParseMove(m);
        g.PerformMove(move);
        g.PrintBoard();

        Console.WriteLine("Successful move from (" + move.Source.X + ", " + move.Source.Y + ") to (" + move.Destination.X + ", " +
                          move.Destination.Y + ")");
        
        Console.WriteLine();

    }

}

// TODO: Normalize error messages