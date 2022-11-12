namespace Chuss2;

public abstract class ValidationResult
{

    public sealed class Valid : ValidationResult
    {
        
        public Piece? CapturedPiece { get; set; }

        public Valid(Piece? capturedPiece = null) => CapturedPiece = capturedPiece;

    }

    public sealed class Invalid : ValidationResult
    {
        
        public string? Reason { get; set; }

        public Invalid(string? reason = null) => Reason = reason;

    }

}