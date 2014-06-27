using System;
using Nest;

namespace CQRSShop.Service.Documents
{
    public class Product
    {
        [ElasticProperty(Index = FieldIndexOption.not_analyzed)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}