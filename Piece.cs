using System.Runtime.CompilerServices;

namespace Chuss2;

public class Piece
// The parent class for all types of Pieces (Pawn, Rook, Knight, Bishop, Queen, King)
// Cannot be instantiated directly
{

    public Point Position { get; set; }
    public char PieceTypeChar { get; set; }
    public bool IsWhite;

    protected Piece(Point position, bool isWhite)
    {

        Position = position;
        IsWhite = isWhite;

    }

    protected Piece(int column, int row, bool isWhite)
    {

        Position = new Point(column, row);
        IsWhite = isWhite;

    }

}

class Pawn : Piece
{

    public Pawn(Point position, bool isWhite) : base(position, isWhite)
    {

        PieceTypeChar = isWhite ? 'P' : 'p';

    }
    
    public Pawn(int column, int row, bool isWhite) : base(column, row, isWhite)
    {

        PieceTypeChar = isWhite ? 'P' : 'p';

    }

}

class Rook : Piece
{

    public Rook(Point position, bool isWhite) : base(position, isWhite)
    {

        PieceTypeChar = isWhite ? 'R' : 'r';

    }
    
    public Rook(int column, int row, bool isWhite) : base(column, row, isWhite)
    {

        PieceTypeChar = isWhite ? 'R' : 'r';

    }

}

class Knight : Piece
{

    public Knight(Point position, bool isWhite) : base(position, isWhite)
    {

        PieceTypeChar = isWhite ? 'N' : 'n';

    }
    
    public Knight(int column, int row, bool isWhite) : base(column, row, isWhite)
    {

        PieceTypeChar = isWhite ? 'N' : 'n';

    }

}

class Bishop : Piece
{

    public Bishop(Point position, bool isWhite) : base(position, isWhite)
    {

        PieceTypeChar = isWhite ? 'B' : 'b';

    }
    
    public Bishop(int column, int row, bool isWhite) : base(column, row, isWhite)
    {

        PieceTypeChar = isWhite ? 'B' : 'b';

    }

}

class Queen : Piece
{

    public Queen(Point position, bool isWhite) : base(position, isWhite)
    {

        PieceTypeChar = isWhite ? 'Q' : 'q';

    }
    
    public Queen(int column, int row, bool isWhite) : base(column, row, isWhite)
    {

        PieceTypeChar = isWhite ? 'Q' : 'q';

    }

}

class King : Piece
{

    public King(Point position, bool isWhite) : base(position, isWhite)
    {

        PieceTypeChar = isWhite ? 'K' : 'k';

    }
    
    public King(int column, int row, bool isWhite) : base(column, row, isWhite)
    {

        PieceTypeChar = isWhite ? 'K' : 'k';

    }

}
