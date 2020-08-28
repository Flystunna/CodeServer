using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeServer.Core.Models
{
    public class project
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int id { get; set; }
        public string external_id { get; set; }
        public int sdlc_systemid { get; set; }
        public virtual sdlc_system sdlc_system { get; set; }
        public string name { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
    }
}
