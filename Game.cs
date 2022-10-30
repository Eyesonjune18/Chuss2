using System.Text;

namespace Chuss2;

public class Game
// Represents an overall Game-state, including the Board, Players, and Pieces,
// along with any game-specific settings/options
{

    private Piece?[,] _board;
    // The 8x8 grid of Pieces
    private Point? _enPassantTile;
    // The current tile that can be used to en passant capture
    private bool _whiteCanCastleKingside;
    // Whether white can still castle kingside
    private bool _whiteCanCastleQueenside;
    // Whether white can still castle queenside
    private bool _blackCanCastleKingside;
    // Whether black can still castle kingside
    private bool _blackCanCastleQueenside;
    // Whether black can still castle queenside
    private int _halfMoves;
    // Number of halfmoves (turns since Pawn has been moved, or a Piece has been captured)
    private int _fullMoves;
    // Number of fullmoves (total moves in the game)
    private bool _isWhiteTurn;
    // Whether the current turn is for white (true) or black (false)
    private string? _fen;
    // A string that represents the Board state
    private List<Piece> _capturedWhitePieces;
    // The white Pieces that have been captured by black
    private List<Piece> _capturedBlackPieces;
    // The black Pieces that have been captured by white

    public Game()
    // Default constructor for starting a new Game
    {
        
        _board = new Piece[8, 8];
        _whiteCanCastleKingside = true;
        _whiteCanCastleQueenside = true;
        _blackCanCastleKingside = true;
        _blackCanCastleQueenside = true;
        _enPassantTile = null;
        _halfMoves = 0;
        _fullMoves = 0;
        _isWhiteTurn = true;

        ResetGame();
        
        Console.WriteLine("FEN: " + _fen);

        _capturedWhitePieces = new List<Piece>();
        _capturedBlackPieces = new List<Piece>();

    }

    private void ResetGame()
    {
    
        SetGamestateWithFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0");
    
    }

    private string GenerateCurrentFen()
    // Generates a FEN string from the current game state
    {

        StringBuilder fen = new StringBuilder();
        int emptyTilesInSection = 0;
        // Represents the amount of sequential tiles without Pieces, to be reset after each section is broken,
        // or at the end of a row when broken by a '/'

        for (int pos = 0; pos < 64; pos++)
        {

            Piece? currentPiece = PieceAtPosition(Translate1DCoordTo2D(pos));
            // Get the current Piece from the coordinate on the board corresponding to the 1D coordinate pos

            if (pos != 63 && (pos + 1) % 8 == 0)
                // At the end of a row, except for the very last tile (7, 0)
            {

                if (emptyTilesInSection != 0) fen.Append(emptyTilesInSection + 1);
                emptyTilesInSection = 0;
                // If an empty tile section is currently being evaluated, break it and reset the counter
                fen.Append('/');

            }
            else if (currentPiece == null) emptyTilesInSection++;
            // If the tile is null, either start or continue incrementing the section counter
            else fen.Append(currentPiece.PieceTypeChar);
            // If there is a Piece, append the char representing the piece (ex. white Pawn = 'P')

        }

        fen.Append(_isWhiteTurn ? " w" : " b");
        // Add a 'w' or 'b' representing the current turn color

        fen.Append(' ');
        if (_whiteCanCastleKingside) fen.Append('K');
        if (_whiteCanCastleQueenside) fen.Append('Q');
        if (_blackCanCastleKingside) fen.Append('k');
        if (_blackCanCastleQueenside) fen.Append('q');
        if (!(_whiteCanCastleKingside || _whiteCanCastleQueenside || _blackCanCastleKingside ||
              _blackCanCastleQueenside)) fen.Append('-');
        // Add one or more of the characters in "KQkq" depending on castling options for each side
        // If neither side has castling options, add a '-'

        fen.Append(' ');
        fen.Append((_enPassantTile == null) ? '-' : _enPassantTile.ToAlgebraicNotation());
        // If there is an en passant target tile, append its algebraic name

        fen.Append(" " + _halfMoves + " " + _fullMoves);
        // Add the number of halfmoves and fullmoves

        return fen.ToString();

    }

    private void SetGamestateWithFen(string fen) 
    // Parses a FEN string into a game state
    {

        string errorMsg = "[ERROR] An illegal FEN string was used: ";
        string unexpectedChar = errorMsg + "an unexpected character was found in ";
        string notEnoughChars = errorMsg + "incorrect amount of characters in board section";

        string[] fenSplit = fen.Split(' ');
        if (fenSplit.Length != 6) throw new ArgumentException(errorMsg + "incorrect element count", nameof(fen));
        string fenBoard = fenSplit[0];
        string fenTurn = fenSplit[1];
        string fenCastle = fenSplit[2];
        string fenEnPassant = fenSplit[3];
        string fenHalfmove = fenSplit[4];
        string fenFullmove = fenSplit[5];
        // Split the FEN into the elements specified by the standard
        
        Point currentPos = new Point(0, 7);
        Piece? currentPiece = null;
        
        foreach (char c in fenBoard)
        {

            switch (c)
            {

                case '/':
                    if (currentPos.X == 7) currentPos.X = 0;
                    else throw new ArgumentException(notEnoughChars, nameof(fen));
                    currentPos.Y--;
                    break;
                case >= '1' and <= '8':
                    currentPos.X += (c - '1');
                    break;
                case 'p':
                    currentPiece = new Pawn(currentPos, false);
                    break;
                case 'r':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'n':
                    currentPiece = new Knight(currentPos, false);
                    break;
                case 'b':
                    currentPiece = new Bishop(currentPos, false);
                    break;
                case 'q':
                    currentPiece = new Queen(currentPos, false);
                    break;
                case 'k':
                    currentPiece = new King(currentPos, false);
                    break;
                case 'P':
                    currentPiece = new Pawn(currentPos, false);
                    break;
                case 'R':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'N':
                    currentPiece = new Knight(currentPos, false);
                    break;
                case 'B':
                    currentPiece = new Bishop(currentPos, false);
                    break;
                case 'Q':
                    currentPiece = new Queen(currentPos, false);
                    break;
                case 'K':
                    currentPiece = new King(currentPos, false);
                    break;
                default:
                    throw new ArgumentException(unexpectedChar + "board section", nameof(fen));

            }

            if (currentPos.IsOutOfBounds())
                throw new ArgumentOutOfRangeException(nameof(fen), errorMsg + "ran out of board bounds");

            if (currentPiece == null) continue; 
            _board[currentPos.X, currentPos.Y] = currentPiece;
            currentPiece = null;
            if(currentPos.X < 7) currentPos.X++;

        }

        if (currentPos.X != 7 || currentPos.Y != 0)
            throw new ArgumentException(notEnoughChars, nameof(fen));

        switch (fenTurn)
        {

            case "w":
                _isWhiteTurn = true;
                break;
            case "b":
                _isWhiteTurn = false;
                break;
            default:
                throw new ArgumentException(unexpectedChar + "turn section",
                    nameof(fen));

        }

        foreach (char c in fenCastle)
        {

            switch (c)
            {

                case '-':
                    break;
                case 'K':
                    _whiteCanCastleKingside = true;
                    break;
                case 'Q':
                    _whiteCanCastleQueenside = true;
                    break;
                case 'k':
                    _blackCanCastleKingside = true;
                    break;
                case 'q':
                    _blackCanCastleQueenside = true;
                    break;
                default:
                    throw new ArgumentException(unexpectedChar + "castle section");

            }

        }

        if (fenEnPassant != "-")
        {
            if (fenEnPassant.Length != 2 || (fenEnPassant[0] is <= 'a' or >= 'h') ||
                (fenEnPassant[1] is <= '1' or >= '8')) throw new ArgumentException(unexpectedChar + "en passant section", nameof(fen));
            _enPassantTile = new Point(fenEnPassant[0] - 'a', fenEnPassant[1] - '1');
            
        }

        try { _halfMoves = int.Parse(fenHalfmove); }
        catch (FormatException) { throw new ArgumentException(unexpectedChar + "halfmove section", nameof(fen)); }
        try { _fullMoves = int.Parse(fenFullmove); }
        catch (FormatException) { throw new ArgumentException(unexpectedChar + "fullmove section", nameof(fen)); }

        _fen = fen;

    }

    private static Point Translate1DCoordTo2D(int coord)
    // Translates a 1D board coordinate to a 2D board coordinate
    // Top-left to bottom-right
    {
        
        return new Point(coord % 8, 7 - coord / 8);

    }

    private Piece? PieceAtPosition(int column, int row)
    // Returns the Piece at a given tile (Point parameter)
    {

        return PieceAtPosition(new Point(column, row));

    }

    private Piece? PieceAtPosition(Point pos)
    // Returns the Piece at a given tile (int parameters)
    {
        
        if (!new Point(pos.X, pos.Y).IsOutOfBounds()) return _board[pos.X, pos.Y];
        else throw new ArgumentOutOfRangeException(nameof(pos), "[ERROR] An attempt was made to retrieve a Piece outside the boundaries of the board");
        // Throw exception if position is out of 8x8 range

        return null;
        // Should never happen

    }

}
