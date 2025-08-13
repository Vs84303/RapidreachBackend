using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace RapidReachNET.Models;

public partial class RapidreachContext : DbContext
{
    public RapidreachContext()
    {
    }

    public RapidreachContext(DbContextOptions<RapidreachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Tracking> Trackings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=rapidreach;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("branches");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .HasColumnName("branch_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("couriers");

            entity.HasIndex(e => e.BranchId, "FKf9kctwrsiav1v2mdhw0wndxhv");

            entity.HasIndex(e => e.UserId, "FKikc54fmhn8gjhr4t5hc176hrv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.DropLocation)
                .HasMaxLength(255)
                .HasColumnName("drop_location");
            entity.Property(e => e.ParcelDescription)
                .HasMaxLength(255)
                .HasColumnName("parcel_description");
            entity.Property(e => e.ParcelWeight).HasColumnName("parcel_weight");
            entity.Property(e => e.PickUpLocation)
                .HasMaxLength(255)
                .HasColumnName("pick_up_location");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Branch).WithMany(p => p.Couriers)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKf9kctwrsiav1v2mdhw0wndxhv");

            entity.HasOne(d => d.User).WithMany(p => p.Couriers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKikc54fmhn8gjhr4t5hc176hrv");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PRIMARY");

            entity.ToTable("feedbacks");

            entity.HasIndex(e => e.UserId, "FK312drfl5lquu37mu4trk8jkwx");

            entity.Property(e => e.FeedbackId).HasColumnName("feedback_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK312drfl5lquu37mu4trk8jkwx");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.CourierId, "FKe4vq9lc0mx0kfiyyyqvr9490q");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.CourierId).HasColumnName("courier_id");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");

            entity.HasOne(d => d.Courier).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKe4vq9lc0mx0kfiyyyqvr9490q");
        });

        modelBuilder.Entity<Tracking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tracking");

            entity.HasIndex(e => e.CourierId, "FK62ktttyhdhyw9b4kmj2vwekwx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourierDate).HasColumnName("courier_date");
            entity.Property(e => e.CourierId).HasColumnName("courier_id");
            entity.Property(e => e.TrackingStatus)
                .HasMaxLength(255)
                .HasColumnName("tracking_status");

            entity.HasOne(d => d.Courier).WithMany(p => p.Trackings)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK62ktttyhdhyw9b4kmj2vwekwx");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.BranchId, "FK9o70sp9ku40077y38fk4wieyk");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(255)
                .HasColumnName("contact");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Pincode)
                .HasMaxLength(255)
                .HasColumnName("pincode");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("user_name");

            entity.HasOne(d => d.Branch).WithMany(p => p.Users)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK9o70sp9ku40077y38fk4wieyk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
