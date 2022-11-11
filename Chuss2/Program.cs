using Chuss2;

Gamestate g = new Gamestate();
g.PrintBoard();

g.SetGamestateWithFen("r4bkr/ppp3pp/2n1B3/4p3/8/8/PPPP1PPP/RNB1K2R b KQ - 0 3");
g.PrintBoard();
Console.WriteLine(g.GenerateCurrentFen());

while (true)
{
    
    Console.Write("Move: ");
    string? m = Console.ReadLine();
    if (m != null)
    {
        
        List<Point> move = Utilities.ParseMove(m);
        g.PerformMove(move[0], move[1]);
        g.PrintBoard();
        
        Console.WriteLine("Successful move from (" + move[0].X + ", " + move[0].Y + ") to (" + move[1].X + ", " + move[1].Y + ")");
        
        Console.WriteLine();

    }

}

// TODO: Normalize error messages