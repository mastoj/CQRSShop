namespace CQRSShop.Contracts.Commands
open CQRSShop.Infrastructure
open System

type CreateCustomer = {Id: Guid; Name: string } with interface ICommand
