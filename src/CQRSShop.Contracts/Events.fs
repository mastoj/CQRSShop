namespace CQRSShop.Contracts.Events
open CQRSShop.Contracts.Types
open CQRSShop.Infrastructure
open System

// Customer events
type CustomerCreated = {Id: Guid; Name: string } 
    with interface IEvent with member this.Id with get() = this.Id

type CustomerMarkedAsPreferred = {Id: Guid; Discount: int }
    with interface IEvent with member this.Id with get() = this.Id

// Product events
type ProductCreated = {Id: Guid; Name: string; Price: int }
    with interface IEvent with member this.Id with get() = this.Id

// Basket events
type BasketCreated = { Id: Guid; CustomerId: Guid; Discount: int}
    with interface IEvent with member this.Id with get() = this.Id

type ItemAdded = { Id: Guid; ProductId: Guid; ProductName: string; OriginalPrice: int; DiscountedPrice: int; Quantity: int}
    with 
    override this.ToString() = sprintf "Item added. Id: %O, Price: %d, Discounted: %d, Quantity: %d" this.Id this.OriginalPrice this.DiscountedPrice this.Quantity
    interface IEvent with member this.Id with get() = this.Id

type CustomerIsCheckingOutBasket = { Id: Guid }
    with interface IEvent with member this.Id with get() = this.Id

type BasketCheckedOut = { Id: Guid; ShippingAddress: Address } 
    with interface IEvent with member this.Id with get() = this.Id

// Order events
type OrderCreated ={ Id: Guid; BasketId: Guid }
    with interface IEvent with member this.Id with get() = this.Id

type ShippingProcessStarted = {Id: Guid}
    with interface IEvent with member this.Id with get() = this.Id

type OrderCancelled = {Id: Guid}
    with interface IEvent with member this.Id with get() = this.Id

type OrderShipped = {Id: Guid}
    with interface IEvent with member this.Id with get() = this.Id

