using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Likes;
using OFP_CORE.Entities.Log;
using OFP_CORE.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OFP_DAL.Context
{
    public class OneFixedProblemContext : IdentityDbContext<BaseUser, BaseUserRole, string>
    {
        public OneFixedProblemContext(DbContextOptions<OneFixedProblemContext> options)
        : base(options)
        {
        }

        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProblemLike> ProblemLikes { get; set; }
        public DbSet<AnswerLike> AnswerLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Suggest> Suggests { get; set; }
        public DbSet<ReportAnswer> ReportAnswers { get; set; }
        public DbSet<ReportComment> ReportComments { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Mail> Mails { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=YASINTANRIVERDI\\SQLEXPRESS;Initial Catalog=OneFixedProblem;Integrated Security=True;Connect Timeout=30;Encrypt=True;TrustServerCertificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;").UseLazyLoadingProxies();
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<Problem>().HasKey(p => p.Id);
            builder.Entity<Answer>().HasKey(p => p.Id);
            builder.Entity<Comment>().HasKey(p => p.Id);
            builder.Entity<Suggest>().HasKey(p => p.Id);
            builder.Entity<ProblemLike>().HasKey(p => p.Id);
            builder.Entity<AnswerLike>().HasKey(p => p.Id);
            builder.Entity<CommentLike>().HasKey(p => p.Id);
            builder.Entity<ReportAnswer>().HasKey(p => p.Id);
            builder.Entity<ReportComment>().HasKey(p => p.Id);
            builder.Entity<Log>().HasKey(p => p.Id);
            builder.Entity<Mail>().HasKey(p => p.Id);

            // BASEUSER
            builder.Entity<BaseUser>()
                .HasMany(x => x.Answers)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // PROBLEM
            builder.Entity<Problem>()
                .HasMany(x => x.Answers)
                .WithOne(x => x.Problem)
                .HasForeignKey(x => x.ProblemId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Problem>()
                .HasMany(x => x.ProblemLikes)
                .WithOne(x => x.Problem)
                .HasForeignKey(x => x.ProblemId)
                .OnDelete(DeleteBehavior.NoAction);

            // ANSWER
            builder.Entity<Answer>()
                .HasMany(x => x.Comments)
                .WithOne(x => x.Answer)
                .HasForeignKey(x => x.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Answer>()
                .HasMany(x => x.AnswerLikes)
                .WithOne(x => x.Answer)
                .HasForeignKey(x => x.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);


            // COMMENT
            builder.Entity<Comment>()
                .HasMany(x => x.CommentLikes)
                .WithOne(x => x.Comment)
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            // REPORTCOMMENT
            builder.Entity<Comment>()
                .HasMany(x => x.ReportComments)
                .WithOne(x => x.Comment)
                .HasForeignKey(x => x.ReportedCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            // REPORTANSWER
            builder.Entity<Answer>()
                .HasMany(x => x.ReportAnswers)
                .WithOne(x => x.Answer)
                .HasForeignKey(x => x.ReportedAnswerId)
                .OnDelete(DeleteBehavior.NoAction);



            var adminUser = new BaseUser
            {
                Id = "1",
                FirstName = "Super",
                LastName = "Admin",
                Region = "Turkey",
                Gender = "Male",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                UserName = "AdminYasin",
                NormalizedUserName = "ADMINYASIN",
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Status = Status.Active,
                CreatedDate = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<BaseUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "adminsuperAdmin123");

            builder.Entity<BaseUser>().HasData(adminUser);

            var adminRole = new BaseUserRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var userRole = new BaseUserRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER"
            };


            builder.Entity<BaseUserRole>().HasData(adminRole, userRole);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });


            base.OnModelCreating(builder);
        }
    }
}
