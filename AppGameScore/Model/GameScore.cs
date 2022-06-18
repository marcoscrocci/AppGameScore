using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppScoreExemplo.Model
{
    public class GameScore
    {
        public GameScore()
        {
            this.id = 0;
            this.name = "";
            this.phrase = "";
            this.highscore = 0;
            this.email = "";
            this.game = "";
        }
        public int id { get; set; }
        [Required]
        public int highscore { get; set; }
        [Required]
        public string game { get; set; }
        public string name { get; set; }
        [Required, EmailAddress]
        public string email { get; set; }
        public string phrase { get; set; }
    }
}
