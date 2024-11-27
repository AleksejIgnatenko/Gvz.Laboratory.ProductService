using Gvz.Laboratory.ProductService.Abstractions;
using Gvz.Laboratory.ProductService.Dto;
using Gvz.Laboratory.ProductService.Entities;
using Gvz.Laboratory.ProductService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.ProductService.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly GvzLaboratoryProductServiceDbContext _context;
        private readonly IProductRepository _productRepository;
        public PartyRepository(GvzLaboratoryProductServiceDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }
        public async Task<Guid> CreatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id);
            if (existingParty == null)
            {
                var productEntity = await _productRepository.GetProductEntityByIdAsync(party.ProductId)
                    ?? throw new InvalidOperationException($"Product with Id '{party.ProductId}' was not found.");

                var partyEntity = new PartyEntity
                {
                    Id = party.Id,
                    BatchNumber = party.BatchNumber,
                    DateOfReceipt = party.DateOfReceipt,
                    Product = productEntity,
                    SupplierName = party.SupplierName,
                    ManufacturerName = party.ManufacturerName,
                    BatchSize = party.BatchSize,
                    SampleSize = party.SampleSize,
                    TTN = party.TTN,
                    DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety,
                    TestReport = party.TestReport,
                    DateOfManufacture = party.DateOfManufacture,
                    ExpirationDate = party.ExpirationDate,
                    Packaging = party.Packaging,
                    Marking = party.Marking,
                    Result = party.Result,
                    Note = party.Note,
                    Surname = party.Surname,
                };

                await _context.Parties.AddAsync(partyEntity);
                await _context.SaveChangesAsync();
            }
            return party.Id;
        }
        public async Task<(List<PartyModel> parties, int numberParties)> GetProductPartiesForPageAsync(Guid productId, int pageNumber)
        {
            var partyEntities = await _context.Parties
                .AsNoTracking()
                .Where(p => p.Product.Id == productId)
                .Include(p => p.Product)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            if (!partyEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Where(p => p.Product.Id == productId)
                    .Include(p => p.Product)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberParties = await _context.Parties
                .Where(p => p.Product.Id == productId)
                .CountAsync();

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                ProductModel.Create(p.Product.Id, p.Product.ProductName, p.Product.UnitsOfMeasurement, false).product,
                p.SupplierName,
                p.ManufacturerName,
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                p.Surname,
                p.Note
                )).ToList();
            return (parties, numberParties);
        }
        public async Task<Guid> UpdatePartyAsync(PartyDto party)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id)
                ?? throw new InvalidOperationException($"Party with Id '{party.Id}' was not found.");

            var productEntity = await _productRepository.GetProductEntityByIdAsync(party.ProductId)
                ?? throw new InvalidOperationException($"Product with Id '{party.ProductId}' was not found.");

            existingParty.BatchNumber = party.BatchNumber;
            existingParty.DateOfReceipt = party.DateOfReceipt;
            existingParty.Product = productEntity;
            existingParty.SupplierName = party.SupplierName;
            existingParty.ManufacturerName = party.ManufacturerName;
            existingParty.BatchSize = party.BatchSize;
            existingParty.SampleSize = party.SampleSize;
            existingParty.TTN = party.TTN;
            existingParty.DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety;
            existingParty.TestReport = party.TestReport;
            existingParty.DateOfManufacture = party.DateOfManufacture;
            existingParty.ExpirationDate = party.ExpirationDate;
            existingParty.Packaging = party.Packaging;
            existingParty.Marking = party.Marking;
            existingParty.Result = party.Result;
            existingParty.Note = party.Note;

            await _context.SaveChangesAsync();

            return party.Id;
        }
        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
