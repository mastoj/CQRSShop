using System;
using CQRSShop.Contracts.Types;
using Nest;

namespace CQRSShop.Service.Documents
{
    public class Basket
    {
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public Guid Id { get; set; }
        [ElasticProperty(Type = FieldType.nested)]
        public OrderLine[] OrderLines { get; set; }
        public BasketState BasketState { get; set; }
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public Guid OrderId { get; set; }
    }

    public enum BasketState
    {
        Shopping,
        CheckingOut,
        CheckedOut,
        Paid
    }
}