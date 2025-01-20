using dotnetFinalProjectWinForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

////// dotnetFinalProjectWinForm.exe
namespace dotnetFinalProjectWinForm
{
    public partial class WelcomeLogIn : Form
    {
        public Player player;
        private int id;
        private string name;
         
        MainMenuForm nextForm;

        public WelcomeLogIn()
        {
            InitializeComponent();
        }

        private void WelcomeLogIn_Load(object sender, EventArgs e)
        {
            var httpClient = HttpClientHelper.GetClient();
            httpClient.BaseAddress = new Uri(Constants.SERVER_PATH_DOMAIN);        // Updated CHECK IF IT WOTRKS    
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (await ValidateInput())
            {
                Console.WriteLine($"local var-> ID:{id} NAME:{name}");
                player = await GetPlayerAsync();
                Console.WriteLine($"PLAYER: ID:{player.Id} NAME: {player.Name}");
                nextForm = new MainMenuForm(this);
                nextForm.Show();
                this.Hide();
            }
        }


        private void SetErrorLabel(string msg, Color color)
        {
            errorLabel.Text = msg;
            errorLabel.ForeColor = color;
        }

        private void ClearTextFields()
        {
            IdInput.Clear();
            NameInput.Clear();
        }

        private async Task<bool> ValidateInput()
        {
            if (!int.TryParse(IdInput.Text, out id))
            {
                SetErrorLabel("Id must be an integer! try again:", Color.Red);
                ClearTextFields();
                return false;
            }
            name = NameInput.Text;
            Console.WriteLine($"Id: {id} Name: {name}");
            if (NameInput.Text == string.Empty || IdInput.Text == string.Empty)
            {
                SetErrorLabel("Name / Id cannot be empty, try again:", Color.Red);
                return false;
            }
            if (!await VerifyPlayerAsync(id, name))
            {
                SetErrorLabel("User doesn't exist, try again or sign up at the website!", Color.Red);
                ClearTextFields();
                return false;
            }
            SetErrorLabel("USER FOUND", Color.Green);
            return true;
        }


        // GET: api/TblPlayers/Verify
        public async Task<bool> VerifyPlayerAsync(int id, string name)
        {
            var httpClient = HttpClientHelper.GetClient();
            // Build the query string
            // Uri.EscapeDataString --> ensures that any special characters in the name parameter (including spaces) are properly encoded, resulting in a URL-safe string
            string verifyData = $"api/TblPlayers/Verify?id={id}&name={Uri.EscapeDataString(name)}";
            string verifyPath = Constants.SERVER_PATH_DOMAIN + verifyData;
            HttpResponseMessage response = await httpClient.GetAsync(verifyPath);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return bool.Parse(result);
            }
            return false;
        }

        public async Task<Player> GetPlayerAsync()
        {
            var httpClient = HttpClientHelper.GetClient();
            string verifyData = $"api/TblPlayers/{id}";
            string verifyPath = Constants.SERVER_PATH_DOMAIN + verifyData;

            player = null;
            HttpResponseMessage response = await httpClient.GetAsync(verifyPath);
            if (response.IsSuccessStatusCode)
            {
                player = await response.Content.ReadAsAsync<Player>();
            }
            return player;
        }
    }
}
