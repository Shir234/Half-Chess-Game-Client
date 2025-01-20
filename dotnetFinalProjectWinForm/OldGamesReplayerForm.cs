using dotnetFinalProjectWinForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static dotnetFinalProjectWinForm.Models.Games;

namespace dotnetFinalProjectWinForm
{
    public partial class OldGamesReplayerForm : Form
    {
        public GameRecordsEntities1 db = new GameRecordsEntities1();


        private GameRecorder recorder;
        private GameReplayer player;

        private int boardLength, boardWidth, boardSquareSize,
                    boardMargin, boardMarginTop;

        private Bitmap replayBitmap;

        private Color drawColor, moveColor, attackColor;

        public int selectedGameId;
        public ChessPieceType[,] GameBoard { get; set; }

        public OldGamesReplayerForm(int olgGameId)
        {
            InitializeComponent();

            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);

            this.UpdateStyles();
            this.DoubleBuffered = true;

            selectedGameId = olgGameId;
            setGameBoard();
            InitializeNewGame();

            // Init board size and properties
            boardLength = Constants.BoardLength;
            boardWidth = Constants.BoardWidth;
            boardSquareSize = Constants.BoardSquareSize;
            boardMargin = Constants.BoardMargin;
            boardMarginTop = Constants.BoardMarginTop;

            // Init colors
            drawColor = Color.Red;
            moveColor = Color.PaleGreen;
            attackColor = Color.Red;

            replayBitmap = new Bitmap(
                boardWidth * boardSquareSize + (2 * boardMargin),
                boardLength * boardSquareSize + boardMarginTop + boardMargin);

            using (Graphics g = Graphics.FromImage(replayBitmap))
            {
                DrawGame(g);
            }
        }


        private void OldGamesReplayerForm_Load(object sender, EventArgs e)
        {
            // Get the selected game from your database
            GameRecord savedGame = LoadGameFromDatabase(selectedGameId);
            printMoves(savedGame);
            player = new GameReplayer(savedGame, this);
            player.StartReplay();
        }

        private void printMoves(GameRecord savedGame)
        {
            Console.WriteLine("Recoed:");
            foreach (var move in savedGame.Moves)
            {
                Console.WriteLine($"MoveId = {move.MoveNumber}. \n[{move.FromRow},{move.FromCol}] to [{move.ToRow}, {move.ToCol}] by {(ChessPieceType)(int)move.PieceType}");
                //db.TblMoves.Add(new TblMoves
                //{
                //    GameId = gameToAdd.GameId,  // Link to the parent game
                //    FromRow = move.FromRow,
                //    FromCol = move.FromCol,
                //    ToRow = move.ToRow,
                //    ToCol = move.ToCol,
                //    PieceType = (int)move.PieceType,
                //    MoveNumber = move.MoveNumber
                //});
            }
            Console.WriteLine(savedGame.ToString());
            //var move = savedGame.Moves[currentMoveIndex++]
            ;

        }

        private void InitializeNewGame()
        {
            recorder = new GameRecorder();
            recorder.StartNewGame();
        }

        private GameRecord LoadGameFromDatabase(int gameId)
        {
            var game = db.TblGame.Include("TblMoves").FirstOrDefault(g => g.GameId == gameId);
            if (game == null)
            {
                return null; // Return null if the game is not found
            }

            return new GameRecord
            {
                GameId = game.GameId,
                Result = (Constants.GAME_RESULT)game.Result,

                Moves = game.TblMoves.Select(m => new Move
                {
                    MoveNumber = m.MoveId,
                    FromRow = m.FromRow,
                    ToRow = m.ToRow,
                    FromCol = m.FromCol,
                    ToCol = m.ToCol,
                    PieceType = (ChessPieceType)m.PieceType
                }).ToList()
            };
        }

        private void OldGamesReplayerForm_Paint(object sender, PaintEventArgs e)
        {
            if (replayBitmap != null)
            {
                e.Graphics.DrawImage(replayBitmap, 0, 0);
            }
        }

        private void setGameBoard()
        {
            this.GameBoard = new ChessPieceType[,]
            { { ChessPieceType.BKing, ChessPieceType.BKnight, ChessPieceType.BBishop, ChessPieceType.BCastle},
                { ChessPieceType.BPawn, ChessPieceType.BPawn, ChessPieceType.BPawn, ChessPieceType.BPawn},
                { 0, 0, 0, 0},
                { 0, 0, 0, 0},
                { 0, 0, 0, 0},
                { 0, 0, 0, 0},
                { ChessPieceType.WPawn, ChessPieceType.WPawn, ChessPieceType.WPawn, ChessPieceType.WPawn},
                { ChessPieceType.WKing, ChessPieceType.WKnight, ChessPieceType.WBishop, ChessPieceType.WCastle} };
        }

        /// <summary>
        /// Draw the chess game
        /// </summary>
        /// <param name="graphicsBoard"></param>
        private void DrawGame(Graphics graphicsBoard)
        {
            DrawBoard(graphicsBoard);
            PlacePiecesOnBoard(graphicsBoard);
        }

        /// <summary>
        /// Add the chess pieces to the board
        /// </summary>
        /// <param name="g"></param>
        private void PlacePiecesOnBoard(Graphics g)
        {
            ChessPieceType[,] gameBoard = GameBoard;
            int chessPieceSize = Constants.BoardChessPieceSize;

            for (int row = 0; row < boardLength; row++)
            {
                for (int col = 0; col < boardWidth; col++)
                {
                    int pieceDeviation = (boardSquareSize - chessPieceSize) / 2;
                    Image chessPieceImage = getImageByEnum(gameBoard[row, col]);

                    g.DrawImage(chessPieceImage,
                        (col * boardSquareSize) + pieceDeviation + boardMargin, (row * boardSquareSize) + pieceDeviation + boardMarginTop,
                        chessPieceSize, chessPieceSize);
                }
            }
        }

        /// <summary>
        /// Get image file from resources by the chess piece
        /// </summary>
        /// <param name="chessPieceType"></param>
        /// <returns></returns>
        private Image getImageByEnum(ChessPieceType chessPieceType)
        {
            Image image = null;
            switch (chessPieceType)
            {
                case ChessPieceType.WKing:
                    image = Properties.Resources.WKing;
                    break;
                case ChessPieceType.BKing:
                    image = Properties.Resources.BKing;
                    break;
                case ChessPieceType.WKnight:
                    image = Properties.Resources.WKnight;
                    break;
                case ChessPieceType.BKnight:
                    image = Properties.Resources.BKnight;
                    break;
                case ChessPieceType.WBishop:
                    image = Properties.Resources.WBishop;
                    break;
                case ChessPieceType.BBishop:
                    image = Properties.Resources.BBishop;
                    break;
                case ChessPieceType.WCastle:
                    image = Properties.Resources.WCastle;
                    break;
                case ChessPieceType.BCastle:
                    image = Properties.Resources.BCastle;
                    break;
                case ChessPieceType.WPawn:
                    image = Properties.Resources.WPawn;
                    break;
                case ChessPieceType.BPawn:
                    image = Properties.Resources.BPawn;
                    break;
                default:
                    image = new Bitmap(1, 1);
                    break;
            }
            return image;
        }

        /// <summary>
        /// Draw the board squares
        /// </summary>
        /// <param name="g"></param>
        private void DrawBoard(Graphics g)
        {
            for (int i = 0; i < Constants.BoardLength; i++)
            {
                for (int j = 0; j < Constants.BoardWidth; j++)
                {
                    Color color;
                    if ((i % 2 == 0 && j % 2 == 0) || (i % 2 != 0 && j % 2 != 0))
                    {
                        color = Color.BurlyWood;
                    }
                    else
                    {
                        color = Color.Brown;
                    }

                    SolidBrush squareColor = new SolidBrush(color);
                    g.FillRectangle(squareColor,
                        (j * boardSquareSize) + boardMargin, (i * boardSquareSize) + boardMarginTop,
                        boardSquareSize, boardSquareSize);

                    Pen squareBorder = new Pen(Color.Black);
                    g.DrawRectangle(squareBorder,
                    (j * boardSquareSize) + boardMargin, (i * boardSquareSize) + boardMarginTop,
                        boardSquareSize, boardSquareSize);
                }
            }

        }

        internal void MovePiece(ChessPieceType pieceType, int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine("Moved");
            GameBoard[toRow, toCol] = pieceType;
            GameBoard[fromRow, fromCol] = ChessPieceType.None;

            // Redraw the board with the updated state
            using (Graphics g = Graphics.FromImage(replayBitmap))
            {
                DrawGame(g);
            }

            // Request a redraw of the form
            this.Invalidate();
            this.Update();
        }

        public void resetBoard()
        {
            using (Graphics g = Graphics.FromImage(replayBitmap))
            {
                DrawGame(g);
            }
            this.Invalidate();
        }
    }
}
