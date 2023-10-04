using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Logic
{
    public class Move
    {
        public Move(Position From, Position To)
        {
            this.From = From;
            this.To = To;
        }

        public Position From { get; set; }
        public Position To { get; set; }

    }
}