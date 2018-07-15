using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SammakEnterprise.MagazineStore.Models
{
    public class Response<T> //where T: Type
    {
        public List<T> Data { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
