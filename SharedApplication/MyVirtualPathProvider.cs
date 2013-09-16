using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace SharedApplication
{
    public class MyVirtualPathProvider : VirtualPathProvider
    {
        public static Dictionary<string, string> GetEmbededViewCatalog()
        {
            var type = typeof(MyVirtualPathProvider);
            var topNameSpace = type.Namespace + ".";
            var query = from resname in type.Assembly.GetManifestResourceNames() // = "SharedApplication.Views.Home.SharedView1.cshtml"
                        where resname.StartsWith(topNameSpace)
                        let name = Path.GetFileNameWithoutExtension(resname)     // = "SharedApplication.Views.Home.SharedView1"
                                        .Substring(topNameSpace.Length)          // = "Views.Home.SharedView1"
                                        .Replace('.', '/')                       // = "Views/Home/SharedView1"
                        let ext = Path.GetExtension(resname)                     // = ".cshtml"
                        select new
                        {
                            vpath = "~/" + name + ext, // key   = "~/Views/Home/SharedView1.cshtml"
                            resname                    // value = "SharedApplication.Views.Home.SharedView1.cshtml"
                        };
            return query.ToDictionary(item => item.vpath, item => item.resname);
        }

        public override bool FileExists(string virtualPath)
        {
            var exists = base.FileExists(virtualPath);
            if (exists) return true;

            var appRelativeVirtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            var viewCatalog = GetEmbededViewCatalog();
            exists = viewCatalog.ContainsKey(appRelativeVirtualPath);
            return exists;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var exists = base.FileExists(virtualPath);
            if (exists) return base.GetFile(virtualPath);

            var appRelativeVirtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            var viewCatalog = GetEmbededViewCatalog();
            exists = viewCatalog.ContainsKey(appRelativeVirtualPath);
            if (exists) return new MyVirtualFile(virtualPath, viewCatalog[appRelativeVirtualPath]);

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            var exists = base.FileExists(virtualPath);
            if (exists)
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            return null;
        }
    }
}