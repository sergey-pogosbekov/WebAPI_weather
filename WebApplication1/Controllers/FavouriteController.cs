using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using WebApplication1.App_Code;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/favourite")]
    //[ApiController]
    public class FavouriteController : ControllerBase
    {
        IMyWeatherService weatherApi;
        DbContextForApi db;
        public FavouriteController(DbContextForApi _db /*IMyWeatherService _weatherService*/)
        {
            //weatherApi = _weatherService;
            db = _db;

        }
        // GET api/values
        //[HttpGet("get/{text}")]
        [HttpGet("get")]
        public JsonResult Get()
        {
            return new JsonResult(db.Favorites.ToList());
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[HttpGet("getinfobycity/{idCityKey}")]
        //public async Task<JsonResult> GetInfoByCity(string idCityKey)
        //{
        //    return new JsonResult(await weatherApi.GetWeatherInfoByCityKey(idCityKey));
        //}

        // POST api/values
        [HttpGet("post/{id}")]
        public JsonResult Post(string id)
        {
            if (String.IsNullOrWhiteSpace(id)) return new JsonResult("");
            try
            {
                var fav = new Favorite() { idFav = Guid.NewGuid(), cityInfoId = id };
                db.Favorites.Add(fav);
                db.SaveChanges();
                return new JsonResult(fav);
            }
            catch(Exception e)
            {
                return new JsonResult(false);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpGet("{id}")]
        public JsonResult Delete(Guid id)
        {
            if (Guid.Empty == id) return new JsonResult("");
            try
            {
                var fav = db.Favorites.FirstOrDefault(d => d.idFav == id);
                db.Favorites.Remove(fav);
                db.SaveChanges();
                return new JsonResult(fav);
            }
            catch (Exception e)
            {
                return new JsonResult(false);
            }
        }
    }
}
