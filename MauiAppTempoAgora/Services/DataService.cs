namespace MauiAppTempoAgora.Services;

using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

public class DataService
{
    public static async Task<Tempo?> GetPrevisao(string cidade)
    {
        Tempo? t = null;

        string chave = "0685645ea0aa6917fdad21c523a8ae4c";
        string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                     $"q={cidade}&units=metric&appid={chave}";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage resp = await client.GetAsync(url);

            if (resp.IsSuccessStatusCode)
            {
                string json = await resp.Content.ReadAsStringAsync();

                var rascunho = JObject.Parse(json);

                DateTime time = DateTime.UnixEpoch;
                DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                t = new Tempo()
                {
                    lat = (double)rascunho["coord"]["lat"],
                    lon = (double)rascunho["coord"]["lon"],
                    description = (string)rascunho["weather"][0]["description"],
                    main = (string)rascunho["weather"][0]["main"],
                    temp_min = (double)rascunho["main"]["temp_min"],
                    temp_max = (double)rascunho["main"]["temp_max"],
                    speed = (double)rascunho["wind"]["speed"],
                    visibility = (int)rascunho["visibility"],
                    sunrise = sunrise.ToString("HH:mm"),
                    sunset = sunset.ToString("HH:mm")
                }; //Fecha obj do tempo.
            } //Fecha if resp.IsSuccessStatusCode
        }// fecha laço using

        return t;
    }
}


