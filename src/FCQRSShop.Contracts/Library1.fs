namespace FCQRSShop.Contracts
open System

module Dsl = 
    type CommandData = 
        | CreateCustomer of Name: string
        | MarkCustomerAsPreferred of Discount: int
        | CreateProduct of Name: string * Price: int
//        | MarkCustomerAsPreferred of CustomerId: Guid * Discount: int

    type EventData = 
        | CustomerCreated of Name: string
        | CustomerMarkedAsPreferred of Discount: int
        | ProductCreated of Name: string * Price: int
//        | CustomerMadePreferred of CustomerId: Guid * Discount: int

    type Command = {Id: Guid; Data: CommandData}
    type Event = {Id: Guid; Data: EventData}

module Infrastructure = 

    let rec load transition state events =
        match events with
        | [] -> state
        | e::evts -> load transition (transition state e) evts         
        

module Domain =
    type Customer = {Id: Guid}
    let defaultCustomer = {Id = Guid.Empty}

    open Dsl
    let customerTransitions state = function
        | CustomerCreated(name) -> Some(state)
        | CustomerMarkedAsPreferred(discount) -> Some(state)
        | _ -> None
    
    let customerHandlers state = function
        | CreateCustomer(name) -> Some([CustomerCreated(name)])
        | MarkCustomerAsPreferred(discount) -> Some([CustomerMarkedAsPreferred(discount)])
        | _ -> None


    type Product = {Id: Guid}
    let defaultProduct = {Id = Guid.Empty}

    let productTransitions state = function
        | ProductCreated(name, price) -> Some(state)
        | _ -> None

    let productHandlers state = function
        | CreateProduct(name, price) -> Some([ProductCreated(name, price)])
        | _ -> None

    let inline getHolyTriage c : (a, a -> EventData -> Option a, a -> CommandData -> b) =
        match c with
        | CreateCustomer(_) -> (defaultCustomer, customerTransitions, customerHandlers)
        | MarkCustomerAsPreferred(_) -> (defaultCustomer, customerTransitions, customerHandlers)
        | CreateProduct(_,_) -> (defaultProduct, productTransitions, productHandlers)


//module Stuff = 
//    open Events
//    type CustomerState = {Id: Guid; Name: string; Discount: int}
//    let defaultCustomer = { Id = Guid.Empty; Name = ""; Discount = 0 }
//    let customerTransition event state = 
//        match event with
//        | CustomerCreated(id, name) -> {defaultCustomer with Name = name; Id = id}
//        | CustomerMadePreferred(id, discount) -> {state with Discount = discount}
//
//    let rec loadFromEvents transitionFunction state events = 
//        match events with
//        | head::tail -> loadFromEvents transitionFunction (transitionFunction head state) tail
//        | [] -> state
//
//    let customerLoad = loadFromEvents customerTransition defaultCustomer
        
        // Customer commands
//type CreateCustomer = {Id: Guid; Name: string } with interface ICommand
//type MarkCustomerAsPreferred = {Id: Guid; Discount: int } with interface ICommand
