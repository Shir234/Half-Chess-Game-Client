using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static dotnetFinalProjectWinForm.Models.Games;

namespace dotnetFinalProjectWinForm
{
    public class GamePiece
    {
        private string Name { get; set; }
        private int X { get; set; }
        private int Y { get; set; }

        private ChessPieceType PieceType { get; set; }
        
    }
}
