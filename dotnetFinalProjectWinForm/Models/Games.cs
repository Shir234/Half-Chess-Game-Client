using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetFinalProjectWinForm.Models
{
    public class Games
    {
        public Player Player {  get; set; }

        // Enums to identify game pieces
        public enum ChessPieceType { None = 0, WKing, BKing, WBishop, BBishop, WKnight, BKnight, WCastle, BCastle, WPawn, BPawn };


        // FROM CONSTANCE - SAVE? DELETE?
        public static readonly int BoardLength = 8;
        public static readonly int BoardWidth = 4;
        public static readonly int BoardSquareSize = 90;
        public static readonly int BoardChessPieceSize = 80;
        public static readonly int BoardMargin = 20;

        // Variables to manage the game
        public bool isGameActive;                              // is the current game active ? 
        public bool isPlayerWin;
        public bool isPlayerTurn { get; set; }                 // is the user or server turn


        public ChessPieceType[,] GameBoard { get; set; }                    // Matrix - represent the game board

        private int WKingLocationX, BKingLocationX;
        private int WKingLocationY, BKingLocationY;             // King location on the board
        private int boardLength = BoardLength, boardWidth = BoardWidth;
        internal bool isCheck;

        public int Timer { get; set; }

        public void SetPlayerTurn()
        {
            isPlayerTurn = !isPlayerTurn;
        }

        // Move Piece --> set it in the new location and empty the last location on board
        public void MovePiece(ChessPieceType piece, int fromRow, int fromCol, int toRow, int toCol)
        {
            GameBoard[toRow, toCol] = piece;
            GameBoard[fromRow, fromCol] = ChessPieceType.None;
        }

        /// <summary>
        /// Check if the two pieces are of the same color.
        /// </summary>
        /// <param name="piece">The first piece</param>
        /// <param name="targetPiece">The second piece</param>
        /// <returns></returns>
        public bool IsSameColor(ChessPieceType piece, ChessPieceType targetPiece)
        {
            return (piece == ChessPieceType.WPawn || piece == ChessPieceType.WKing || piece == ChessPieceType.WKnight || piece == ChessPieceType.WBishop || piece == ChessPieceType.WCastle) &&
                   (targetPiece == ChessPieceType.WPawn || targetPiece == ChessPieceType.WKing || targetPiece == ChessPieceType.WKnight || targetPiece == ChessPieceType.WBishop || targetPiece == ChessPieceType.WCastle) ||
                   (piece == ChessPieceType.BPawn || piece == ChessPieceType.BKing || piece == ChessPieceType.BKnight || piece == ChessPieceType.BBishop || piece == ChessPieceType.BCastle) &&
                   (targetPiece == ChessPieceType.BPawn || targetPiece == ChessPieceType.BKing || targetPiece == ChessPieceType.BKnight || targetPiece == ChessPieceType.BBishop || targetPiece == ChessPieceType.BCastle);
        }
    }
}
