using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SteamDomain.Database.SqlServer.Entities;

namespace SteamDomain.Database.SqlServer.Context;

public partial class SteamContext : DbContext
{
    public SteamContext(DbContextOptions<SteamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Dlc> Dlcs { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Library> Libraries { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<ReviewAnswer> ReviewAnswers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAchievement> UserAchievements { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.AchievementId).HasName("PK__Achievem__276330E0BB30C49E");

            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Game).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Achieveme__GameI__6383C8BA");
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.DeveloperId).HasName("PK__Develope__DE084CD1DBD88584");

            entity.ToTable("Developer");

            entity.Property(e => e.DeveloperId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("DeveloperID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DeveloperName).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<Dlc>(entity =>
        {
            entity.HasKey(e => e.Dlcid).HasName("PK__DLCs__8EAFB339D97ED532");

            entity.ToTable("DLCs");

            entity.Property(e => e.Dlcid).HasColumnName("DLCID");
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Dlcname)
                .HasMaxLength(200)
                .HasColumnName("DLCName");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Game).WithMany(p => p.Dlcs)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__DLCs__GameID__01142BA1");
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Body).HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EmailTemplateId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.FriendId }).HasName("PK__Friends__3DA43AFACF454E72");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FriendId).HasColumnName("FriendID");
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.FriendNavigation).WithMany(p => p.FriendFriendNavigations)
                .HasForeignKey(d => d.FriendId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friends__FriendI__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.FriendUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friends__UserID__534D60F1");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Games__2AB897DD8B5295F1");

            entity.Property(e => e.GameId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("GameID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");
            entity.Property(e => e.ImageUrl).HasMaxLength(600);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Developer).WithMany(p => p.Games)
                .HasForeignKey(d => d.DeveloperId)
                .HasConstraintName("FK__Games__Developer__5FB337D6");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Games)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Games__Publisher__60A75C0F");
        });

        modelBuilder.Entity<GameGenre>(entity =>
        {
            entity.HasKey(e => e.GameGenreId).HasName("PK__Game_Gen__B56670D54ABDF23B");

            entity.ToTable("Game_Genre");

            entity.Property(e => e.GameGenreId).HasColumnName("GameGenreID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameGenres)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Game_Genr__GameI__05D8E0BE");

            entity.HasOne(d => d.Genre).WithMany(p => p.GameGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Game_Genr__Genre__06CD04F7");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PK__Genre__0385055E211FD6E8");

            entity.ToTable("Genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.LibraryId).HasName("PK__Library__A13647BF13AEE81A");

            entity.ToTable("Library");

            entity.Property(e => e.LibraryId).HasColumnName("LibraryID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Libraries)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Library__GameID__74AE54BC");

            entity.HasOne(d => d.User).WithMany(p => p.Libraries)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Library__UserID__73BA3083");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.OfferId).HasName("PK__Offers__8EBCF0B14DD08C19");

            entity.Property(e => e.OfferId).HasColumnName("OfferID");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Game).WithMany(p => p.Offers)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Offers__GameID__09A971A2");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasIndex(e => e.Code, "UQ_Permissions_Code").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Specificity)
                .HasMaxLength(20)
                .HasDefaultValue("ByAssignment");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__4C657E4B04A94273");

            entity.Property(e => e.PublisherId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PublisherID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PublisherName).HasMaxLength(150);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AEA9B91FC5");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Reviews__GameID__6C190EBB");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Reviews__UserID__6B24EA82");
        });

        modelBuilder.Entity<ReviewAnswer>(entity =>
        {
            entity.HasKey(e => new { e.ReviewId, e.UserId }).HasName("PK__ReviewAn__A5C4F5049B01FACA");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ReviewAnswersId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Review).WithMany(p => p.ReviewAnswers)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewAns__Revie__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.ReviewAnswers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReviewAns__UserI__70DDC3D8");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => new { e.RoleId, e.PermissionId });

            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_RolePermissions_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RolePermissions_Role");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__Sessions__C9F492705F62BC91");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Sessions__GameID__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Sessions__UserID__7C4F7684");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__Status__C8EE2043249E563F");

            entity.ToTable("Status");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ShowName).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACAA425068");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105348B65F6EA").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UserName).HasMaxLength(100);

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Users__StatusID__4F7CD00D");
        });

        modelBuilder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(e => e.UserAchievementId).HasName("PK__UserAchi__07E627D6F6BC8789");

            entity.Property(e => e.UserAchievementId).HasColumnName("UserAchievementID");
            entity.Property(e => e.AchievementId).HasColumnName("AchievementID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Achievement).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.AchievementId)
                .HasConstraintName("FK__UserAchie__Achie__6754599E");

            entity.HasOne(d => d.User).WithMany(p => p.UserAchievements)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserAchie__UserI__66603565");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.UserRoleAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_UserRoles_AssignedBy");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoles_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_User");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishlistId).HasName("PK__Wishlist__233189CB011EE259");

            entity.ToTable("Wishlist");

            entity.Property(e => e.WishlistId).HasColumnName("WishlistID");
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Game).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Wishlist__GameID__797309D9");

            entity.HasOne(d => d.User).WithMany(p => p.Wishlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Wishlist__UserID__787EE5A0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
