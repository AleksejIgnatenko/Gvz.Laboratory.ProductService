﻿namespace Gvz.Laboratory.ProductService.Models
{
    public class PartyModel
    {
        public Guid Id { get; }
        public int BatchNumber { get; }
        public string DateOfReceipt { get; } = string.Empty;
        public ProductModel Product { get; } = new ProductModel();
        public string SupplierName { get; } = string.Empty;
        public string ManufacturerName { get; } = string.Empty;
        public double BatchSize { get; }
        public double SampleSize { get; }
        public int TTN { get; }
        public string DocumentOnQualityAndSafety { get; } = string.Empty;
        public string TestReport { get; } = string.Empty;
        public string DateOfManufacture { get; } = string.Empty;
        public string ExpirationDate { get; } = string.Empty;
        public string Packaging { get; } = string.Empty;
        public string Marking { get; } = string.Empty;
        public string Result { get; } = string.Empty;
        public string Surname { get; } = string.Empty;
        public string Note { get; } = string.Empty;
        public PartyModel(Guid id, int batchNumber, string dateOfReceipt, ProductModel product, string supplierName,
            string manufacturerName, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking, string result, string surname,
            string note)
        {
            Id = id;
            BatchNumber = batchNumber;
            DateOfReceipt = dateOfReceipt;
            Product = product;
            SupplierName = supplierName;
            ManufacturerName = manufacturerName;
            SampleSize = batchSize;
            SampleSize = sampleSize;
            TTN = ttn;
            DocumentOnQualityAndSafety = documentOnQualityAndSafety;
            TestReport = testReport;
            DateOfManufacture = dateOfManufacture;
            ExpirationDate = expirationDate;
            Packaging = packaging;
            Marking = marking;
            Result = result;
            Surname = surname;
            Note = note;
        }
        public static PartyModel Create(Guid id, int batchNumber, string dateOfReceipt, ProductModel product, string supplierName,
            string manufacturerName, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking, string result, string surname,
            string note)
        {
            return new PartyModel(id, batchNumber, dateOfReceipt, product, supplierName,
            manufacturerName, batchSize, sampleSize, ttn, documentOnQualityAndSafety,
            testReport, dateOfManufacture, expirationDate, packaging, marking, result, surname,
            note);
        }
    }
}
