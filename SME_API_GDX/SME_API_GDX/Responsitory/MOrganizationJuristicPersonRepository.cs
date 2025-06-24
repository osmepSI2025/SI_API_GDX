using System;
using SME_API_GDX.Entities;
using System.Collections.Generic;
using SME_API_GDX.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
public class MOrganizationJuristicPersonRepository
{
    private readonly GDXDBContext _context;

    public MOrganizationJuristicPersonRepository(GDXDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MOrganizationJuristicPerson>> GetAllAsync()
    {
        return await _context.MOrganizationJuristicPeople.ToListAsync();
    }

    public async Task<MOrganizationJuristicPerson?> GetByIdAsync(string id)
    {
        try 
        {
            return await _context.MOrganizationJuristicPeople
       .Include(x => x.TJuristicObjectives)
       .Include(x => x.TJuristicPersonLists)
       .Include(x => x.TOrganizationJuristicAddresses)
       .Include(x => x.TOrganizationJuristicPersonDescriptions)
       .FirstOrDefaultAsync(x => x.OrganizationJuristicId == id);
        } 
        catch(Exception ex) 
        { 
        return new MOrganizationJuristicPerson(); // Return an empty object or handle as needed
        }
       
    }

    public async Task<bool> AddAsync(MOrganizationJuristicPerson entity)
    {
        try
        {
            await _context.MOrganizationJuristicPeople.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            // Optionally log the exception
            return false;
        }
    }

    public async Task<bool> UpdateAsync(MOrganizationJuristicPerson entity)
    {
        _context.MOrganizationJuristicPeople.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await _context.MOrganizationJuristicPeople.FindAsync(id);
        if (entity == null) return false;
        _context.MOrganizationJuristicPeople.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
