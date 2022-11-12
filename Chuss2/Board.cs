using System.Text;

namespace Chuss2;

public class Board
{

    private readonly Piece?[,] _pieceArr = new Piece?[8, 8];

    public Board() : this("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR") {}
    // Default constructor - uses default FEN string

    public Board(string fen)
    // Main constructor - uses specified FEN string
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

            if (currentPiece is null) continue;

            if (currentPiece is Pawn)
            // If the Piece is a Pawn, check if it has moved already
            // Other Piece types are irrelevant, as they do not behave differently depending on HasMoved
            {

                currentPiece.HasMoved = currentPiece.IsWhite switch
                {

                    true => currentPos2D.Y != 1,
                    false => currentPos2D.Y != 6

                };

            }

            _pieceArr[currentPos2D.X, currentPos2D.Y] = currentPiece;
            currentPiece = null;
            currentPos1D++;

        }

        if (currentPos1D != 64)
            throw new ArgumentException(
                "[ERROR] An illegal FEN string was used: incorrect amount of characters in board section", nameof(fen));

    }

    public Piece?[,] PieceArr => _pieceArr;

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

    public string GenerateFen()
    // Generates the section of the FEN string representing the Board
    {

        StringBuilder fen = new StringBuilder();
        
        int emptyTilesInSection = 0;
        // Represents the amount of sequential tiles without Pieces, to be reset after each section is broken,
        // or at the end of a row when broken by a '/'

        for (int pos = 0; pos < 64; pos++)
        {

            Piece? currentPiece = PieceAtPosition(Utilities.Translate1DCoordTo2D(pos));
            // Get the current Piece from the coordinate on the board corresponding to the 1D coordinate pos

            if (pos != 0 && (pos % 8 == 0 || pos == 63))
                // At the end of a row, append '/' and do other behavior
                // TODO: Figure out a better way to fix issue with last tile
            {

                if (pos == 63 && currentPiece is null) emptyTilesInSection++;
                // Make sure last tile is evaluated

                if (emptyTilesInSection > 0)
                    // If an empty tile section is currently being evaluated, break it and reset the counter
                {
                    
                    fen.Append(emptyTilesInSection);
                    emptyTilesInSection = 0;
                    
                }
                
                if (pos != 63) fen.Append('/');

            }
            if (currentPiece is null) emptyTilesInSection++;
            // If the tile is null, either start or continue incrementing the section counter
            else
            {

                if (emptyTilesInSection > 0) fen.Append(emptyTilesInSection);
                fen.Append(currentPiece.PieceTypeChar);
                emptyTilesInSection = 0;

            }
            // If there is a Piece, append the char representing the piece (ex. white Pawn = 'P')

        }

        return fen.ToString();

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