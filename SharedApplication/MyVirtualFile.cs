using System;
using System.Linq;
using System.IO;
using System.Web.Hosting;

namespace SharedApplication
{
    public class MyVirtualFile : VirtualFile
    {
        protected string _ResourceName;

        public MyVirtualFile(string virtualPath, string resourceName)
            : base(virtualPath)
        {
            this._ResourceName = resourceName;
        }

        public override Stream Open()
        {
            return this.GetType().Assembly.GetManifestResourceStream(this._ResourceName);
        }
    }
}