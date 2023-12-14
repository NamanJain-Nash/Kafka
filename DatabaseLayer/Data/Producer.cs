using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Data
{
    public class Producer
    {
        public string id { get; set; }
        public string body { get; set; }
        public List<string> to { get; set;}
        public string subject { get; set; }

    }
}
