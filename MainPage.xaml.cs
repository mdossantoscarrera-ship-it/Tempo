using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;
using System;
using System.Net.Http;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // ✅ MÉTODO QUE ESTAVA FALTANDO (CORRIGE O ERRO)
        private async void Button_Clicked_Localizacao(object sender, EventArgs e)
        {
            lbl_coords.Text = "Localização não implementada ainda";
        }

        private async void Button_Clicked_Previsao(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txt_cidade.Text))
                {
                    string cidade = txt_cidade.Text.Trim();

                    Tempo? t = await DataService.GetPrevisao(cidade);

                    if (t != null)
                    {
                        lbl_res.Text =
                            $"Cidade: {cidade}\n" +
                            $"Latitude: {t.lat}\n" +
                            $"Longitude: {t.lon}\n" +
                            $"Clima: {t.description}\n" +
                            $"Vento: {t.speed} m/s\n" +
                            $"Visibilidade: {t.visibility} metros\n" +
                            $"Nascer do Sol: {t.sunrise}\n" +
                            $"Pôr do Sol: {t.sunset}\n" +
                            $"Temp Máx: {t.temp_max}°C\n" +
                            $"Temp Min: {t.temp_min}°C";

                        string lat = t.lat.ToString().Replace(",", ".");
                        string lon = t.lon.ToString().Replace(",", ".");

                        string mapa = $"https://embed.windy.com/embed.html?" +
                                      $"type=map&location=coordinates" +
                                      $"&metricRain=mm&metricTemp=°C&metricWind=km/h" +
                                      $"&zoom=5&overlay=wind&product=ecmwf&level=surface" +
                                      $"&lat={lat}&lon={lon}";

                        wv_mapa.Source = new UrlWebViewSource
                        {
                            Url = mapa
                        };
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Cidade não encontrada!", "OK");
                        lbl_res.Text = "Sem dados de previsão.";
                    }
                }
                else
                {
                    await DisplayAlert("Atenção", "Digite o nome da cidade.", "OK");
                }
            }
            catch (HttpRequestException)
            {
                await DisplayAlert("Conexão", "Sem conexão com a internet!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}