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
    private string _fen;
    // A string that represents the Board state
    private List<Piece> _capturedWhitePieces;
    // The white Pieces that have been captured by black
    private List<Piece> _capturedBlackPieces;
    // The black Pieces that have been captured by white

    public Game()
    // Default constructor for starting a new Game
    {
        
        _board = new Piece[8, 8];
        _enPassantTile = null;
        _whiteCanCastleKingside = true;
        _whiteCanCastleQueenside = true;
        _blackCanCastleKingside = true;
        _blackCanCastleQueenside = true;
        _halfMoves = 0;
        _fullMoves = 0;
        _isWhiteTurn = true;
        
        ResetGame();
        _fen = GenerateCurrentFen();

        Console.WriteLine("FEN: " + _fen);

        _capturedWhitePieces = new List<Piece>();
        _capturedBlackPieces = new List<Piece>();

    }

    // private void ResetGame()
    // {
    //
    //     "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 0";
    //
    // }

    private void ResetGame()
    // Resets the chess board to the default chess starting layout
    // Assumes that _board has been initialized
    // TODO: Make a different function that does this from a Fen string (probably delete this one as well)
    {
    
        _board[0, 1] = new Pawn(0, 1, true);
        _board[1, 1] = new Pawn(1, 1, true);
        _board[2, 1] = new Pawn(2, 1, true);
        _board[3, 1] = new Pawn(3, 1, true);
        _board[4, 1] = new Pawn(4, 1, true);
        _board[5, 1] = new Pawn(5, 1, true);
        _board[6, 1] = new Pawn(6, 1, true);
        _board[7, 1] = new Pawn(7, 1, true);
        // White Pawns
        
        _board[0, 0] = new Rook(0, 0, true);
        _board[1, 0] = new Knight(1, 0, true);
        _board[2, 0] = new Bishop(2, 0, true);
        _board[3, 0] = new Queen(3, 0, true);
        _board[4, 0] = new King(4, 0, true);
        _board[5, 0] = new Bishop(5, 0, true);
        _board[6, 0] = new Knight(6, 0, true);
        _board[7, 0] = new Rook(7, 0, true);
        // White Rooks, Knights, Bishops, Queen, and King
        
        _board[0, 6] = new Pawn(0, 6, false);
        _board[1, 6] = new Pawn(1, 6, false);
        _board[2, 6] = new Pawn(2, 6, false);
        _board[3, 6] = new Pawn(3, 6, false);
        _board[4, 6] = new Pawn(4, 6, false);
        _board[5, 6] = new Pawn(5, 6, false);
        _board[6, 6] = new Pawn(6, 6, false);
        _board[7, 6] = new Pawn(7, 6, false);
        // Black Pawns
        
        _board[0, 7] = new Rook(0, 7, false);
        _board[1, 7] = new Knight(1, 7, false);
        _board[2, 7] = new Bishop(2, 7, false);
        _board[3, 7] = new Queen(3, 7, false);
        _board[4, 7] = new King(4, 7, false);
        _board[5, 7] = new Bishop(5, 7, false);
        _board[6, 7] = new Knight(6, 7, false);
        _board[7, 7] = new Rook(7, 7, false);
        // Black Rooks, Knights, Bishops, Queen, and King
    
    }

    private string GenerateCurrentFen()
    // Generates a Fen string from the current game state
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
    
    private void SetGamestateWithFen(string fen) {
    // Generates a Fen string from the current game state

        Point currentPos = new Point(0, 7);
        Piece? currentPiece;

        foreach(char c in fen)
        {

            switch (c)
            {
                
                case ' ' or '-':
                    break;
                case '/':
                    currentPos.Y++;
                    break;
                case >= '0' and <= '7': 
                    currentPos.X += (c - '0');
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
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'P':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'R':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'N':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'B':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'Q':
                    currentPiece = new Rook(currentPos, false);
                    break;
                case 'K':
                    currentPiece = new Rook(currentPos, false);
                    break;

            }

            if (currentPos.IsOutOfBounds())
                throw new ArgumentOutOfRangeException(nameof(fen), "[ERROR] An illegal FEN String was used: ran out of board bounds");

        }

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
