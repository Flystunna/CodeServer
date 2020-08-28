using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeServer.Core.Models
{
    public class sdlc_system
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string base_url { get; set; }
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
    }
}
