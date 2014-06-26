namespace CQRSShop.Contracts.Events
open CQRSShop.Infrastructure
open System

type CustomerCreated = {Id: Guid; Name: string } 
    with interface IEvent with member this.Id with get() = this.Id

type CustomerMarkedAsPreferred = {Id: Guid; Discount: int }
    with interface IEvent with member this.Id with get() = this.Id
