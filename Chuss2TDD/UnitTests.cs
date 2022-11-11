namespace Chuss2TDD;

public class Tests
{
    private const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0";
    private const string SpecialFen1 = "r4bkr/ppp3pp/2n1B3/4p3/8/8/PPPP1PPP/RNB1K2R b KQ - 0 3";
    private const string SpecialFen2 = "1rr5/1pp1k1pp/p1nb4/3Q1p2/1P1BP1P1/5P1b/P1P1K2P/4R2R b - - 3 19";
    private readonly Board _defaultBoard = new Board();
    // Board has default FEN built-in
    private readonly Board _specialBoard1 = new Board(SpecialFen1.Split(' ')[0]);
    private readonly Board _specialBoard2 = new Board(SpecialFen2.Split(' ')[0]);
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
    public void FenPropertyTestDefault()
    // Tests the default Gamestate FEN property being set correctly
    {

        Gamestate g = new Gamestate();
        
        Assert.That(g.Fen, Is.EqualTo(DefaultFen));

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
            Assert.That(g.Fen, Is.EqualTo(DefaultFen));
            Assert.That(!g.CapturedWhitePieces.Any());
            Assert.That(!g.CapturedBlackPieces.Any());
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
            Assert.That(g.Fen, Is.EqualTo(SpecialFen1));
            Assert.That(!g.CapturedWhitePieces.Any());
            Assert.That(!g.CapturedBlackPieces.Any());
            
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
            Assert.That(g.Fen, Is.EqualTo(SpecialFen2));
            Assert.That(!g.CapturedWhitePieces.Any());
            Assert.That(!g.CapturedBlackPieces.Any());
            
        });

    }

}