using LorafyAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LorafyAPI
{
    public class AppContext : DbContext
    {
        public DbSet<EndDevice> EndDevices { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<UplinkMessage> UplinkMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EndDevice>()
                .HasKey(e => e.EUI);
            modelBuilder.Entity<EndDevice>()
                .Property(e => e.EUI)
                .IsRequired();
            modelBuilder.Entity<EndDevice>()
                .Property(e => e.Name)
                .IsRequired();
            modelBuilder.Entity<EndDevice>()
                .Property(e => e.Address)
                .IsRequired();
            modelBuilder.Entity<EndDevice>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<EndDevice>()
                 .Property(e => e.DateUpdated)
                 .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Owned<GatewayLocation>();
            modelBuilder.Entity<Gateway>().OwnsOne(e => e.Location);
            modelBuilder.Entity<Gateway>()
                .HasKey(e => e.EUI);
            modelBuilder.Entity<Gateway>()
                .Property(e => e.EUI)
                .IsRequired();
            modelBuilder.Entity<Gateway>()
                .Property(e => e.Name)
                .IsRequired();
            modelBuilder.Entity<Gateway>()
                .Property(e => e.RSSI)
                .IsRequired();
            modelBuilder.Entity<Gateway>()
                .Property(e => e.SNR)
                .IsRequired();
            modelBuilder.Entity<Gateway>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Gateway>()
                 .Property(e => e.DateUpdated)
                 .ValueGeneratedOnAddOrUpdate();

            modelBuilder.Owned<UplinkMessagePayload>();
            modelBuilder.Owned<UplinkMessageDataRate>();
            modelBuilder.Entity<UplinkMessage>()
                .OwnsOne(e => e.Payload);
            modelBuilder.Entity<UplinkMessage>()
                .OwnsOne(e => e.DataRate);
            modelBuilder.Entity<UplinkMessage>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<UplinkMessage>()
                .Property(e => e.EndDeviceEUI)
                .IsRequired();
            modelBuilder.Entity<UplinkMessage>()
                .Property(e => e.GatewayEUI)
                .IsRequired();
            modelBuilder.Entity<UplinkMessage>()
                .Property(e => e.DateReceived)
                .IsRequired();
            modelBuilder.Entity<UplinkMessage>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UplinkMessage>()
                 .Property(e => e.DateUpdated)
                 .ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<UplinkMessage>()
                .HasOne(e => e.EndDevice)
                .WithMany(d => d.UplinkMessages)
                .HasForeignKey(e => e.EndDeviceEUI);
            modelBuilder.Entity<UplinkMessage>()
                .HasOne(e => e.Gateway)
                .WithMany(g => g.UplinkMessages)
                .HasForeignKey(e => e.GatewayEUI);
        }
    }
}
