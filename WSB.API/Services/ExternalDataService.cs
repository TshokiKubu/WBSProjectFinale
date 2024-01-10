

namespace WSB.API.Services
{
    public class ExternalDataService
    {
        public async Task<string> GetDataById(string id)
        {            
            await Task.Delay(200); 
            return $"Data for ID {id}";
        }
    }
}
