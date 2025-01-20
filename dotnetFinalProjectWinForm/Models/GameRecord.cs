using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static dotnetFinalProjectWinForm.Constants;

namespace dotnetFinalProjectWinForm.Models
{
    internal class GameRecord
    {
        public int GameId { get; set; }
        public DateTime GameDate { get; set; }
        public Constants.GAME_RESULT Result { get; set; }
        public List<Move> Moves { get; set; }
    }
}
