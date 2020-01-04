namespace DoAn_CNPM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Song")]
    public partial class Song
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Song()
        {
            Song_PlayList = new HashSet<Song_PlayList>();
            Artists = new HashSet<Artist>();
            Genres = new HashSet<Genre>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Lyrics { get; set; }

        public int? Views { get; set; }

        public int? Likes { get; set; }

        [StringLength(500)]
        public string FileName { get; set; }

        public bool? Status { get; set; }

        public int? Album { get; set; }

        public int? Composer { get; set; }

        public int? ViewsPerWeek { get; set; }

        public int? ViewsPerMonth { get; set; }

        public virtual Album Album1 { get; set; }

        public virtual Artist Artist { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song_PlayList> Song_PlayList { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Artist> Artists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
