using SammakEnterprise.MagazineStore.Models;
using SammakEnterprise.MagazineStore.Services;
using SammakEnterprise.MagazineStore.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace SammakEnterprise.MagazineStore
{
    internal class MagazineStoreMain
    {
        private readonly DataService _dataService = new DataService();

        public void Run()
        {
            //GetToken();
            //GetCategories();
            //GetSubscribers();
            //GetMagazinesForCategory("Animals");
            //GetAllMagazines();
            DoAllAndPostAnswer();
        }

        private TokenResponse GetTokenResponse()
        {
            return _dataService.GetTokenAsync().GetAwaiter().GetResult();
        }

        private void GetToken()
        {
            var timeMeasure = TimeMeasure.BeginTiming("GetToken");

            var tokenResponse = GetTokenResponse();

            JsonDisplay.ConsoleDisplayJsonResponse("GetToken", tokenResponse);
            timeMeasure.EndTimingAndConsoleDisplay();
        }

        private CategoriesResponse GetCategoriesResponse(string token)
        {
            return _dataService.GetCategoriesAsync(token).GetAwaiter().GetResult();
        }

        private void GetCategories()
        {
            var tokenResponse = GetTokenResponse();
            var timeMeasure = TimeMeasure.BeginTiming("GetCategories");

            var categoriesResponse = GetCategoriesResponse(tokenResponse.Token);

            JsonDisplay.ConsoleDisplayJsonResponse("GetCategories", categoriesResponse);
            timeMeasure.EndTimingAndConsoleDisplay();
        }

        private SubscribersResponse GetSubscribers(string token)
        {
            return _dataService.GetSubscriberResponseAsync(token).GetAwaiter().GetResult();
        }

        private void GetSubscribers()
        {
            var tokenResponse = GetTokenResponse();
            var timeMeasure = TimeMeasure.BeginTiming("GetSubscribers");

            var subscribersResponse = GetSubscribers(tokenResponse.Token);

            JsonDisplay.ConsoleDisplayJsonResponse("GetSubscribers", subscribersResponse);
            timeMeasure.EndTimingAndConsoleDisplay();
        }

        private MagazinesResponse GetMagazinesForCategory(string token, string category)
        {
            return _dataService.GetMagazineResponseAsync(token, category).GetAwaiter().GetResult();
        }

        private void GetMagazinesForCategory(string category = "News")
        {
            var tokenResponse = GetTokenResponse();
            var timeMeasure = TimeMeasure.BeginTiming($"GetMagazinesForCategory {category}");

            var magazinesResponse = GetMagazinesForCategory(tokenResponse.Token, category);

            JsonDisplay.ConsoleDisplayJsonResponse($"GetMagazines for category {category}", magazinesResponse);
            timeMeasure.EndTimingAndConsoleDisplay();
        }

        private List<MagazinesResponse> GetAllMagazinesResponses(CategoriesResponse categoriesResponse)
        {
            // build a list of categories to make one http call for each in parallel mode
            var categoryList = new List<string>();
            foreach (var category in categoriesResponse.Data)
            {
                categoryList.Add(category);
            }
            var magazineListResponse = _dataService.GetMagazineResponseAsync(categoriesResponse.Token, categoryList)
                .GetAwaiter()
                .GetResult()
                .ToList();

            return magazineListResponse;
        }

        private void GetAllMagazines()
        {
            var tokenResponse = GetTokenResponse();
            var categoriesResponse = GetCategoriesResponse(tokenResponse.Token);
            var timing = TimeMeasure.BeginTiming("GetAllMagazinesResponses");

            var magazineListResponse = GetAllMagazinesResponses(categoriesResponse);

            JsonDisplay.ConsoleDisplayJsonResponse("GetAllMagazines WhenAll", magazineListResponse);
            timing.EndTimingAndConsoleDisplay();
        }

        private void DoAllAndPostAnswer()
        {
            var timeMeasure = TimeMeasure.BeginTiming("DoAllAndPostAnswer");

            var tokenResponse = GetTokenResponse();
            JsonDisplay.ConsoleDisplayJsonResponse("Token", tokenResponse);
            var categoriesResponse = GetCategoriesResponse(tokenResponse.Token);
            JsonDisplay.ConsoleDisplayJsonResponse("Categories", categoriesResponse);
            var subscribersResponse = GetSubscribers(tokenResponse.Token);
            JsonDisplay.ConsoleDisplayJsonResponse("Subscribers", subscribersResponse);
            var magazineListResponse = GetAllMagazinesResponses(categoriesResponse);
            JsonDisplay.ConsoleDisplayJsonResponse("List of all Magazines", magazineListResponse);

            // fill in the magazines detais for each subscriber
            foreach (var subscriber in subscribersResponse.Data)
            {
                foreach (var magazinesResponse in magazineListResponse)
                {
                    subscriber.Magazines.AddRange(
                        subscriber.MagazineIds
                            .SelectMany(magazineId => magazinesResponse.Data, (magazineId, magazine) => new { magazineId, magazine })
                            .Where(anonim => anonim.magazine.Id == anonim.magazineId)
                            .Select(anon => new Magazine
                            {
                                Id = anon.magazineId,
                                Category = anon.magazine.Category,
                                Name = anon.magazine.Name
                            })
                    );
                }
            }
            JsonDisplay.ConsoleDisplayJsonData("Subscribers with filled Magazines", subscribersResponse);

            // prepare the post payload with all subscribers who have at least one subscription in each category
            var answerPost = new AnswerPost
            {
                Subscribers = subscribersResponse.Data
                    .Where(subscriber => subscriber.Magazines
                                             .Select(c => new { c.Category })
                                             .GroupBy(x => x.Category)
                                             .Select(x => x.First()).Count() == categoriesResponse.Data.Count
                    )
                    .Select(subscriber => subscriber.Id).ToList()
            };
            JsonDisplay.ConsoleDisplayJsonData("answerPost", answerPost);

            var answerResponse = _dataService.PostAnswerAsync(tokenResponse.Token, answerPost).GetAwaiter().GetResult();

            JsonDisplay.ConsoleDisplayJsonResponse("Answer Response after post", answerResponse);

            // display the overal time
            timeMeasure.EndTimingAndConsoleDisplay();
        }

    }

}
