using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotnetFinalProjectWinForm
{
    public partial class DisplayOldGames : Form
    {
        public GameRecordsEntities1 db = new GameRecordsEntities1();

        private MainMenuForm MainForm;
        private OldGamesReplayerForm oldGamesReplayerForm;

        public DisplayOldGames(MainMenuForm mainForm)
        {
            InitializeComponent();
            MainForm = mainForm;
        }

        private void DisplayOldGames_Load(object sender, EventArgs e)
        {
            tblBindingSource.DataSource = db.TblGame.ToList();
            tblDataGridView.DataSource = tblBindingSource;
            tblBindingNavigator.BindingSource = tblBindingSource;
        }

        private void DisplayOldGames_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Show();
        }

        private void Replay_Click(object sender, EventArgs e)
        {
            if (tblDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = tblDataGridView.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["GameId"].Value);
                oldGamesReplayerForm = new OldGamesReplayerForm(id);
                oldGamesReplayerForm.Show();
            }
            else
            {
                MessageBox.Show("No row selected.");
            }
        }

        private void Done_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
