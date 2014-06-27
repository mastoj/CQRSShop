namespace CQRSShop.Contracts.Types
open System

type Address = { Street: string }
type OrderLine = {ProductId: Guid; ProductName: string; OriginalPrice: int; DiscountedPrice: int; Quantity: int}
    with override this.ToString() = sprintf "ProdcutName: %s, Price: %d, Discounted: %d, Quantity: %d" this.ProductName this.OriginalPrice this.DiscountedPrice this.Quantity
