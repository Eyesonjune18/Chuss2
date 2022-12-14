using System.Text;

namespace Chuss2;

public class Gamestate
// Represents an overall gamestate, including the Board, Players, and Pieces,
// along with any game-specific settings/options
{

    #region Fields
    
    private Board _board;
    private Point? _enPassantTile;
    // The current tile that can be used to en passant capture
    private bool _whiteCanCastleKingside;
    private bool _whiteCanCastleQueenside;
    private bool _blackCanCastleKingside;
    private bool _blackCanCastleQueenside;
    private bool _whiteInCheck;
    private bool _blackInCheck;
    private int _halfMoves;
    // Number of halfmoves (turns since Pawn has been moved, or a Piece has been captured)
    private int _fullMoves;
    // Number of fullmoves (total moves in the game)
    private bool _isWhiteTurn;

    #endregion

    #region Constructors

    public Gamestate() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0") {}
    // Default constructor - uses default FEN string

    public Gamestate(string fen) 
    // Main constructor - uses specified FEN string
    {
        
        _board = new Board();

        SetGamestateWithFen(fen);

    }
    
    #endregion
    
    #region Properties

    public Board Board => _board;

    public Point? EnPassantTile => _enPassantTile;

    public bool WhiteCanCastleKingside => _whiteCanCastleKingside;
    
    public bool WhiteCanCastleQueenside => _whiteCanCastleQueenside;

    public bool BlackCanCastleKingside => _blackCanCastleKingside;

    public bool BlackCanCastleQueenside => _blackCanCastleQueenside;

    public int HalfMoves => _halfMoves;

    public int FullMoves => _fullMoves;

    public bool IsWhiteTurn => _isWhiteTurn;

    #endregion

    #region Mutators

    private void SetGamestateWithFen(string fen) 
    // Parses a FEN string into a Gamestate by setting all relevant fields
    {

        const string errorMsg = "[ERROR] An illegal FEN string was used: ";
        const string unexpectedChar = errorMsg + "an unexpected character was found in ";
        // Add shorthands for various errors

        string[] fenSplit = fen.Split(' ');
        if (fenSplit.Length != 6) throw new ArgumentException(errorMsg + "incorrect element count", nameof(fen));
        string fenBoard = fenSplit[0];
        string fenTurn = fenSplit[1];
        string fenCastle = fenSplit[2];
        string fenEnPassant = fenSplit[3];
        string fenHalfmove = fenSplit[4];
        string fenFullmove = fenSplit[5];
        // Split the FEN into the elements specified by the standard

        _board = new Board(fenBoard);
        
        _isWhiteTurn = fenTurn switch
        {
            
            "w" => true,
            "b" => false,
            _ => throw new ArgumentException(unexpectedChar + "turn section", nameof(fen))
            
        };

        if (!fenCastle.Equals("-"))
        {
            
            foreach (char c in fenCastle)
            {
        
                switch (c)
                {
        
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
        
        }

        if (!fenEnPassant.Equals("-"))
        {
            
            if (fenEnPassant.Length != 2 || (fenEnPassant[0] is < 'a' or > 'h') ||
                (fenEnPassant[1] is not '3' or '6')) throw new ArgumentException(unexpectedChar + "en passant section", nameof(fen));
            _enPassantTile = Utilities.FromAlgebraicNotation(fenEnPassant);
            
        }

        try { _halfMoves = int.Parse(fenHalfmove); }
        catch (FormatException) { throw new ArgumentException(unexpectedChar + "halfmove section", nameof(fen)); }
        try { _fullMoves = int.Parse(fenFullmove); }
        catch (FormatException) { throw new ArgumentException(unexpectedChar + "fullmove section", nameof(fen)); }
        
        if (LookForCheckWithoutMove(true)) _whiteInCheck = true;
        if (LookForCheckWithoutMove(false)) _blackInCheck = true;
        // TODO: Remove if refactored

    }

    public void PerformMove(Move move)
    // Performs a move with input supplied from the user
    {

        ValidationResult validity = ValidateMove(move);

        if (validity is ValidationResult.Invalid iv)
            throw new ArgumentException("[ERROR] " + iv.Reason, nameof(move.Destination));

        if (validity is ValidationResult.Valid)
        {
            
            AdjustStateModifiers(move);
            MovePiece(move);

        }

    }

    private void MovePiece(Move move)
    // Moves a Piece irrespective of any legality checks
    {
        
        _board.SetPiece(move.Destination, _board.PieceAtPosition(move.Source));
        _board.ClearPiece(move.Source);
        
    }

    private void AdjustStateModifiers(Move move)
    // Edits fields as needed after a given Move
    {
        
        if (LookForCheckWithoutMove(true)) _whiteInCheck = true;
        if (LookForCheckWithoutMove(false)) _blackInCheck = true;
        // TODO: Potentially add some optimization by returning checks from ValidationResults
        
        Piece? movedP = _board.PieceAtPosition(move.Source);
        // If the Move is valid, the moved Piece cannot be null, but nullability is included for syntactical purposes
        Piece? capturedP = _board.PieceAtPosition(move.Destination);

        if (movedP is not null) movedP.HasMoved = true;

        _fullMoves++;
        if (movedP is not Pawn && capturedP is null) _halfMoves++;
        else _halfMoves = 0;
            
        _isWhiteTurn = !_isWhiteTurn;
        // Swap the turn to the other side
        
    }

    #endregion
    
    #region Accessors
    
    public string GenerateCurrentFen()
    // Generates a FEN string from the current game state
    {

        StringBuilder fen = new StringBuilder();

        fen.Append(_board.GenerateFen());
        // Add the Board FEN

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
        fen.Append(_enPassantTile is null ? '-' : Utilities.ToAlgebraicNotation(_enPassantTile));
        // If there is an en passant target tile, append its algebraic name

        fen.Append(" " + _halfMoves + " " + _fullMoves);
        // Add the number of halfmoves and fullmoves

        return fen.ToString();

    }

    public ValidationResult ValidateMove(Move move) => ValidateMove(move, true);

    public ValidationResult ValidateMove(Move move, bool lookForCheck)
    {

        ValidationResult result = new ValidationResult.Invalid();

        if (move.Source.HasSameCoords(move.Destination))
            // Checks if the Piece is actually being moved
            return new ValidationResult.Invalid("A Piece cannot be moved to its own position");

        Piece? sourceP = _board.PieceAtPosition(move.Source);
        // The Piece to be moved
        Piece? destinationP = _board.PieceAtPosition(move.Destination);
        // The Piece to be captured (if a capture will occur)

        if (sourceP is null) return new ValidationResult.Invalid("An empty tile cannot be moved");
            // Checks for empty tiles

            if (sourceP.IsWhite != _isWhiteTurn)
                return new ValidationResult.Invalid("The moved Piece must be friendly");
            // Prevents moving enemy Pieces

        if (!sourceP.IsMoveLegal(move, destinationP is not null))
            // Checks whether move pattern is legal for this Piece type
            return new ValidationResult.Invalid("The given Piece type cannot move in the way specified");

        if (sourceP is not Knight && CheckForCollision(move)) 
            // Knights are excluded from collision checker because they can hop over Pieces
            return new ValidationResult.Invalid("Pieces cannot collide with other Pieces when moving");

        if (destinationP is not null && sourceP.IsWhite == destinationP.IsWhite)
            // If the captured Piece is the same color as the moved Piece
            return new ValidationResult.Invalid("A Piece cannot capture another Piece of the same color");
        if (destinationP is not null && sourceP.IsWhite != destinationP.IsWhite)
            // If there is a capture and it is valid (captured Piece is the opposite color)
            result = new ValidationResult.Valid(destinationP);
            // Add a captured Piece to the result
        else if (destinationP is null) result = new ValidationResult.Valid();
            // If the move has not tripped any illegality checks and it is not a capture, the move is valid

        if (lookForCheck && LookForCheckWithMove(move))
            return new ValidationResult.Invalid("A move cannot put the current side's King in check");

        return result;

    }

    private bool CheckForCollision(Move move) => CheckForCollision(move, false);
    
    private bool CheckForCollision(Move move, bool includeCaptureTile)
    // Checks whether the given move will result in a collision between two pieces
    {

        List<Point> tilesToCheck = Utilities.GetPointsBetween(move.Source, move.Destination);
        // Get all Points between the source and the destination
        tilesToCheck.RemoveAt(0);
        // Ignore the Piece itself for all moves
        if(!includeCaptureTile) tilesToCheck.RemoveAt(tilesToCheck.Count - 1);
        // Ignore the destination tile for collisions if not specified (generally because a piece is being captured)

        return tilesToCheck.Any(p => _board.PieceAtPosition(p) is not null);
        // If any of the tiles in between the source and destination contain a Piece,
        // return true (collision has occurred)
        
    }

    private bool LookForCheckWithMove(Move move) =>
        LookForCheckWithMove(move, _isWhiteTurn);

    private bool LookForCheckWithMove(Move move, bool whiteTurn)
    // Returns true if the current color's King will be in check after a given move
    {

        Gamestate g = new Gamestate(GenerateCurrentFen());
        g.MovePiece(move);
        g.AdjustStateModifiers(move);

        return LookForCheck(g, whiteTurn);

    }

    private bool LookForCheckWithoutMove() => LookForCheckWithoutMove(_isWhiteTurn);

    private bool LookForCheckWithoutMove(bool whiteTurn) => LookForCheck(this, whiteTurn);
    // Returns true if the given color's king is currently in check

    private bool LookForCheck(Gamestate game, bool whiteTurn)
    // Returns true if the the given color's King is in check, disregarding turn order
    {

        Point kingPos = game.KingPos(whiteTurn);

        for (int i = 0; i < 64; i++)
        {
            
            Point pos = Utilities.Translate1DCoordTo2D(i);
            Piece? p = game.Board.PieceAtPosition(pos);
            
            if (p is null) continue;

            if (whiteTurn && p.IsWhite || !whiteTurn && !p.IsWhite) continue;
            // Skip friendly Pieces

            if (game.ValidateMove(new Move(pos, kingPos), false) is ValidationResult.Valid) return true;

        }

        return false;

    }

    public bool LookForCheckmate() => LookForCheckmate(_isWhiteTurn);

    private bool LookForCheckmate(bool white)
    // Returns true if the given color is in checkmate
    {

        List<Move> possibleMoves = Utilities.GetPossibleFriendlyMoves(this);

        foreach (Move m in possibleMoves)
        {

            if (!LookForCheckWithMove(m)) return false;

        }

        return true;

    }
    
    private Point KingPos(bool whiteTurn)
    // Retrieves the position of the given side's King
    {

        for (int i = 0; i < 64; i++)
        {

            Point pos = Utilities.Translate1DCoordTo2D(i);
            Piece? p = _board.PieceAtPosition(pos);

            if (p is King && p.IsWhite == whiteTurn) return pos;

        }

        return new Point(-1, -1);
        // Should never happen, as the King cannot be captured

    }

    #endregion
    
    #region UI

    public void PrintBoard()
    // Prints the board to the CLI
    // TODO: Move this to a proper UI class
    {

        for (int i = 0; i < 64; i++)
        {

            Piece? p = _board.PieceAtPosition(Utilities.Translate1DCoordTo2D(i));
            
            char pChar = ' ';
            if (p is not null) pChar = p.PieceTypeChar;

            if (i != 0 && i % 8 == 0) Console.WriteLine();
            if ((i + (i / 8) % 2) % 2 == 0) Console.Write("[" + pChar + "]");
            if ((i + (i / 8) % 2) % 2 != 0) Console.Write("(" + pChar + ")");
            // Adjust whether to print a "white" or "black" tile depending on the row/column being even/odd

        }

        Console.WriteLine('\n');

    }
    
    #endregion
    
}