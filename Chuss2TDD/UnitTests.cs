namespace Chuss2TDD;

public class Tests
{
    
    private const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0";
    private const string SpecialFen1 = "r4bkr/ppp3pp/2n1B3/4p3/8/8/PPPP1PPP/RNB1K2R b KQ - 0 3";
    private const string SpecialFen2 = "1rr5/1pp1k1pp/p1nb4/3Q1p2/1P1BP1P1/5P1b/P1P1K2P/4R2R b - - 3 19";
    private const string SpecialFen3 = "Q7/7k/K7/8/8/8/8/8 b - - 0 0";
    private readonly Board _defaultBoard = new();
    // Board has default FEN built-in
    private readonly Board _specialBoard1 = new(SpecialFen1.Split(' ')[0]);
    private readonly Board _specialBoard2 = new(SpecialFen2.Split(' ')[0]);
    private readonly Board _specialBoard3 = new(SpecialFen3.Split(' ')[0]);
    // Functionality of Board constructor has been manually verified

    [SetUp]
    public void Setup()
    {

        

    }

    [Test]
    public void GenerateFenTestDefault()
    // Tests GenerateCurrentFen() using the default FEN string
    {

        Gamestate g = new Gamestate();

        Assert.That(g.GenerateCurrentFen(), Is.EqualTo(DefaultFen));

    }
    
    [Test]
    public void GenerateFenTestSpecial1() 
    // Tests GenerateCurrentFen() using a miscellaneous FEN string
    {

        Gamestate g = new Gamestate(SpecialFen1);

        Assert.That(g.GenerateCurrentFen(), Is.EqualTo(SpecialFen1));

    }
    
    [Test]
    public void GenerateFenTestSpecial2() 
    // Tests GenerateCurrentFen() using a miscellaneous FEN string
    {

        Gamestate g = new Gamestate(SpecialFen2);

        Assert.That(g.GenerateCurrentFen(), Is.EqualTo(SpecialFen2));

    }
    
    [Test]
    public void GenerateFenTestSpecial3() 
    // Tests GenerateCurrentFen() using a miscellaneous FEN string
    {

        Gamestate g = new Gamestate(SpecialFen3);

        Assert.That(g.GenerateCurrentFen(), Is.EqualTo(SpecialFen3));

    }

    [Test]
    public void SetGamestateTestDefault()
    {
        Gamestate g = new Gamestate();
        
        Assert.Multiple(() =>
        {
            
            Assert.That(g.Board.Equals(_defaultBoard), Is.True);
            Assert.That(g.EnPassantTile is null);
            Assert.That(g.WhiteCanCastleKingside);
            Assert.That(g.WhiteCanCastleQueenside);
            Assert.That(g.BlackCanCastleKingside);
            Assert.That(g.BlackCanCastleQueenside);
            Assert.That(g.HalfMoves, Is.EqualTo(0));
            Assert.That(g.FullMoves, Is.EqualTo(0));
            Assert.That(g.IsWhiteTurn);
            Assert.That(g.GenerateCurrentFen(), Is.EqualTo(DefaultFen));
            // TODO: Add capture arrays when capture thing works
            
        });
        
    }

    [Test]
    public void SetGamestateTestSpecial1() 
    // Tests all properties being set correctly for a miscellaneous FEN string
    {

        Gamestate g = new Gamestate(SpecialFen1);
        
        Assert.Multiple(() =>
        {
            
            Assert.That(g.Board.Equals(_specialBoard1), Is.True);
            Assert.That(g.EnPassantTile is null);
            Assert.That(g.WhiteCanCastleKingside);
            Assert.That(g.WhiteCanCastleQueenside);
            Assert.That(!g.BlackCanCastleKingside);
            Assert.That(!g.BlackCanCastleQueenside);
            Assert.That(g.HalfMoves, Is.EqualTo(0));
            Assert.That(g.FullMoves, Is.EqualTo(3));
            Assert.That(!g.IsWhiteTurn);
            Assert.That(g.GenerateCurrentFen(), Is.EqualTo(SpecialFen1));
            
        });

    }
    
    [Test]
    public void SetGamestateTestSpecial2() 
    // Tests all properties being set correctly for a miscellaneous FEN string
    {

        Gamestate g = new Gamestate(SpecialFen2);
        
        Assert.Multiple(() =>
        {
            
            Assert.That(g.Board.Equals(_specialBoard2), Is.True);
            Assert.That(g.EnPassantTile is null);
            Assert.That(!g.WhiteCanCastleKingside);
            Assert.That(!g.WhiteCanCastleQueenside);
            Assert.That(!g.BlackCanCastleKingside);
            Assert.That(!g.BlackCanCastleQueenside);
            Assert.That(g.HalfMoves, Is.EqualTo(3));
            Assert.That(g.FullMoves, Is.EqualTo(19));
            Assert.That(!g.IsWhiteTurn);
            Assert.That(g.GenerateCurrentFen(), Is.EqualTo(SpecialFen2));
            
        });

    }

    [Test]
    public void SetGamestateTestSpecial3() 
    // Tests all properties being set correctly for a miscellaneous FEN string
    {

        Gamestate g = new Gamestate(SpecialFen3);
        
        Assert.Multiple(() =>
        {
            
            Assert.That(g.Board.Equals(_specialBoard3), Is.True);
            Assert.That(g.EnPassantTile is null);
            Assert.That(!g.WhiteCanCastleKingside);
            Assert.That(!g.WhiteCanCastleQueenside);
            Assert.That(!g.BlackCanCastleKingside);
            Assert.That(!g.BlackCanCastleQueenside);
            Assert.That(g.HalfMoves, Is.EqualTo(0));
            Assert.That(g.FullMoves, Is.EqualTo(0));
            Assert.That(!g.IsWhiteTurn);
            Assert.That(g.GenerateCurrentFen(), Is.EqualTo(SpecialFen3));
            
        });

    }

    [Test]
    public void MoveValidationSelfCheckTest()
    // Tests if the self-check rule works
    {

        Gamestate g = new Gamestate(SpecialFen3);
        
        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(7, 6), new Point(7, 7))));

    }

    [Test]
    public void MoveValidationPawnTest()
    // Tests if Pawn move behavior works
    // TODO: Add color-specific tests
    {

        Gamestate g = new Gamestate();

        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(0, 1), new Point(0, 6))));
        // Past move distance test
        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(0, 1), new Point(1, 2))));
        // Capture pattern with no capture test
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(0, 1), new Point(0, 2))));
        // Standard 1-tile move test
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(0, 1), new Point(0, 3))));
        // Standard en passant move test
        Assert.DoesNotThrow(() => g.PerformMove(new Move(new Point(0, 1), new Point(0, 2))));
        // Standard move, prep for next test
        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(0, 2), new Point(0, 4))));
        // En passant after first move test

        g = new Gamestate("k7/8/8/8/8/pp6/P7/7K w - - 0 0");
        
        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(0, 1), new Point(0, 2))));
        // Standard move with capture test
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(0, 1), new Point(1, 2))));
        // Capture pattern with capture test

    }

    [Test]
    public void MoveValidationRookTest()
    // Tests if Rook move behavior works
    {
    
        Gamestate g = new Gamestate("k7/8/8/8/4R3/8/8/7K w - - 0 0");
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(5, 3))));
        // Standard 1-tile move test x+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(3, 3))));
        // Standard 1-tile move test x-
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(4, 4))));
        // Standard 1-tile move test y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(4, 2))));
        // Standard 1-tile move test y-
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(7, 3))));
        // Standard multi-tile move test x+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(0, 3))));
        // Standard multi-tile move test x-
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(4, 7))));
        // Standard multi-tile move test y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(4, 0))));
        // Standard multi-tile move test y-

        g = new Gamestate("k7/8/8/4p3/4R3/8/8/7K w - - 0 0");

        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(4, 3), new Point(4, 7))));
        // Collision checker test
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(4, 4))));
        // Standard capture test
    
    }
    
    [Test]
    public void MoveValidationKnightTest()
    // Tests if Knight move behavior works
    {

        Gamestate g = new Gamestate("k7/8/8/8/4N3/8/8/7K w - - 0 0");
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(6, 4))));
        // Standard 1-tile move test x+y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(6, 2))));
        // Standard 1-tile move test x+y-
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(2, 4))));
        // Standard 1-tile move test x-y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(2, 2))));
        // Standard 1-tile move test x-y-
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(5, 5))));
        // Standard multi-tile move test y+x+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(3, 5))));
        // Standard multi-tile move test y+x-
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(5, 2))));
        // Standard multi-tile move test y-x+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(3, 2))));
        // Standard multi-tile move test y-x-

        g = new Gamestate("k7/8/8/2p5/4N3/8/8/7K w - - 0 0");

        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(2, 4))));
        // Standard capture test
        
    }
    
    [Test]
    public void MoveValidationBishopTest() 
    // Tests if Bishop move behavior works
    {
    
        Gamestate g = new Gamestate("4k3/8/8/8/4B3/8/8/4K3 w - - 0 0");
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(5, 4))));
        // Standard 1-tile move test x+y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(3, 4))));
        // Standard 1-tile move test x-y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(5, 2))));
        // Standard 1-tile move test x+y-
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(3, 2))));
        // Standard 1-tile move test x-y-
        
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(7, 6))));
        // Standard multi-tile move test x+y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(0, 6))));
        // Standard multi-tile move test x-y+
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(7, 0))));
        // Standard multi-tile move test x+y-
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(1, 0))));
        // Standard multi-tile move test x-y-

        g = new Gamestate("k7/8/8/3p4/4B3/8/8/7K w - - 0 0");

        Assert.Throws<ArgumentException>(() => g.PerformMove(new Move(new Point(4, 3), new Point(0, 6))));
        // Collision checker test
        Assert.DoesNotThrow(() => g.ValidateMove(new Move(new Point(4, 3), new Point(3, 5))));
        // Standard capture test
    
    }

    [Test]
    public void CheckMateTest()
    // Tests if the LookForCheckmate() function works
    {

        Gamestate g = new Gamestate("3k4/8/8/8/8/8/1r6/r2K4 w - - 0 1");
        
        Assert.That(g.LookForCheckmate());

        g = new Gamestate("3k4/8/8/8/2Q5/8/1r6/r2K4 w - - 0 1");
        
        Assert.That(!g.LookForCheckmate());

    }

}