﻿namespace Tabloid.Infrastructure
{
    public class DataSeed
    {
        private readonly TabDbContext _context;

        public DataSeed(TabDbContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            if (!_context.Albums.Any())
            {
                //_context.Albums.Add
            }
        }
    }
}
