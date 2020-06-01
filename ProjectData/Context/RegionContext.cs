﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectData.Models;

namespace ProjectData.Context
{
    public class RegionContext : DbContext
    {
        private IConfiguration Configuration;
        public DbSet<Region> region { get; set; }

        public RegionContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Configuration = new ConfigurationBuilder().
                AddJsonFile("appsettings.json", false).
                Build();

            string server = Configuration.GetSection("ConnectToDB").GetSection("server").Value;
            string userDB = Configuration.GetSection("ConnectToDB").GetSection("UserId").Value;
            string password = Configuration.GetSection("ConnectToDB").GetSection("password").Value;
            string database = Configuration.GetSection("ConnectToDB").GetSection("database").Value;

            string connectStr = "Server=" + server + ";UserId=" + userDB + ";password="
                + password + ";database=" + database + ";";

            optionsBuilder.UseMySql(connectStr);
        }
    }
}
