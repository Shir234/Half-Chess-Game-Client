using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace dotnetFinalProjectWinForm.Models
{
    public class GameRecorder
    {
        public GameRecordsEntities1 db = new GameRecordsEntities1();

        private GameRecord currentGame;
        private List<Move> moves = new List<Move>();
        private int moveNumber = 0;

        public void StartNewGame()
        {
            currentGame = new GameRecord
            {
                GameDate = DateTime.Now,
                Moves = new List<Move>()
            };
        }

        public void RecordMove(Games.ChessPieceType pieceType, int fromRow, int fromCol, int toRow, int toCol)
        {
            var move = new Move
            {
                PieceType = pieceType,
                FromRow = fromRow,
                FromCol = fromCol,
                ToRow = toRow,
                ToCol = toCol,
                MoveNumber = moveNumber++
            };
            moves.Add(move);
        }

        public void SaveGameToDB(Constants.GAME_RESULT result)
        {
            currentGame.Result = result;
            currentGame.Moves = moves;

            var gameToAdd = new TblGame
            {
                GameDate = DateTime.Now,
                Result = (int)result
            };

            //db.TblGame.Add(new TblGame
            //{
            //    GameId = currentGame.GameId,
            //    GameDate = currentGame.GameDate,
            //    Result = (int)result
            //});
            db.TblGame.Add(gameToAdd);
            db.SaveChanges();

            foreach (var move in moves)
            {
                db.TblMoves.Add(new TblMoves
                {
                    GameId = gameToAdd.GameId,  // Link to the parent game
                    FromRow = move.FromRow,
                    FromCol = move.FromCol,
                    ToRow = move.ToRow,
                    ToCol = move.ToCol,
                    PieceType = (int)move.PieceType,
                    MoveNumber = move.MoveNumber
                });
            }

            db.SaveChanges();  // Save all moves


        }
    }
}
