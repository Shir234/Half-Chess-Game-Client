using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetFinalProjectWinForm
{
    public class Constants
    {

        public static readonly int PenDrawSize = 8;

        public static readonly int BoardLength = 8;
        public static readonly int BoardWidth = 4;
        public static readonly int BoardSquareSize = 90;
        public static readonly int BoardChessPieceSize = 80;
        public static readonly int BoardMargin = 20;
        public static readonly int BoardMarginTop = 80;

        public enum GAME_RESULT { Win, Tie, Lose };

        // public enum ChessPieceType { None = 0, WKing, BKing, WBishop, BBishop, WKnight, BKnight, WCastle, BCastle, WPawn, BPawn };

        /// http://localhost:40106
        /// https://localhost:7067/
        public static string SERVER_PATH_DOMAIN = "https://localhost:7067/";
        public static int SERVER_PORT = 40106;
        public static string SERVER_ENDPOINT_GET_USER = "/users";
        public static string PATH_URL_PUT_BOARD_UPDATE = "/game/update";

    }
}
