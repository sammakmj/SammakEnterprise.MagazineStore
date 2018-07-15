using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SammakEnterprise.MagazineStore.Models
{
    public class Magazine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }

    public class MagazinesResponse : Response
    {
        public List<Magazine> Data { get; set; }
    }
}
