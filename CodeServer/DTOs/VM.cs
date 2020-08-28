using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeServer.DTOs
{
    public class ProjectDTO
    {
        public int id { get; set; }
        public string external_id { get; set; }
        public sdlcSystem sdlcSystem { get; set; }
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class sdlcSystem
    {
        public int id { get; set; }
        public string base_url { get; set; }
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
    }
}
