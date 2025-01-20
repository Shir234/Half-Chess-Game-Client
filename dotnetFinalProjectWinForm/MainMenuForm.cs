using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dotnetFinalProjectWinForm.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnetFinalProjectWinForm
{
    public partial class MainMenuForm : Form
    {
        WelcomeLogIn WelcomeForm;
        GameForm gameForm = null;
        DisplayOldGames oldGames;

        public Games gameMng = new Games();

        public MainMenuForm(WelcomeLogIn welcomForm)
        {
            InitializeComponent();
            WelcomeForm = welcomForm;
            gameMng.Player = WelcomeForm.player;
        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            TimerListCombo.Items.AddRange(new object[] { 20, 30, 40, 50, 60 });
            TimerListCombo.SelectedIndex = 0;
            playerDetails.Text = $"Player ID: {gameMng.Player.Id} \nName: {gameMng.Player.Name.Trim()} \nCountry: {gameMng.Player.Country.Trim()} \nPhone: {gameMng.Player.Phone}";

            gameMng.Player.Id = WelcomeForm.player.Id;
            Console.WriteLine($"player id form 2: {gameMng.Player.Id}");
        }

        public void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WelcomeForm.Close();
            this.Dispose();
            HttpClientHelper.GetClient().Dispose();
            Application.Exit();
            Environment.Exit(0); // Ensure all threads and processes are terminated
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            gameMng.Timer = Convert.ToInt32(TimerListCombo.SelectedItem);
            await PostStartGame(gameMng.Player.Id);
            Console.WriteLine("AFTER START GAME:");
            Console.WriteLine($"Player id: {gameMng.Player.Id} active: {gameMng.isGameActive}");

            this.Hide();
            // create the game form -> initiate 
            gameForm = new GameForm(this);
            gameForm.Show();
        }

        private void MainMenuForm_Shown(object sender, EventArgs e)
        {
            this.Show();
            if (gameForm != null)
                gameForm.Close();
            if (oldGames != null)
                oldGames.Close();
            // post server - game not active / kill -> reset, before reset = save the game to DB
        }

        public async Task PostStartGame(int id)
        {
            Console.WriteLine($"ID SENT TO SERVER IN POST START GAME: {id}");
            var httpClient = HttpClientHelper.GetClient();
            string verifyPath = Constants.SERVER_PATH_DOMAIN + "api/Game/start";

            // Create content with playerId
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("playerId", id.ToString())
            });

            HttpResponseMessage response = await httpClient.PostAsync(verifyPath, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var gameData = JObject.Parse(responseData);

                var board2D = JsonConvert.DeserializeObject<Games.ChessPieceType[,]>(gameData["board"].ToString());         // Deserialize the board as a 2D array
                gameMng.GameBoard = board2D;
                gameMng.isPlayerTurn = (bool)gameData["isPlayerTurn"];
                gameMng.isGameActive = true;
            }
            else
            {
                MessageBox.Show($"Error starting game: {response.StatusCode}");
            }

        }

        private void AllGames_Click(object sender, EventArgs e)
        {
            this.Hide();
            // create the game form -> initiate 
            oldGames = new DisplayOldGames(this);
            oldGames.Show();
        }


    }
}
