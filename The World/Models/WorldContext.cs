using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
	
namespace TheWorld.Models
{
    public class WorldContext : DbContext 
    {
		public WorldContext()
		{ 

		}

		public DbSet<Trip> Trips { get; set; }
		public DbSet<Stop> Stops { get; set; }
    }
}
