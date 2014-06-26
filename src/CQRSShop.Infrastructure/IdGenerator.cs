using System;

namespace CQRSShop.Infrastructure
{
    public class IdGenerator
    {
        private static Func<Guid> _generator;

        public static Func<Guid> GenerateGuid
        {
            get
            {
                _generator = _generator ?? Guid.NewGuid;
                return _generator;
            }
            set { _generator = value; }
        }
    }
}