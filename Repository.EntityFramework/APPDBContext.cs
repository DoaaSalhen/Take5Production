using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Take5.Models;
using Take5.Models.Models.MasterModels;

namespace Repository.EntityFramework
{
    public class APPDBContext : DbContext
    {
        public APPDBContext(DbContextOptions<APPDBContext> options) : base(options)
        {

        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<DangerCategory> DangerCategories { get; set; }
        public DbSet<Danger> Dangers { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripQuestion> TripQuestions { get; set; }
        public DbSet<TripDanger> TripDangers { get; set; }
        public DbSet<JobSite> JobSites { get; set; }
        public DbSet<MeasureControl> MeasureControls { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<TripJobsite> TripJobsites { get; set; }
        public DbSet<WarningType> WarningTypes { get; set; }
        public DbSet<TripJobsiteWarning> TripJobsiteWarnings { get; set; }

        public DbSet<TripCancellation> TripCancellations { get; set; }

        public DbSet<TripTake5Together> TripTake5Togethers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserNotification>().HasKey(UN => new { UN.NotificationId, UN.userId });
            modelBuilder.Entity<TripJobsite>().HasKey(t => new { t.TripId, t.JobSiteId });
        }
    }
}
