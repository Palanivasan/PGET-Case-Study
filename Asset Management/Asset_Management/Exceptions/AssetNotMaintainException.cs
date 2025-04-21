using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Exceptions
{
    public class AssetNotMaintainException : Exception
    {
        public AssetNotMaintainException() : base("Asset cannot be maintained.") { }

        public AssetNotMaintainException(string message) : base(message) { }
    }
}
