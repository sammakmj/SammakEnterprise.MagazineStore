using System.Collections.Generic;

namespace SammakEnterprise.MagazineStore.Models
{
    public class Answer
    {
        public string TotalTime { get; set; }
        public List<string> ShouldBe { get; set; }
        public bool AnswerCorrect { get; set; }
    }

    public class AnswerResponse : Response
    {
        public Answer Data { get; set; }

    }

}
