using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DBEntities
{
    public interface IEntityBase
    {
        Guid Id { get; set; }
        string CreatedBy { get; set; }
        DateTime CTS { get; set; }
        string ModifyBy { get; set; }
        DateTime? MTS { get; set; }
    }
    public abstract class EntityBase : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "UNIQUEIDENTIFIER")]
        public Guid Id { get; set; }
        [Column(TypeName = "VARCHAR(500)")]
        public string CreatedBy { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime CTS { get; set; }
        [Column(TypeName = "VARCHAR(500)")]
        public string ModifyBy { get; set; }
        [Column(TypeName = "DATETIME")]
        public DateTime? MTS { get; set; }
    }
}
