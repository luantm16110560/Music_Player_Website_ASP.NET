namespace DoAn_CNPM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RankForWeek")]
    public partial class RankForWeek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Rank { get; set; }

        public int? Song { get; set; }
    }
}
