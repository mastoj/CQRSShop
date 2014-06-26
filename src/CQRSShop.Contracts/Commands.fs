namespace CQRSShop.Contracts.Commands
open CQRSShop.Infrastructure
open System

type CreateCustomer = {Id: Guid; Name: string } with interface ICommand
type MarkCustomerAsPreferred = {Id: Guid; Discount: int } with interface ICommand