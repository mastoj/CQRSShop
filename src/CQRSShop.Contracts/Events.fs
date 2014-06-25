namespace CQRSShop.Contracts.Commands
open CQRSShop.Infrastructure
open System

type CustomerCreated = {Id: Guid; Name: string } 
    with interface IEvent with member this.Id with get() = this.Id
