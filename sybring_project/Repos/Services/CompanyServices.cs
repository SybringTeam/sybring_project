﻿using Microsoft.EntityFrameworkCore;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Repos.Services
{
    public class CompanyServices : ICompanyServices
    {
        private readonly ApplicationDbContext _db;

        public CompanyServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddCompanyAsync(Company company)
        {
            await _db.AddAsync(company);
            await _db.SaveChangesAsync();

        }

        public async Task<Company> DeleteCompanyAsync(int id)
        {
            var del = await _db.Companies.FindAsync(id);
            _db.Companies.Remove(del);
            await _db.SaveChangesAsync();
            return del;
        }

        public async Task<List<Company>> GetCompanyAsync()
        {
            return await _db.Companies.Include(c => c.Project).ToListAsync();

        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            var company = await _db.Companies
                 .Include(c => c.Project)
                 .FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                throw new InvalidOperationException($"Company with ID {id} not found.");
            }
            return company;
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            

            _db.Update(company);
            await _db.SaveChangesAsync();
            return true;

           
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var project = await _db.Projects.Include(p => p.Companies)
                .FirstOrDefaultAsync(p => p.Id == id);

            return project;
        }



        public async Task<Company> GetCompanyByProjectIdAsync(int projectId)
        {
            var company = await _db.Companies
                                   .Include(c => c.Project)
                                   .FirstOrDefaultAsync(c => c.Project.Id == projectId);

            return company;
        }


    }
}
