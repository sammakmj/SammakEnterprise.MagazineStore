using SammakEnterprise.MagazineStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SammakEnterprise.MagazineStore.Services
{
    public class DataService : BaseDataService
    {
        public async Task<TokenResponse> GetTokenAsync()
        {
            var path = "token";
            return await GenericGetAsync<TokenResponse>(path);
        }

        public async Task<CategoriesResponse> GetCategoriesAsync(string token)
        {
            var path = $"categories/{token}";
            return await GenericGetAsync<CategoriesResponse>(path);
        }

        public async Task<SubscribersResponse> GetSubscriberResponseAsync(string token)
        {
            var path = $"subscribers/{token}";
            return await GenericGetAsync<SubscribersResponse>(path);
        }

        public async Task<MagazinesResponse> GetMagazineResponseAsync(string token, string category)
        {
            var path = $"magazines/{token}/{category}";
            return await GenericGetAsync<MagazinesResponse>(path);
        }

        public async Task<MagazinesResponse[]> GetMagazineResponseAsync(string token, List<string> categories)
        {
            var magazineResponseTasks = new List<Task<MagazinesResponse>>();
            foreach(var category in categories)
            {
                var path = $"magazines/{token}/{category}";
                var vehicleTask = GenericGetAsync<MagazinesResponse>(path);
                magazineResponseTasks.Add(vehicleTask);
            }
            return await Task.WhenAll(magazineResponseTasks);
        }

        public async Task<AnswerResponse> PostAnswerAsync(string token, AnswerPost answerPost)
        {
            var path = $"answer/{token}";
            return await GenericPostAsync<AnswerPost, AnswerResponse>(path, answerPost);
        }

    }
}
