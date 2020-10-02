using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DBEntities.Entities
{
    [Table("Searches", Schema = "dbo")]
    public class Searches : EntityBase
    {
        [Column(TypeName = "VARCHAR(50)")]
        public string UserName { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string AlgorithmKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string WordStream { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string Matrix { get; set; }
    }
}
