namespace CQRSShop.Contracts.Commands
open CQRSShop.Contracts.Types
open CQRSShop.Infrastructure
open System

// Customer commands
type CreateCustomer = {Id: Guid; Name: string } with interface ICommand
type MarkCustomerAsPreferred = {Id: Guid; Discount: int } with interface ICommand

// Product commands
type CreateProduct = {Id: Guid; Name: string; Price: int } with interface ICommand

// Basket commands
type CreateBasket = { Id: Guid; CustomerId: Guid} with interface ICommand
type AddItemToBasket = { Id: Guid; ProductId: Guid; Quantity: int } with interface ICommand
type ProceedToCheckout = { Id: Guid } with interface ICommand
type CheckoutBasket = { Id: Guid; ShippingAddress: Address } with interface ICommand
type MakePayment = {Id: Guid; Payment: int } with interface ICommand

// Order commands
type StartShippingProcess = { Id: Guid } with interface ICommand
type CancelOrder = { Id: Guid } with interface ICommand
type ShipOrder = { Id: Guid } with interface ICommand
type ApproveOrder = { Id: Guid } with interface ICommand
