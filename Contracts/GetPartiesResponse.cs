namespace Gvz.Laboratory.ProductService.Contracts
{
    public record GetPartiesResponse(
            Guid Id,
            int BatchNumber,
            string DateOfReceipt,
            string ProductName,
            string SupplierName,
            string ManufacturerName,
            double BatchSize,
            double SampleSize,
            int TTN,
            string DocumentOnQualityAndSafety,
            string TestReport,
            string DateOfManufacture,
            string ExpirationDate,
            string Packaging,
            string Marking,
            string Result,
            string Responsible, //Surname
            string Note
            );
}
