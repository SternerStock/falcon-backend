namespace Falcon.API.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Falcon.API.Helpers;
    using Falcon.API.Models;

    public class MinecraftPositionsController : ApiController
    {
        // GET: api/MinecraftPositions
        [ActionName("GetPositions")]
        public IEnumerable<MinecraftPlayerModel> Get()
        {
            var playerLocations = new List<MinecraftPlayerModel>();
            var nbtFiles = MinecraftCache.GetPlayerData();
            foreach (var nbt in nbtFiles)
            {
                var playerData = new MinecraftPlayerModel(nbt.Item1, nbt.Item2);
                playerLocations.Add(playerData);
            }

            return playerLocations;
        }
    }
}
