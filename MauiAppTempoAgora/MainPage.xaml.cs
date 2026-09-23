// Removidos usings específicos do Android para manter compatibilidade com .NET MAUI multi-plataforma
using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System.Linq;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Event handlers de UI normalmente são async void
        private async void Button_Clicked_Previsao(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCidade?.Text))
                {
                    if (lbl_res != null)
                        lbl_res.Text = "Preencha a cidade.";
                    return;
                }

                Tempo? T = await DataService.GetPrevisao(txtCidade.Text);

                if (T == null)
                {
                    if (lbl_res != null)
                        lbl_res.Text = "Cidade não encontrada.";
                    return;
                }

                string dados_previsao =
                    $"Cidade: {T.name}\n" +
                    $"Latitude: {T.lat}  Longitude: {T.lon}\n" +
                    $"Nascer do Sol: {T.sunrise}  Pôr do Sol: {T.sunset}\n" +
                    $"Temp Máx: {T.temp_max}°C  Temp Min: {T.temp_min}°C\n" +
                    $"Clima: {T.description} ({T.main})\n" +
                    $"Vento: {T.speed} m/s  Visibilidade: {T.visibility} m";

                if (lbl_res != null)
                {
                    lbl_res.Text = dados_previsao;

                    string mapa = $"https://embed.windy.com/embed.html?" +
                                  $"type=map&location=coordinates&metricRain=mm&metricTemp=°C" +
                                  $"&metricWind=km/h&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                                  $"&lat={T.lat.ToString().Replace(",", ".")}&lon={T.lon.ToString().Replace(",", ".")}";
                    wv_mapa.Source = mapa;
                }
                else
                {
                    await DisplayAlert(
                        "Erro",
                        "Sem dados de previsão.",
                        "OK");
                    {

                    }
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert(
                    "Sem conexão",
                    "Verifique sua conexão com a internet.",
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Ocorreu um erro: {ex.Message}",
                    "OK");

            }
        }

        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            try
            {
                GeolocationRequest request =
                    new GeolocationRequest(
                         GeolocationAccuracy.Medium,
                         TimeSpan.FromSeconds(10)
                    );

                Location? local = await Geolocation.Default.GetLocationAsync(request);

                if (local != null)
                {
                    string local_disp = $"Latitude: {local.Latitude} \n" +
                         $"Longitude: {local.Longitude}";

                    lbl_coords.Text = local_disp;

                    //pega nome da cidade a partir das coordenadas
                    await GetCidade(local.Latitude, local.Longitude);
                }
                else
                {
                    await DisplayAlert(
                        "Erro",
                        "Não foi possível obter a localização.",
                        "OK");
                }

            }
            catch (FeatureNotSupportedException FnsEx)
            {
                await DisplayAlert(
                    "Erro: Dispositivo não suporta",
                     FnsEx.Message,
                    "OK");
            }
            catch (FeatureNotEnabledException FneEx)
            {
                await DisplayAlert(
                    "Erro: Localização Desativada",
                     FneEx.Message,
                    "OK");
            }
            catch (PermissionException PEx)
            {
                await DisplayAlert(
                    "Erro: Permissão da Localização",
                     PEx.Message,
                    "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                     ex.Message,
                    "OK");
            }
        }
        private async Task GetCidade(double lat, double lon)
        {
            try
            {

                IEnumerable<Placemark>? places = await Geocoding.Default.GetPlacemarksAsync(lat, lon);

                Placemark? place = places?.FirstOrDefault();

                if (place != null)
                {
                    txtCidade.Text = place.Locality ?? txtCidade.Text;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                     ex.Message,
                    "OK");
            }
        }
    }
}
