using System;
using Nest;

namespace CQRSShop.Service.Documents
{
    public class Customer
    {
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPreferred { get; set; }
        public int Discount { get; set; }
    }
}