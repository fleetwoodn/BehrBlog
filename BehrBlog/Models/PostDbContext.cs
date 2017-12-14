using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BehrBlog.Models;


namespace BehrBlog.Models
{
    public class PostDbContext : DbContext
    {
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Picts> Picts { get; set; }

        public System.Data.Entity.DbSet<BehrBlog.ViewModels.MainDetailViewModel> FinalDetailViewModels { get; set; }
    }
}