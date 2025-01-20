using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static dotnetFinalProjectWinForm.Models.Games;

namespace dotnetFinalProjectWinForm.Models
{
    internal class GameReplayer
    {
        private GameRecord game;
        private int currentMoveIndex = 0;
        private Timer moveTimer;
        private OldGamesReplayerForm oldGamesReplayerForm;


        public GameReplayer(GameRecord game, OldGamesReplayerForm oldGamesReplayerForm)
        {
            this.game = game;
            this.oldGamesReplayerForm = oldGamesReplayerForm;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            moveTimer = new Timer();
            moveTimer.Interval = 3000; // One second between moves
            moveTimer.Tick += PlayNextMove_Tick;
        }

        private void PlayNextMove_Tick(object sender, EventArgs e)
        {
            if (currentMoveIndex >= game.Moves.Count)
            {
                moveTimer.Stop();
                return;
            }

            var move = game.Moves[currentMoveIndex++];
            ExecuteMove(move);
        }

        private void ExecuteMove(Move move)
        {
            if (oldGamesReplayerForm.InvokeRequired)
            {
                oldGamesReplayerForm.Invoke(new Action(() => ExecuteMove(move)));
                return;
            }

            oldGamesReplayerForm.MovePiece(
                move.PieceType,
                move.FromRow,
                move.FromCol,
                move.ToRow,
                move.ToCol
            );
        }

        public void StartReplay()
        {
            Console.WriteLine();
            currentMoveIndex = 0;
            moveTimer.Start();
        }

        public void PauseReplay()
        {
            moveTimer.Stop();
        }

        public void ResumeReplay()
        {
            moveTimer.Start();
        }

        public void SetReplaySpeed(int millisecondInterval)
        {
            moveTimer.Interval = millisecondInterval;
        }

    }



}
