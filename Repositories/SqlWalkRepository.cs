using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SqlWalkRepository : IwalkRepository
    {    
        private readonly NZWalksDbContext dbContext;
        public SqlWalkRepository(NZWalksDbContext dbContext)
        { 
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;

        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk == null){
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null,string? filterQuery = null,
          string? sortBy = null,bool isAscending = true,int pageNumber  = 1 , int pageSize = 100
        )

        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) 
            {
                if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase)) {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }      
            }

            //Sorting 
            if(string.IsNullOrWhiteSpace(sortBy) == false) {
                if(sortBy.Equals("Name",StringComparison.OrdinalIgnoreCase)) {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase)) {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //pagination 
            var skipResults = (pageNumber - 1) * pageSize;
            
            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();

           // return await dbContext.Walks
           //        .Include("Difficulty")
          //         .Include("Region")
          //         .ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
           return await dbContext.Walks
              .Include("Difficulty")
              .Include("Region")
              .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk == null) {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.RegionId = existingWalk.RegionId;

            await dbContext.SaveChangesAsync();

            return existingWalk;

        }
    }
}