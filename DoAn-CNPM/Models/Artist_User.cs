namespace DoAn_CNPM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Artist_User
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Artist { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string User { get; set; }

        public bool? Status { get; set; }

        public virtual Artist Artist1 { get; set; }

        public virtual User User1 { get; set; }
    }
}
