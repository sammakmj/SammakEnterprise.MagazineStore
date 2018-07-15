using System.Collections.Generic;

namespace SammakEnterprise.MagazineStore.Models
{
    public class Subscriber
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<int> MagazineIds { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// This Magazines property is not coming back from http call, instead it will be populated by this application after the call.
        /// This additonal property does not bother the http call either, it will be null when http comes back.
        /// </remarks>
        public List<Magazine> Magazines { get; set; } = new List<Magazine>();
    }
    public class SubscribersResponse : Response
    {
        public List<Subscriber> Data { get; set; }
    }
}
