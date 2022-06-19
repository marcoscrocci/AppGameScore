using AppScoreExemplo.Model;
using AppScoreExemplo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;


namespace AppGameScore
{
    public partial class MainPage : ContentPage
    {
        private GameScore score;
        GameScoreApi api;
        public MainPage()
        {
            InitializeComponent();
            api = new GameScoreApi();
            double paddingTop = 0;
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                paddingTop = 60;
            }
            tituloFrame.Padding = new Thickness(20, paddingTop, 20, 20);
        }

        private async void LocalizarButton_Clicked(object sender, EventArgs e)
        {
            //localiza um registro
            try
            {
                if (entId.Text.Trim() == String.Empty)
                {
                    LimparCampos();
                } else
                {
                    score = await api.GetHighScore(Convert.ToInt32(entId.Text));
                    if (score.id > 0)
                    {
                        entHiScore.Text = score.highscore.ToString();
                        entGame.Text = score.game;
                        entName.Text = score.name;
                        entPhrase.Text = score.phrase;
                        entEmail.Text = score.email;
                        btSalvar.Text = "Atualizar";
                    }
                    else
                    {
                        await DisplayAlert("Informação", "Registro não encontrado", "OK");
                        LimparCampos();
                    }
                }
            }
            catch (Exception error)
            {
                await DisplayAlert("Erro", error.Message, "OK");
            }

        }

        private async void CadastrarButton_Clicked(object sender, EventArgs e)
        {
            //cadastra ou altera um registro
            try
            {
                if (await DadosValidosAsync())
                {
                    score = new GameScore();
                    score.game = entGame.Text;
                    score.name = entName.Text;
                    if (entHiScore.Text.Trim() != String.Empty)
                    {
                        score.highscore = Convert.ToInt32(entHiScore.Text);
                    }
                    score.email = entEmail.Text;
                    score.phrase = entPhrase.Text;
                    if (btSalvar.Text == "Atualizar")
                    {
                        score.id = Convert.ToInt32(entId.Text);
                        await api.UpDateHighScore(score);
                    }
                    else
                    {
                        await api.CreateHighScore(score);
                    }
                    this.LimparCampos();
                    await DisplayAlert("Alerta", "Operação realizada com sucesso", "OK");
                    entId.Focus();
                }
            }
            catch (Exception error)
            {
                await DisplayAlert("Erro", error.Message, "OK");
            }

        }

        private void LimparCampos()
        {
            entId.Text = "";
            entHiScore.Text = "";
            entGame.Text = "";
            entName.Text = "";
            entPhrase.Text = "";
            entEmail.Text = "";
            btSalvar.Text = "Cadastrar";
        }

        private async void ExcluirButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                score = await api.GetHighScore(Convert.ToInt32(entId.Text));
                if (score.id > 0)
                {
                    await api.DeleteHighScore(score.id);
                }
                LimparCampos();
                await DisplayAlert("Alerta", "Operação realizada com sucesso", "OK");
            }
            catch (Exception error)
            {
                await DisplayAlert("Erro", error.Message, "OK");
            }
        }

        private void NovoButton_Clicked(object sender, EventArgs e)
        {
            LimparCampos();
            entHiScore.Focus();
        }

        private async Task<bool> DadosValidosAsync()
        {
            string mensagem = "Informe o(s) campo(s):";
            bool validos = true;
            if (entHiScore.Text.Length < 1)
            {
                mensagem += "\n" + "- Highscore";
                validos = false;
            }
            if (entGame.Text.Length < 1)
            {
                mensagem += "\n" + "- Game";
                validos = false;
            }
            if (entName.Text.Length < 1)
            {
                mensagem += "\n" + "- Nome";
                validos = false;
            }
            if (entEmail.Text.Length < 1)
            {
                mensagem += "\n" + "- E-mail";
                validos = false;
            }
            if (!validos)
            {
                await DisplayAlert("Alerta", mensagem, "OK");
            }
            return validos;
        }

    }
}
