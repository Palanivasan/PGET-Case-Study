using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset_Management.Exceptions
{
    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException() : base("Asset not found.") { }

        public AssetNotFoundException(string message) : base(message) { }
    }
}
