using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.App_Code
{

    //public static class FeedIteratorExtensions
    //{
        //public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this FeedIterator<T> iterator)
        //{
        //    while (iterator.HasMoreResults)
        //    {
        //        foreach (var item in await iterator.ReadNextAsync())
        //        {
        //            yield return item;
        //        }
        //    }
        //}
    //}

    public class ApiCity {
        public string LocalizedName { get; set; }
        public string Key { get; set; }
    }

    public class ApiMetric
    {
        public double? Value { get; set; }
    }
    public class ApiTemperature
    {
        public ApiMetric Metric { get; set; }
    }
    public class ApiWeatherInfo
    {
        public ApiTemperature Temperature { get; set; }
        public string WeatherText{ get; set; }
        public int Id { get; set; }
    }
    
    public class CityInfo
    {
        [Key]
        public string idCityKey { get; set; }
        public string idCityName { get; set; }
    }
    public class WeatherInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public CityInfo cityInfo { get; set; }
        public string cityInfoId { get; set; }
        public double? metricValue { get; set; } //Temperature.Metric.Value
        public string description { get; set; }
        public DateTime dateCreated { get; set; }
    }
    
    public class MyWeatherService : IMyWeatherService
    {
        private readonly DbContextForApi _context;
        private readonly IHttpClientFactory _http;
        private string apiKey = "qA4xAIC7eymfpdpO6t6ECvWLqT34jgAC";

        public MyWeatherService(DbContextForApi context, IHttpClientFactory clientFactory)
        {
            _context = context;
            _http = clientFactory;
        }

        public async Task<WeatherInfo> GetWeatherInfoByCityKey(string idKey)
        {
            if (_context.Cities.Any(city => idKey == city.idCityKey))
            {
                var current = _context.WeathersDetails.Include(d=>d.cityInfo).OrderByDescending(s => s.dateCreated).Where(s => s.cityInfo.idCityKey == idKey && s.dateCreated < DateTime.Now && s.dateCreated > DateTime.Today).FirstOrDefault();

                if (current != null)
                {
                    return (current);
                }
                else
                {
                    //get api key and cityKey (from db)
                    var cityObj = _context.Cities.Where(city => city.idCityKey == idKey).ToList().FirstOrDefault();
                    if (cityObj == null)
                        return null;



                    var request = new HttpRequestMessage(HttpMethod.Get,
         "http://dataservice.accuweather.com/currentconditions/v1/" + cityObj.idCityKey + "?apikey=" + this.apiKey /*  "urlTOGetWeatherInfoByCity" */ );
                    //request.Headers.Add("Accept", "application/vnd.github.v3+json");
                    //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                    var client = _http.CreateClient();

                    var response = await client.SendAsync(request);

                    WeatherInfo weatherVal = null;
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            weatherVal = (await JsonSerializer.DeserializeAsync<IEnumerable<ApiWeatherInfo>>(responseStream)).ToList().Select(s => new WeatherInfo() { 
                                cityInfo = new CityInfo() {
                                idCityKey = cityObj.idCityKey,
                                idCityName = cityObj.idCityName
                                }, cityInfoId = cityObj.idCityKey, description = s.WeatherText, metricValue = s.Temperature.Metric.Value }).FirstOrDefault();

                            //ToList().

                            if (weatherVal != null)
                            {
                                weatherVal.dateCreated = DateTime.Now;
                                weatherVal.cityInfoId = idKey;
                                

                                current = weatherVal;
                            }

                            return (current);
                        }
                    }
                    else
                    {
                        return null;
                    }

                }

            }

            else
            {
                return null;
            }

            //throw new NotImplementedException();
        }

        //string[] getCitiesByText(string text)
        //{
        //    return new string[] { "Kyiv"};
        //}

        public async Task<IEnumerable<CityInfo>> GetCitiesByText(string text)
        {
            if (_context.Cities.Any(city => city.idCityName.ToLower().StartsWith(text.ToLower())))
            {
                var searchedCities = _context.Cities.OrderBy(s => s.idCityName).Where(s => s.idCityName.ToLower().StartsWith(text.ToLower())).ToList();

                if (searchedCities != null)
                {
                    return searchedCities.Select(d => new CityInfo() { idCityKey = d.idCityKey, idCityName = d.idCityName }).ToList();
                }
                else
                {

                    return null;


                }

            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Get,
        "http://dataservice.accuweather.com/locations/v1/cities/autocomplete" + "?apikey=" + this.apiKey + "&q=" + text /*  "urlTOGetWeatherInfoByCity" */ );
                //request.Headers.Add("Accept", "application/vnd.github.v3+json");
                //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _http.CreateClient();

                var response = await client.SendAsync(request);

                CityInfo[] current = null;

                CityInfo[] citiesVal;

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        citiesVal = (await JsonSerializer.DeserializeAsync<IEnumerable<ApiCity>>(responseStream)).ToList().Select(s =>
                        {
                            //dynamic r = JsonSerializer.Deserialize<dynamic>(s.GetRawText());
                            return new CityInfo()
                            {
                                idCityName = s.LocalizedName,
                                idCityKey = s.Key
                            };
                        }).ToArray();
                    }
                    if (citiesVal != null)
                    {
                        this._context.Cities.AddRange(citiesVal);
                        this._context.SaveChanges();
                        current = citiesVal;
                    }

                    return (current);
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
