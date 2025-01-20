using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static dotnetFinalProjectWinForm.Models.Games;

namespace dotnetFinalProjectWinForm.Models
{
    internal class Move
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
        public ChessPieceType PieceType { get; set; }
        public int MoveNumber { get; set; }    
    }
}
