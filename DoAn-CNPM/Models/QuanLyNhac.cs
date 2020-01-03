namespace DoAn_CNPM.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QuanLyNhac : DbContext
    {
        public QuanLyNhac()
            : base("name=QuanLyNhac")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Artist_User> Artist_User { get; set; }
        public virtual DbSet<FavoriteSong> FavoriteSongs { get; set; }
        public virtual DbSet<FollowArtist> FollowArtists { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<LikeAlbum> LikeAlbums { get; set; }
        public virtual DbSet<LikeSong> LikeSongs { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<RankForMonth> RankForMonths { get; set; }
        public virtual DbSet<RankForWeek> RankForWeeks { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<Song_PlayList> Song_PlayList { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .HasMany(e => e.Songs)
                .WithOptional(e => e.Album1)
                .HasForeignKey(e => e.Album);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.Artists)
                .WithMany(e => e.Albums)
                .Map(m => m.ToTable("Artist_Album").MapLeftKey("Album").MapRightKey("Artist"));

            modelBuilder.Entity<Artist>()
                .HasMany(e => e.Artist_User)
                .WithRequired(e => e.Artist1)
                .HasForeignKey(e => e.Artist)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Artist>()
                .HasMany(e => e.Songs)
                .WithOptional(e => e.Artist)
                .HasForeignKey(e => e.Composer);

            modelBuilder.Entity<Artist>()
                .HasMany(e => e.Songs1)
                .WithMany(e => e.Artists)
                .Map(m => m.ToTable("Song_Artist").MapLeftKey("Artist").MapRightKey("Song"));

            modelBuilder.Entity<Artist_User>()
                .Property(e => e.User)
                .IsUnicode(false);

            modelBuilder.Entity<FavoriteSong>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<FollowArtist>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Songs)
                .WithMany(e => e.Genres)
                .Map(m => m.ToTable("Song_Genre").MapLeftKey("Genre").MapRightKey("Song"));

            modelBuilder.Entity<LikeAlbum>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<LikeSong>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Playlist>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Playlist>()
                .HasMany(e => e.Song_PlayList)
                .WithRequired(e => e.Playlist1)
                .HasForeignKey(e => e.PlayList)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song>()
                .HasMany(e => e.Song_PlayList)
                .WithRequired(e => e.Song1)
                .HasForeignKey(e => e.Song)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Artist_User)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
