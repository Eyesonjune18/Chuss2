namespace Chuss2;

public class Board
{

    private readonly Piece?[,] _pieceArr = new Piece?[8, 8];

    public Board() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR") {}

    public Board(string fen)
    {

        int currentPos1D = 0;
        Piece? currentPiece = null;
        
        foreach (char c in fen)
        {

            Point currentPos2D = Utilities.Translate1DCoordTo2D(currentPos1D);

            switch (c)
            {

                case '/':
                    if (currentPos2D.X != 0)
                        throw new ArgumentException(
                            "[ERROR] An illegal FEN string was used: incorrect amount of characters in board section",
                            nameof(fen));
                    break;
                case >= '1' and <= '8':
                    currentPos1D += (c - '0');
                    break;
                case 'P':
                    currentPiece = new Pawn(true);
                    break;
                case 'R':
                    currentPiece = new Rook(true);
                    break;
                case 'N':
                    currentPiece = new Knight(true);
                    break;
                case 'B':
                    currentPiece = new Bishop(true);
                    break;
                case 'Q':
                    currentPiece = new Queen(true);
                    break;
                case 'K':
                    currentPiece = new King(true);
                    break;
                case 'p':
                    currentPiece = new Pawn(false);
                    break;
                case 'r':
                    currentPiece = new Rook(false);
                    break;
                case 'n':
                    currentPiece = new Knight(false);
                    break;
                case 'b':
                    currentPiece = new Bishop(false);
                    break;
                case 'q':
                    currentPiece = new Queen(false);
                    break;
                case 'k':
                    currentPiece = new King(false);
                    break;
                default:
                    throw new ArgumentException(
                        "[ERROR] An illegal FEN string was used: an unexpected character was found in board section",
                        nameof(fen));

            }

            if (currentPos2D.IsOutOfBounds())
                throw new ArgumentOutOfRangeException(nameof(fen),
                    "[ERROR] An illegal FEN string was used: ran out of board bounds");

            if (currentPiece == null) continue;
            _pieceArr[currentPos2D.X, currentPos2D.Y] = currentPiece;
            currentPiece = null;
            currentPos1D++;

        }

        if (currentPos1D != 64)
            throw new ArgumentException(
                "[ERROR] An illegal FEN string was used: incorrect amount of characters in board section", nameof(fen));

    }

    public void SetPiece(Point pos, Piece? piece) => _pieceArr[pos.X, pos.Y] = piece;
    // Sets the Piece at a given tile

    public void ClearPiece(Point pos) => _pieceArr[pos.X, pos.Y] = null;
    // Deletes the Piece at a given tile
    
    public void ClearBoard()
    // Deletes all the Pieces on the current Board
    {

        for (int i = 0; i < 64; i++)
        {

            Point p = Utilities.Translate1DCoordTo2D(i);

            SetPiece(p, null);

        }
        
    }

    public Piece? PieceAtPosition(Point pos)
    // Returns the Piece at a given tile
    {
        
        if (!new Point(pos.X, pos.Y).IsOutOfBounds()) return _pieceArr[pos.X, pos.Y];
        throw new ArgumentOutOfRangeException(nameof(pos),
            "[ERROR] An attempt was made to retrieve a Piece outside the board boundaries");
        // Throw exception if position is out of 8x8 range

    }

    public bool Equals(Board b)
    // Returns true if all Pieces on the given Board match the ones in this Board
    {

        for (int i = 0; i < 64; i++)
        {

            Point p = Utilities.Translate1DCoordTo2D(i);

            Piece? thisPiece = PieceAtPosition(p);
            Piece? otherPiece = b.PieceAtPosition(p);

            if (thisPiece != null && !thisPiece.Equals(otherPiece)) return false;

        }

        return true;

    }

}