using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using WebApplication1.App_Code;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/values")]
    //[ApiController]
    public class ValuesController : ControllerBase
    {
        IMyWeatherService weatherApi;
        DbContextForApi db;

        public ValuesController(IMyWeatherService _weatherService, DbContextForApi _db)
        {
            weatherApi = _weatherService;
            db = _db;
        }
        // GET api/values
        [HttpGet("get/{text}")]
        public async Task<JsonResult> Get(string text)
        {
            var cities1 = from city in db.Cities.ToList() where city.idCityName.ToLower().StartsWith(text.ToLower())
                          join wd in this.db.WeathersDetails on city.idCityKey equals wd.cityInfo.idCityKey into weatherDetail
                        //  from w in weatherDetail.DefaultIfEmpty(null)
                          select city;

            //var weathers1 = from weather in db.WeathersDetails;

            var cities = from city in cities1.Count() > 0 ? cities1: await this.weatherApi.GetCitiesByText(text)
                         join weath in this.db.WeathersDetails.OrderByDescending(s => s.dateCreated).GroupBy(x => x.cityInfo.idCityKey).Select(x => x.FirstOrDefault()) on city.idCityKey equals weath.cityInfo.idCityKey into ws

                         from w in ws.DefaultIfEmpty() 
                         join fav in this.db.Favorites on city.idCityKey equals fav.cityInfoId into ps
                         from p in ps.DefaultIfEmpty(null)

                         select new Favorite() { cityInfo = city, idFav = (p!=null? p.idFav: Guid.Empty), cityInfoId = city.idCityKey, description = (w!=null?w.description:""), metricValue = (w != null ? w.metricValue: null) };


            return new JsonResult( cities.ToList().Select(res => new Favorite() { description = res.description, metricValue = res.metricValue, cityInfo = res.cityInfo, cityInfoId = res.cityInfo.idCityKey }));
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("getinfobycity/{idCityKey}")]
        public async Task<JsonResult> GetInfoByCity(string idCityKey)
        {
            var res = await weatherApi.GetWeatherInfoByCityKey(idCityKey);
            try { 
            var wd = this.db.WeathersDetails.FirstOrDefault(s => s.cityInfoId == idCityKey);
            if (wd != null && res.dateCreated > wd.dateCreated.AddHours(12))
            {
                db.WeathersDetails.Remove(wd);
                db.WeathersDetails.Add(res);
                db.SaveChanges();
            }
            else
            {
                res.cityInfoId = idCityKey;
                db.WeathersDetails.Add(res);
                db.SaveChanges();
            }
            } catch(Exception ex)
            {
                return new JsonResult(ex);
            }
            //res.Id = -1; 
            return new JsonResult( res);
        }

        // GET api/values/getAllfavcities
        [HttpGet("getAllFavCities")]
        public async Task<JsonResult> GetAllFavCities()
        {
            var cities = from city in this.db.Cities join fav in this.db.Favorites on city.idCityKey equals fav.cityInfoId
            select new Favorite() { cityInfo = city, idFav = fav.idFav, cityInfoId = city.idCityKey } ;

            return new JsonResult( cities.ToList() );
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
