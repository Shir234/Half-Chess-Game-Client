using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetFinalProjectWinForm.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Country { get; set; }

        // public ICollection<TblGames>? Games { get; set; }                THERE IS NO TBL GAMES HERE SO HOW DO WE NAVIGATE? 


        public bool IsLoggedIn { get; set; }
        public int GameTimer { get; set; }


        public Player()
        {
        }

    }
}

