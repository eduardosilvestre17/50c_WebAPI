using System.ComponentModel.DataAnnotations;

namespace Sage50c.WebAPI.Models
{
    public class DocumentoVendaDto
    {
        public string? TransSerial { get; set; }
        public string TransDocument { get; set; } = string.Empty;
        public double TransDocNumber { get; set; }
        public double PartyID { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Today;
        public string CurrencyID { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public bool TaxIncluded { get; set; } = true;
        public short TenderID { get; set; } = 0;
        public short PaymentID { get; set; } = 0;
        public double GlobalDiscount { get; set; } = 0;
        public List<DocumentoVendaDetailDto> Details { get; set; } = new();
    }

    public class DocumentoVendaDetailDto
    {
        [Required]
        public string ItemID { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TaxIncludedPrice { get; set; }
        public string UnitOfSaleID { get; set; } = string.Empty;
        public short WarehouseID { get; set; }
        public double TaxPercent { get; set; }
        public short ColorID { get; set; } = 0;
        public short SizeID { get; set; } = 0;
        public string? PropertyValue1 { get; set; }
    }

    public class DocumentoVendaResponseDto
    {
        public string TransSerial { get; set; } = string.Empty;
        public string TransDocument { get; set; } = string.Empty;
        public double TransDocNumber { get; set; }
        public string TransactionID { get; set; } = string.Empty;
        public double PartyID { get; set; }
        public DateTime CreateDate { get; set; }
        public string CurrencyID { get; set; } = string.Empty;
        public string? Comments { get; set; }
        public bool TaxIncluded { get; set; }
        public double TotalAmount { get; set; }
        public double TotalTax { get; set; }
        public List<DocumentoVendaDetailResponseDto> Details { get; set; } = new();
    }

    public class DocumentoVendaDetailResponseDto
    {
        public int LineItemID { get; set; }
        public string ItemID { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TaxIncludedPrice { get; set; }
        public double LineTotal { get; set; }
        public double LineTax { get; set; }
        public string UnitOfSaleID { get; set; } = string.Empty;
        public short WarehouseID { get; set; }
        public double TaxPercent { get; set; }
        public short ColorID { get; set; }
        public short SizeID { get; set; }
        public string? PropertyValue1 { get; set; }
    }

    public class DocumentoVendaListDto
    {
        public string TransSerial { get; set; } = string.Empty;
        public string TransDocument { get; set; } = string.Empty;
        public double TransDocNumber { get; set; }
        public string TransactionID { get; set; } = string.Empty;
        public double PartyID { get; set; }
        public string PartyName { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public double TotalAmount { get; set; }
        public string CurrencyID { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}