using System;

namespace API.Logic
{
    public class Position
    {
        public int Rank { get; set; }
        public int File { get; set; }

        public Position(int Rank, int File)
        {
            this.Rank = Rank;
            this.File = File;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Position otherPosition = (Position)obj;
            return Rank == otherPosition.Rank && File == otherPosition.File;
        }

        // Override GetHashCode as well when Equals is overridden
        public override int GetHashCode()
        {
            // Using a simple hashing algorithm; you might want to use a better one
            return Rank.GetHashCode() ^ File.GetHashCode();
        }
        public override string ToString()
        {
            return $"({Rank},{File})";
        }

    }
}
