using Project.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project.Models.DataContext
{
    public class ProjectDBContext:DbContext
    {
        public ProjectDBContext():base("ProjectDB")
        {

        }
        public DbSet<StandartDuruslar> StandartDuruslar{ get; set; }
        public DbSet<UretimOperasyonBildirim> UretimOperasyonBildirim{ get; set; }

    }
}