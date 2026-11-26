using System;

namespace Chess2D.Model
{
    public readonly struct Square : IEquatable<Square>
    {
        public int X { get; }
        public int Y { get; }

        public Square(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Square Offset(int x, int y) => new(X + x, Y + y);

        public bool Equals(Square other) => X == other.X && Y == other.Y;
        public override bool Equals(object obj) => obj is Square other && Equals(other);
        public override int GetHashCode() => (X, Y).GetHashCode();
        public static bool operator ==(Square left, Square right) => left.Equals(right);
        public static bool operator !=(Square left, Square right) => !left.Equals(right);
    }
}
