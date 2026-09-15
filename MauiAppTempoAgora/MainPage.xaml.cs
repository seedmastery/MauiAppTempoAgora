using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // Event handlers de UI normalmente são async void
        private async void btnBuscar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var lbl = this.FindByName<Label>("lbl_res");

                if (!string.IsNullOrEmpty(txtCidade.Text))
                {
                    Tempo? T = await DataService.GetPrevisao(txtCidade.Text);

                    if (T != null)
                    {
                        string dados_previsao = "";
                        dados_previsao += $"Latitude: {T.lat} " +
                            $"Longitude: {T.lon} " +
                            $"Nascer do Sol: {T.sunrise} " +
                            $"Por do Sol: {T.sunset} " +
                            $"Temp Máx: {T.temp_max} " +
                            $"Temp Min: {T.temp_min} ";

                        if (lbl != null)
                        {
                            lbl.Text = dados_previsao;
                        }
                        else
                        {
                            await DisplayAlert("Erro", "Controle 'lbl_res' não encontrado no XAML.", "OK");
                        }
                    }
                    else
                    {
                        if (lbl != null)
                        {
                            lbl.Text = "Sem dados de previsão.";
                        }
                        else
                        {
                            await DisplayAlert("Erro", "Controle 'lbl_res' não encontrado no XAML.", "OK");
                        }
                    }
                }
                else
                {
                    if (lbl != null)
                    {
                        lbl.Text = "Preencha a cidade.";
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Controle 'lbl_res' não encontrado no XAML.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
