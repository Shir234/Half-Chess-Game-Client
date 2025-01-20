using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using dotnetFinalProjectWinForm;
using static dotnetFinalProjectWinForm.Constants;
using static dotnetFinalProjectWinForm.Models.Games;
using dotnetFinalProjectWinForm.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace dotnetFinalProjectWinForm
{
    public partial class GameForm : Form
    {
        public GameRecorder gameRecorder = new GameRecorder();

        private Bitmap bitmap, playerDrawingBitmap;
        private Graphics graphicsBoard;

        private bool isDrawMode;

        private int drawSize,
                    mouseDrawLocationX,mouseDrawOldLocationX,
                    mouseDrawLocationY,mouseDrawOldLocationY;

        private int boardLength, boardWidth, boardSquareSize,
                    boardMargin, boardMarginTop;

        private Color drawColor, moveColor, attackColor;

        private bool isFirstInitOrDrawingReset;
        private bool didUserSelectPiece;
        int selectedPieceRow, selectedPieceCol;

        private ChessPieceType selectedPiece;

        private MainMenuForm MainForm;
        private Games gameMng;

        private int CountdownTime { get; set; }
        private System.Windows.Forms.Timer CountdownTimer;

        private System.Windows.Forms.Timer BlinkTimer;          // Secondary timer for blinking
        private bool IsBlinking = false;                        // Flag to toggle visibility
      
        private int serverSelectedPieceEndRow;
        private int serverSelectedPieceEndCol;

        private bool didSendGameOver = false;

        public GameForm(MainMenuForm mainForm)
        {
            InitializeComponent();
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);

            this.UpdateStyles();

            MainForm = mainForm;
            gameMng = mainForm.gameMng;

            InitializeForm();
            InitializeConstants();

            Console.WriteLine("END OF GAME FORM C'TOR");

            gameRecorder.StartNewGame();
        }

        /// <summary>
        /// Init constants of the game
        /// </summary>
        private void InitializeConstants()
        {
            isDrawMode = false;

            //Init pen size for drawing and mouse location for drawing
            drawSize = Constants.PenDrawSize;
            mouseDrawLocationX = -Constants.PenDrawSize;
            mouseDrawOldLocationX = -Constants.PenDrawSize;
            mouseDrawLocationY = -Constants.PenDrawSize;
            mouseDrawOldLocationY = -Constants.PenDrawSize;

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
            
            isFirstInitOrDrawingReset = true;

            // Init player selection
            didUserSelectPiece = false;
            selectedPieceRow = -1;
            selectedPieceCol = -1;
        }

        /// <summary>
    /// Init the form properties.
    /// </summary>
        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.DrawOrPlayBtn.Text = "Draw";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            CountdownTime = gameMng.Timer;

            CountdownTimer = new System.Windows.Forms.Timer();
            CountdownTimer.Interval = 1000;
            CountdownTimer.Tick += CountdownTimer_Tick;

            BlinkTimer = new System.Windows.Forms.Timer();
            BlinkTimer.Interval = 500;
            BlinkTimer.Tick += BlinkTimer_Tick;
            //set label
            CountdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (CountdownTime > 0)
            {
                CountdownTime--;
                //update label
                counter.Text = TimeSpan.FromSeconds(CountdownTime).ToString(@"mm\:ss");
                
                if(CountdownTime == 5)
                {
                    counter.ForeColor = Color.Red;
                    BlinkTimer.Start();
                }
            }
            else
            {
                CountdownTimer.Stop();
                BlinkTimer.Stop();
                counter.Visible = true;
                counter.Text = "Time's up!";
                if (gameMng.isPlayerTurn)
                {
                    GameOver(Constants.GAME_RESULT.Lose);
                }
                else
                {
                    GameOver(Constants.GAME_RESULT.Win);
                }
            }
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            counter.Visible = !counter.Visible;
        }

        private async void GameOver(GAME_RESULT result)
        {
            didSendGameOver = true;
            // Pop a dialog box with the result of the game.
            string resultStr = "You " + result.ToString() + "!";
            DialogResult dialogResult = MessageBox.Show(resultStr, resultStr, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

            gameRecorder.SaveGameToDB(result);
             
            await PutGameOver(gameMng.Player.Id, result);
            // On 'Ok' close the game.
            if (dialogResult == DialogResult.OK)
            {
              //  await PutGameOver(gameMng.PlayerId, result);
                this.Close();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if(gameMng.isPlayerTurn)
            {
                whosTurn.Text = $"{gameMng.Player.Name.Trim()} turn!";
                whosTurn.ForeColor = Color.Black;
            } 
            else
            {
                whosTurn.Text = $"Server turn!";
                whosTurn.ForeColor = Color.Coral;
            }
            if (isFirstInitOrDrawingReset)  // Init the game board
            {
                bitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
                graphicsBoard = Graphics.FromImage(bitmap);
                DrawGame(graphicsBoard);
                isFirstInitOrDrawingReset = false;
            }
            else
            {
                graphicsBoard = Graphics.FromImage(bitmap);
            }
            e.Graphics.Clear(this.BackColor);

            Graphics playerDrawingGraphics = Graphics.FromImage(bitmap);

            if (mouseDrawOldLocationX > 0 && mouseDrawLocationX > 0)    // Draw line between two points only if it's a valid coordinate
            {
                Pen drawPen = new Pen(Color.Red, drawSize);
                graphicsBoard.DrawLine(drawPen,
                    mouseDrawOldLocationX, mouseDrawOldLocationY,
                    mouseDrawLocationX, mouseDrawLocationY);

                drawPen.Dispose();
            }
            graphicsBoard.FillEllipse(Brushes.Red,
            mouseDrawLocationX - drawSize / 2, mouseDrawLocationY - drawSize / 2,
            drawSize, drawSize);

            e.Graphics.DrawImage(bitmap, 0, 0);
            graphicsBoard.Dispose();
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
            ChessPieceType[,] gameBoard = gameMng.GameBoard;
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
            switch (chessPieceType) {
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
                    image = new Bitmap(1,1);
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

        /// <summary>
        /// Move the piece on the board.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="fromRow">The current row</param>
        /// <param name="fromCol">The current col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end row</param>
        public void MovePiece(ChessPieceType piece, int fromRow, int fromCol, int toRow, int toCol)
        {
            gameMng.MovePiece(piece, fromRow, fromCol, toRow, toCol);

            // Redraw the board to reflect the move
            this.Invalidate();
            this.Update();
        }

        /// <summary>
        /// Redraw the board
        /// </summary>
        public void ResetBoard()
        {
            isFirstInitOrDrawingReset = true;
            mouseDrawLocationX = -drawSize;
            mouseDrawLocationY = -drawSize;
            DrawBoard(Graphics.FromImage(bitmap));
            this.Invalidate();
            this.Update();
        }

        /// <summary>
        /// Draw the legal moves on the screen for the chosen piece
        /// </summary>
        /// <param name="chosenPiece">The chosen piece</param>
        /// <param name="boardRow">The current piece row</param>
        /// <param name="boardCol">The current piece col</param>
        private void ShowLegalMovesForChessPiece(ChessPieceType chosenPiece, int boardRow, int boardCol)
        {
            switch (chosenPiece)
            {
                case ChessPieceType.WPawn:
                //case ChessPieceType.BPawn:
                    ShowLegalMovesForPawn(boardRow, boardCol, chosenPiece);
                    break;
                case ChessPieceType.WKing:
                //case ChessPieceType.BKing:
                    ShowLegalMovesForKing(boardRow, boardCol);
                    break;
                case ChessPieceType.WKnight:
               // case ChessPieceType.BKnight:
                    ShowLegalMovesForKnight(boardRow, boardCol);
                    break;
                case ChessPieceType.WBishop:
                //case ChessPieceType.BBishop:
                    ShowLegalMovesForBishop(boardRow, boardCol);
                    break;
                case ChessPieceType.WCastle:
                //case ChessPieceType.BCastle:
                    ShowLegalMovesForCastle(boardRow, boardCol);
                    break;
            }
        }

        /// <summary>
        /// Draw the legal moves on the screen for the castle piece
        /// </summary>
        /// <param name="boardRow">The current row</param>
        /// <param name="boardCol">The current col</param>
        private void ShowLegalMovesForCastle(int boardRow, int boardCol)
        {
            Console.WriteLine("Castle clicked, location: [" + boardRow + ", " + boardCol + "]");

            ResetBoard();

            Graphics pieceGraphics = this.CreateGraphics();     // Get graphics instance
            Bitmap chosenPieceBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            Graphics chosenPieceGraphics = Graphics.FromImage(chosenPieceBitmap);
            ChessPieceType[,] boardGame = gameMng.GameBoard;

            ShowLegalMovesForCastleInDirection(boardCol, boardRow, colDirection: 0, rowDirection: 1, chosenPieceGraphics, boardGame);
            ShowLegalMovesForCastleInDirection(boardCol, boardRow, colDirection: 0, rowDirection: -1, chosenPieceGraphics, boardGame);
            ShowLegalMovesForCastleInDirection(boardCol, boardRow, colDirection: 1, rowDirection: 0, chosenPieceGraphics, boardGame);
            ShowLegalMovesForCastleInDirection(boardCol, boardRow, colDirection: -1, rowDirection: 0, chosenPieceGraphics, boardGame);
            
            pieceGraphics.DrawImage(chosenPieceBitmap, 0, 0);
            PlacePiecesOnBoard(pieceGraphics);
        }

        private void ShowLegalMovesForCastleInDirection(int boardCol, int boardRow, int colDirection, int rowDirection, Graphics chosenPieceGraphics, ChessPieceType[,] boardGame)
        {
            int newBoardRow = boardRow;
            int newBoardCol = boardCol;

            // Keep moving in the given direction until the edge of the board or blocked by a piece
            while (true)
            {
                newBoardRow += rowDirection;
                newBoardCol += colDirection;

                // Check if the new position is within bounds
                if (newBoardRow < 0 || newBoardRow >= boardLength || newBoardCol < 0 || newBoardCol >= boardWidth)
                {
                    break; // Out of bounds, stop checking further
                }

                ChessPieceType castlePiece = boardGame[boardRow, boardCol],
                    targetLocation = boardGame[newBoardRow, newBoardCol];

                // If the square is empty, color it as a legal move
                if (targetLocation == ChessPieceType.None)
                {
                    ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, boardRow: newBoardRow, boardCol: newBoardCol);
                }
                // Check what kind of piecee is on the square
                else if (!gameMng.IsSameColor(piece: castlePiece, targetPiece: targetLocation))
                {
                    ColorLegalMoves(chosenPieceGraphics, moveColor: attackColor, newBoardRow, newBoardCol);
                    break;
                }
                else
                {
                    break; // Stop if the square is blocked by a piece
                }
            }
        }

        /// <summary>
        /// Draw the legal moves on the screen for the bishop piece
        /// </summary>
        /// <param name="boardRow">The current row</param>
        /// <param name="boardCol">The current col</param>
        private void ShowLegalMovesForBishop(int boardRow, int boardCol)
        {
            Console.WriteLine("Bishop clicked, location: [" + boardRow + ", " + boardCol + "]");

            ResetBoard();
            Graphics pieceGraphics = this.CreateGraphics();
            Bitmap chosenPieceBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            Graphics chosenPieceGraphics = Graphics.FromImage(chosenPieceBitmap);
            ChessPieceType[,] boardGame = gameMng.GameBoard;

            // Diagonal moves (4 directions) for Bishop
            int[] directions = new int[] { -1, 1 };
            foreach (int rowDirection in directions)
            {
                foreach (int colDirection in directions)
                {
                    int newBoardRow = boardRow, newBoardCol = boardCol;

                    while (true)
                    {
                        newBoardRow += rowDirection;
                        newBoardCol += colDirection;

                        if (newBoardRow < 0 || newBoardRow >= boardLength || newBoardCol < 0 || newBoardCol >= boardWidth)
                            break;

                        ChessPieceType bishopPiece = boardGame[boardRow, boardCol],
                            targetLocation = boardGame[newBoardRow, newBoardCol];

                        // Check if the square is empty
                        if (targetLocation == ChessPieceType.None)
                        {
                            ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, newBoardRow, newBoardCol);
                        }
                        else if (!gameMng.IsSameColor(piece: bishopPiece, targetPiece: targetLocation))
                        {
                            ColorLegalMoves(chosenPieceGraphics, moveColor: attackColor, newBoardRow, newBoardCol);
                            break;
                        }
                        else
                        {
                            break; // Stop if the square is blocked by a piece
                        }
                    }
                }
            }

            pieceGraphics.DrawImage(chosenPieceBitmap, 0, 0);
            PlacePiecesOnBoard(pieceGraphics);
        }

        /// <summary>
        /// Draw the legal moves on the screen for the knight piece
        /// </summary>
        /// <param name="boardRow">The current row</param>
        /// <param name="boardCol">The current col</param>
        private void ShowLegalMovesForKnight(int boardRow, int boardCol)
        {
            Console.WriteLine("Knight clicked, location: [" + boardRow + ", " + boardCol + "]");

            ResetBoard();
            Graphics pieceGraphics = this.CreateGraphics();
            Bitmap chosenPieceBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            Graphics chosenPieceGraphics = Graphics.FromImage(chosenPieceBitmap);
            ChessPieceType[,] boardGame = gameMng.GameBoard;
            // 8 possible L-shaped moves for Knight
            int[] rowMoves = new int[] { -2, -1, 1, 2,  2,  1, -1, -2 };
            int[] colMoves = new int[] {  1,  2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                int newBoardRow = boardRow + rowMoves[i];
                int newBoardCol = boardCol + colMoves[i];

                if (newBoardRow >= 0 && newBoardRow < boardLength && newBoardCol >= 0 && newBoardCol < boardWidth)
                {
                    ChessPieceType knightPiece = boardGame[boardRow, boardCol],
                        targetLocation = boardGame[newBoardRow, newBoardCol];

                    // Check if the square is empty
                    if (targetLocation == ChessPieceType.None)
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, newBoardRow, newBoardCol);
                    }
                    // Check what kind of piecee is on the square
                    else if (!gameMng.IsSameColor(piece: knightPiece, targetPiece: targetLocation))
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: attackColor, newBoardRow, newBoardCol);
                    }
                }
            }

            pieceGraphics.DrawImage(chosenPieceBitmap, 0, 0);
            PlacePiecesOnBoard(pieceGraphics);
        }

        /// <summary>
        /// Draw the legal moves on the screen for the king piece
        /// </summary>
        /// <param name="boardRow">The current row</param>
        /// <param name="boardCol">The current col</param>
        private void ShowLegalMovesForKing(int boardRow, int boardCol)
        {
            Console.WriteLine("King clicked, location: [" + boardRow + ", " + boardCol + "]");

            ResetBoard();
            Graphics pieceGraphics = this.CreateGraphics();
            Bitmap chosenPieceBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            Graphics chosenPieceGraphics = Graphics.FromImage(chosenPieceBitmap);
            ChessPieceType[,] boardGame = gameMng.GameBoard;

            for (int newBoardRow = boardRow - 1; newBoardRow <= boardRow + 1; newBoardRow++)
            {
                for (int newBoardCol = boardCol - 1; newBoardCol <= boardCol + 1; newBoardCol++)
                {
                    if (newBoardRow == boardRow && newBoardCol == boardCol)
                    {
                        continue;
                    }
                    if (newBoardRow < 0 || newBoardRow >= boardLength)
                    {
                        continue;
                    } 
                    if(newBoardCol < 0 || newBoardCol >= boardWidth)
                    {
                        continue;
                    }

                    ChessPieceType KingPiece = boardGame[boardRow, boardCol],
                        targetLocation = boardGame[newBoardRow, newBoardCol];

                    // Check if the square is empty
                    if (targetLocation == ChessPieceType.None)
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, newBoardRow, newBoardCol);
                    }
                    // Check what kind of piece is on the square
                    else if (!gameMng.IsSameColor(piece: KingPiece, targetPiece: targetLocation))
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: attackColor, newBoardRow, newBoardCol);
                    }

                }
            }
            pieceGraphics.DrawImage(chosenPieceBitmap, 0, 0);
            PlacePiecesOnBoard(pieceGraphics);
        }

        /// <summary>
        /// Draw the legal moves on the screen for the pawn piece
        /// </summary>
        /// <param name="boardRow">The current row</param>
        /// <param name="boardCol">The current col</param>
        /// <param name="pawnType"></param>
        private void ShowLegalMovesForPawn(int boardRow, int boardCol, ChessPieceType pawnType)
        {
            Console.WriteLine("Pawn clicked, location: [" + boardRow + ", " + boardCol+ "]");

            ResetBoard();
            Graphics pieceGraphics = this.CreateGraphics();
            Bitmap chosenPieceBitmap = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            Graphics chosenPieceGraphics = Graphics.FromImage(chosenPieceBitmap);
            ChessPieceType[,] boardGame = gameMng.GameBoard;

            int direction = (pawnType == ChessPieceType.WPawn) ? -1 : 1;
            int startRow = (pawnType == ChessPieceType.WPawn) ? 6 : 1;

            int newBoardRow = boardRow + direction;

            // Move forward by 1 square
            if (newBoardRow >= 0 && newBoardRow < boardLength)
            {
                ChessPieceType targetLocation = boardGame[newBoardRow, boardCol];

                if (targetLocation == ChessPieceType.None)
                {
                    ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, newBoardRow, boardCol );
                }
            }

            // Special move: Pawn can move 2 squares from its initial position
            if (boardRow == startRow)
            {
                newBoardRow = boardRow + (2 * direction);
                if (boardGame[newBoardRow, boardCol] == ChessPieceType.None)
                {
                    ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, boardRow + (2 * direction), boardCol);
                }
            }

            if (boardRow >= 0 && boardRow < boardLength)
            {
                // Horizontal left (to the left side of the board)
                if (boardCol - 1 >= 0)
                {
                    if (boardGame[boardRow, boardCol - 1] == ChessPieceType.None) // The square is empty
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, boardRow, boardCol - 1);
                    }
                }

                // Horizontal right (to the right side of the board)
                if (boardCol + 1 < boardWidth)
                {
                    if (boardGame[boardRow, boardCol + 1] == ChessPieceType.None) // The square is empty
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: moveColor, boardRow, boardCol + 1);
                    }
                }
            }

            int newBoardCol = boardCol - 1; // Check left side first
            // Capture diagonally
            if (newBoardRow >= 0 && newBoardRow < boardLength)
            {
                ChessPieceType pawnPiece, targetLocation;

                // Capture left diagonally
                if (newBoardCol >= 0)
                {
                    pawnPiece = boardGame[boardRow, boardCol];
                    targetLocation = boardGame[newBoardRow, newBoardCol];
                    if (targetLocation != ChessPieceType.None)
                    {
                        if (!gameMng.IsSameColor(piece: pawnType, targetPiece: targetLocation))   // Not the same color
                        {
                            ColorLegalMoves(chosenPieceGraphics, moveColor: attackColor, newBoardRow, newBoardCol);
                        }
                    }
                }

                newBoardCol = boardCol + 1;
                if (newBoardCol < boardWidth)
                {
                    targetLocation = boardGame[newBoardRow, newBoardCol];

                    // Capture right diagonally
                    if (newBoardCol < boardWidth && targetLocation != ChessPieceType.None &&
                        !gameMng.IsSameColor(piece: pawnType, targetPiece: targetLocation)) // Not the same color
                    {
                        ColorLegalMoves(chosenPieceGraphics, moveColor: attackColor, newBoardRow, newBoardCol);
                    }
                }
            }

            pieceGraphics.DrawImage(chosenPieceBitmap, 0, 0);
            PlacePiecesOnBoard(pieceGraphics);
        }

        /// <summary>
        /// Draw the legal moves on the board with the relevant color.
        /// </summary>
        /// <param name="chosenPieceGraphics"></param>
        /// <param name="moveColor">The color</param>
        /// <param name="boardRow"></param>
        /// <param name="boardCol"></param>
        private void ColorLegalMoves(Graphics chosenPieceGraphics, Color moveColor, int boardRow, int boardCol)
        {
            Color legalMovesColor = Color.FromArgb(150, moveColor);

            SolidBrush squareColor = new SolidBrush(legalMovesColor);
            chosenPieceGraphics.FillRectangle(squareColor,
                (boardCol * boardSquareSize) + boardMargin, (boardRow * boardSquareSize) + boardMarginTop,
                boardSquareSize, boardSquareSize);

            Pen squareBorder = new Pen(Color.Black);
            chosenPieceGraphics.DrawRectangle(squareBorder,
                (boardCol * boardSquareSize) + boardMargin, (boardRow * boardSquareSize) + boardMarginTop,
                boardSquareSize, boardSquareSize);

            squareColor.Dispose();
            squareBorder.Dispose();
        }

        /// <summary>
        /// Convert X-coordinate and Y-coordinate to Row and Column -> and return the game piece there
        /// </summary>
        /// <param name="boardX"></param>
        /// <param name="boardY"></param>
        /// <param name="mouseClickRow"></param>
        /// <param name="mouseClickCol"></param>
        /// <returns></returns>
        private ChessPieceType GetMouseClickOnPiece(int boardX, int boardY, out int mouseClickCol, out int mouseClickRow)
        {
            mouseClickCol = (boardX - boardMargin) / boardSquareSize;
            mouseClickRow = (boardY - boardMarginTop) / boardSquareSize;

            Console.WriteLine("isMouseClickOnBoard [row, col] = [" + mouseClickRow + ", " + mouseClickCol + "]");

            ChessPieceType[,] gameBoard = gameMng.GameBoard;

            Console.WriteLine(gameBoard[mouseClickRow, mouseClickCol]);

            ChessPieceType chosenPiece = gameBoard[mouseClickRow, mouseClickCol];
            return chosenPiece;
        }

        /// <summary>
        /// Determine if the mouse click was within those limits
        /// </summary>
        /// <param name="boardX"></param>
        /// <param name="boardY"></param>
        /// <returns> true if the even in the limits </returns>
        private bool IsMouseClickOnBoard(int boardX, int boardY)
        {
            int boardXStartLimit = boardMargin,
                boardYStartLimit = boardMarginTop,
                boardXEndLimit = (boardSquareSize * boardWidth + boardMargin),
                boardYEndLimit = (boardSquareSize * boardLength + boardMarginTop);

            if (!(boardX > boardXStartLimit && boardY > boardYStartLimit) ||
                !(boardX < boardXEndLimit && boardY < boardYEndLimit))
            {
                return false;
            }

            return true;
        }

        private async void Form1_MouseClick(object sender, MouseEventArgs e)
        {         
            if (e.Button == MouseButtons.Left)
            {
                if (isDrawMode)
                {
                    return;
                }
                int mouseX = e.X, mouseY = e.Y;

                if (!IsMouseClickOnBoard(mouseX, mouseY))
                {
                    return;
                }
                int mouseClickCol, mouseClickRow;
                ChessPieceType chosenPiece = GetMouseClickOnPiece(mouseX, mouseY, out mouseClickCol, out mouseClickRow);
                if (!didUserSelectPiece && (chosenPiece == ChessPieceType.None || 
                    gameMng.IsSameColor(chosenPiece, ChessPieceType.BPawn)))           // Check if user selected a piece 
                {
                    return;
                }

                if (!didUserSelectPiece)
                {
                    whosTurn.Text = $"{gameMng.Player.Name} turn";
                    whosTurn.ForeColor = Color.Black;
                    ChoosePiece(mouseClickCol, mouseClickRow, chosenPiece);
                }
                else if (didUserSelectPiece)
                {
                    // Send the move to the server to determine if the move is legal
                    if (!await PutMovePiece(gameMng.Player.Id, selectedPiece, selectedPieceRow, selectedPieceCol, mouseClickRow, mouseClickCol))
                    {
                        whosTurn.Text = $"{gameMng.Player.Name} turn: illegal move, try again!";
                        whosTurn.ForeColor = Color.Red;
                        // if the timer still valid - the player can do other move?
                        didUserSelectPiece = false;                         // Deselect the piece after move
                        ResetBoard();                                       // Clear legal move highlights
                        return;
                    }
                    Console.WriteLine("FIRST - PUT: PutMovePiece");
                    await MoveAndSwitchTurns(mouseClickCol, mouseClickRow);
                }
            }
        }

        private async Task MoveAndSwitchTurns(int mouseClickCol, int mouseClickRow)
        {
            // if legal -> make the move 
            await MovePieceOnBoard(mouseClickCol, mouseClickRow, Constants.GAME_RESULT.Win);
            UpdateTurnProperties(cursor: Cursors.WaitCursor, isEnabled: false);

            Console.WriteLine("SERVER TURN!");
            whosTurn.Text = $"Server turn";
            whosTurn.ForeColor = Color.Coral;
            // ask the server to make a move
            
            await GetServerMove();
            await MovePieceOnBoard(serverSelectedPieceEndCol, serverSelectedPieceEndRow, Constants.GAME_RESULT.Lose);

            UpdateTurnProperties(cursor: Cursors.Default, isEnabled: true);
            Console.WriteLine("AFTER SERVER TURN - PLAYER AGAIN!");

            whosTurn.Text = $"{gameMng.Player.Name} turn";
            whosTurn.ForeColor = Color.Black;

            didUserSelectPiece = false;                         // Deselect the piece after move
        }

        private void DrawOrPlayBtn_Click(object sender, EventArgs e)
        {
            isDrawMode = !isDrawMode;
            
            this.DrawOrPlayBtn.Text = isDrawMode ? "Play" : "Draw";
        }

        private void UpdateTurnProperties(Cursor cursor, bool isEnabled)
        {
            // reset the timer
            CountdownTime = gameMng.Timer;
            // set player turn
            Console.WriteLine("");
            gameMng.SetPlayerTurn();

            // Block the player from clicking on the pieces and change cursor to waiting.
            this.Cursor = cursor;
            this.Enabled = isEnabled;
        }

        private async Task MovePieceOnBoard(int mouseClickCol, int mouseClickRow, GAME_RESULT result)
        {
            MovePiece(selectedPiece, selectedPieceRow, selectedPieceCol, mouseClickRow, mouseClickCol);
            gameRecorder.RecordMove(selectedPiece, selectedPieceRow, selectedPieceCol, mouseClickRow, mouseClickCol);
            ResetBoard();

            if (!await GetGameState())
            {
                //Did not receive answer from the server
                // ERROR : PLAYER WON no connection
                GameOver(GAME_RESULT.Win);
            }

            if (!gameMng.isGameActive)
            {
                whosTurn.Text = $"GAME OVER!";
                whosTurn.ForeColor = Color.Blue;
                CountdownTimer.Stop();
                //If the game is over after player turn - the player won
                GameOver(result);
            }
        }

        private void ChoosePiece(int mouseClickCol, int mouseClickRow, ChessPieceType chosenPiece)
        {
            selectedPiece = chosenPiece;
            selectedPieceRow = mouseClickRow;
            selectedPieceCol = mouseClickCol;
            didUserSelectPiece = true;
            ShowLegalMovesForChessPiece(selectedPiece, selectedPieceRow, selectedPieceCol);
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!isDrawMode)
                {
                    return;
                }
                // Draw on board
                mouseDrawOldLocationX = mouseDrawLocationX;
                mouseDrawLocationX = e.Location.X;
                mouseDrawOldLocationY = mouseDrawLocationY;
                mouseDrawLocationY = e.Location.Y;
                this.Invalidate();
                this.Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //Reset mouse points to avoid a single line drawing
            mouseDrawLocationX = -drawSize;
            mouseDrawLocationY = -drawSize;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                ResetBoard();
            }
        }


        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(gameMng.isGameActive && !didSendGameOver)
            {
                CountdownTimer.Stop();
                GameOver(GAME_RESULT.Lose);
            }
            
            MainForm.Show();
        }

        /// <summary>
        /// Send a GET request to receive the current gameboard.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetGameBoard()
        {
            var httpClient = HttpClientHelper.GetClient();
            string verifyPath = Constants.SERVER_PATH_DOMAIN + $"api/Game/board/{gameMng.Player.Id}";

            HttpResponseMessage response = await httpClient.GetAsync(verifyPath);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var gameData = JObject.Parse(responseData);

                var board2D = JsonConvert.DeserializeObject<ChessPieceType[,]>(gameData["board"].ToString());         // Deserialize the board as a 2D array
                gameMng.GameBoard = board2D;
                return true;
            }
            else
            {
                MessageBox.Show($"Error getting game board: {response.StatusCode}");
                return false;
            }
        }
        /*
         *             if (!await GetGameBoard())
            {
                // show error message
                // close all forms / this form
                // deactivate the game -> and do not save in DB if its not complete 
                // 
            }
         */

        /// <summary>
        /// Send a GET request to receive the game's current state.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetGameState()
        {
            var httpClient = HttpClientHelper.GetClient();
            string verifyPath = Constants.SERVER_PATH_DOMAIN + $"api/Game/state/{gameMng.Player.Id}";

            HttpResponseMessage response = await httpClient.GetAsync(verifyPath);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(responseData);
                Console.WriteLine("GET GAME STATE RESULT:");
                Console.WriteLine(result);
                gameMng.isGameActive = (bool)result["isActive"];
                gameMng.isCheck = (bool)result["isCheck"];
                gameMng.isPlayerWin = (bool)result["isPlayerWin"];
                return true;
            }
            else
            {
                MessageBox.Show($"Error getting game board: {response.StatusCode}");
                return false;
            }
        }

        /// <summary>
        /// Send a GET request to receive the server's move
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetServerMove()
        {
            var httpClient = HttpClientHelper.GetClient();
            string verifyPath = Constants.SERVER_PATH_DOMAIN + $"api/Game/move/server/{gameMng.Player.Id}";

            HttpResponseMessage response = await httpClient.GetAsync(verifyPath);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(responseData);

                //Server's piece choice and start coordinate
                selectedPiece = (ChessPieceType)(int)result["selectedPieceType"];
                selectedPieceRow = (int)result["selectedPieceStartRow"];
                selectedPieceCol = (int)result["selectedPieceStartCol"];

                //Server's end coordinate
                serverSelectedPieceEndRow = (int)result["selectedPieceEndRow"];
                serverSelectedPieceEndCol = (int)result["selectedPieceEndCol"];

                return true;
            }
            else
            {
                MessageBox.Show($"Error getting game board: {response.StatusCode}");
                return false;
            }
        }

        /// <summary>
        /// Send a PUT request to move the player's piece
        /// </summary>
        /// <param name="playerId">The player's id</param>
        /// <param name="piece">The chosen peice</param>
        /// <param name="fromRow">The start row</param>
        /// <param name="fromCol">The start col</param>
        /// <param name="toRow">The end row</param>
        /// <param name="toCol">The end col</param>
        /// <returns></returns>
        public async Task<bool> PutMovePiece(int playerId, ChessPieceType piece, int fromRow, int fromCol, int toRow, int toCol)
        {
            Console.WriteLine($"PUT CLIENT: SEND ID {playerId}");
            var httpClient = HttpClientHelper.GetClient();
            string verifyPath = Constants.SERVER_PATH_DOMAIN + $"api/Game/move/check/{playerId}";

            var moveData = new
            {
                piece = (int)piece,
                fromRow = fromRow,
                fromCol = fromCol,
                toRow = toRow,
                toCol = toCol
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(moveData),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await httpClient.PutAsync(verifyPath, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(responseData);
                return (bool)result["isLegal"];
            }
            else
            {
                MessageBox.Show($"Error checking move: {response.StatusCode} ");
                return false;
            }
        }

        /// <summary>
        /// Send a PUT request to the server to finish the game
        /// </summary>
        /// <param name="gameResult">The result of the game</param>
        /// <param name="playerId">The player's id</param>
        /// <returns></returns>
        private async Task<bool> PutGameOver(int playerId, Constants.GAME_RESULT gameResult)
        {
            // Send an update request to the active game with the result of the game.

            Console.WriteLine($"PUT CLIENT: SEND ID {playerId}");
            var httpClient = HttpClientHelper.GetClient();
            string verifyPath = Constants.SERVER_PATH_DOMAIN + $"api/Game/finish/{playerId}";

            var moveData = new
            {
                result = gameResult
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(moveData),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            HttpResponseMessage response = await httpClient.PutAsync(verifyPath, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                MessageBox.Show($"Error Finishing Game: {response.StatusCode} ");
                return false;
            }
        }
    }
}