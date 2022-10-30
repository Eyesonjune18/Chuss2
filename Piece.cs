using System.Runtime.CompilerServices;

namespace Chuss2;

public abstract class Piece
// The parent class for all types of Pieces (Pawn, Rook, Knight, Bishop, Queen, King)
{
    
    public char PieceTypeChar;
    protected readonly bool IsWhite;

    protected Piece(bool isWhite)
    {
        
        IsWhite = isWhite;

    }

    public virtual bool IsMoveLegal(Point source, Point destination)
    // Checks if the given move is legal for a given Piece type, irrespective of the current gamestate
    // Assumes that basic pre-checks have been done (source and destination tile are not the same, etc.)
    {

        return false;
        // This should never be used, as each Piece type has its own move validation

    }

}

internal class Pawn : Piece
{

    private bool _hasMoved;

    public Pawn(bool isWhite) : base(isWhite)
    {

        PieceTypeChar = isWhite ? 'P' : 'p';
        _hasMoved = false;

    }
    
    public override bool IsMoveLegal(Point source, Point destination)
    // Checks if the given move is legal for Pawns, irrespective of the current gamestate
    {

        int changeX = destination.X - source.X;
        int changeY = destination.Y - source.Y;

        if (!IsWhite) changeY *= -1;
        // Because Pawns move a different direction based on their color, flip the change in Y to reflect the Pawn color

        int allowedDistance = _hasMoved ? 1 : 2;
        // Allow for en passant if the piece has not moved

        if (changeX == 1 && changeY == 1) return true;
        // Pawn capture move - further validation in Game

        return (changeX == 0 && changeY == allowedDistance);
        // Standard move, adjusted based on en passant rule
        // This is the final condition, so return false if not true

    }

}

internal class Rook : Piece
{

    public Rook(bool isWhite) : base(isWhite)
    {

        PieceTypeChar = isWhite ? 'R' : 'r';

    }

    public override bool IsMoveLegal(Point source, Point destination)
    // Checks if the given move is legal for Rooks, irrespective of the current gamestate
    {

        int changeX = destination.X - source.X;
        int changeY = destination.Y - source.Y;
        // Absolute value could be taken here but is unnecessary

        return (changeX == 0 && changeY != 0) ^ (changeX != 0 && changeY == 0);
        // Move can be horizontal or vertical, not both

    }

}

internal class Knight : Piece
{

    public Knight(bool isWhite) : base(isWhite)
    {

        PieceTypeChar = isWhite ? 'N' : 'n';

    }

    public override bool IsMoveLegal(Point source, Point destination)
    // Checks if the given move is legal for Knights, irrespective of the current gamestate
    {

        int changeX = Math.Abs(destination.X - source.X);
        int changeY = Math.Abs(destination.Y - source.Y);
        // Absolute value is taken because Knights can move in any direction

        return (changeX == 1 && changeY == 2) ^ (changeX == 2 && changeY == 1);
        // Move can be in an L-shape in any direction

    }

}

internal class Bishop : Piece
{

    public Bishop(bool isWhite) : base(isWhite)
    {

        PieceTypeChar = isWhite ? 'B' : 'b';

    }

    public override bool IsMoveLegal(Point source, Point destination)
    // Checks if the given move is legal for Bishops, irrespective of the current gamestate
    {

        int changeX = Math.Abs(destination.X - source.X);
        int changeY = Math.Abs(destination.Y - source.Y);
        // Absolute value is taken because Bishops can move in any direction

        return (changeX == changeY);
        // Move can be diagonal, so it is legal as long as changeX == changeY

    }

}

internal class Queen : Piece
{

    public Queen(bool isWhite) : base(isWhite)
    {

        PieceTypeChar = isWhite ? 'Q' : 'q';

    }

    public override bool IsMoveLegal(Point source, Point destination)
    // Checks if the given move is legal for Queens, irrespective of the current gamestate
    {

        int changeX = Math.Abs(destination.X - source.X);
        int changeY = Math.Abs(destination.Y - source.Y);
        // Absolute value is taken because Queens can move in any direction

        return (changeX == 0 && changeY != 0) ^ (changeX != 0 && changeY == 0) ^ (changeX == changeY);
        // Queen move can be either the same as a Rook move or a Bishop move, not both

    }

}

internal class King : Piece
{

    public King(bool isWhite) : base(isWhite)
    {

        PieceTypeChar = isWhite ? 'K' : 'k';

    }

    public override bool IsMoveLegal(Point source, Point destination)
        // Checks if the given move is legal for Kings, irrespective of the current gamestate
    {

        int changeX = Math.Abs(destination.X - source.X);
        int changeY = Math.Abs(destination.Y - source.Y);
        // Absolute value is taken because Kings can move in any direction

        return changeX <= 1 && changeY <= 1;
        // King move can be no more than 1 tile in either/both directions

    }

}
