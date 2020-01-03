namespace DoAn_CNPM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Artist_User = new HashSet<Artist_User>();
            Playlists = new HashSet<Playlist>();
        }

        [Key]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(500)]
        public string Password { get; set; }

        public bool? Role { get; set; }

        public bool? Status { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Artist_User> Artist_User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}
