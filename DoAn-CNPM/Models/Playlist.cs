namespace DoAn_CNPM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Playlist")]
    public partial class Playlist
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Playlist()
        {
            Song_PlayList = new HashSet<Song_PlayList>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? Views { get; set; }

        [StringLength(500)]
        public string Image { get; set; }

        public string Description { get; set; }

        public bool? Status { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song_PlayList> Song_PlayList { get; set; }
    }
}
