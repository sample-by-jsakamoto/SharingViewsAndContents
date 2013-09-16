Shared Project Side:
--------
1. Implements "MyVirtualPathProvider" and "MyVirtualFile" in shared project.
2. Change build action of view files(.cshtml), JavaScript files, CSS files and image files to "Embeded resource" in shared project.

Each Web Application Project Side:
--------
1. Regist "MyVirtualPathProvider" into ASP.NET MVC Framework at Application_Start, Global.asax.
2. Change web.config to static files(.js, .css, .png, etc.) should be handled by System.Web.StaticFileHandler, not IIS.

If you do not want to sharing static files, above step 2 is not required.


