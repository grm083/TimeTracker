using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/datetimepicker/jquery-1.12.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/bundles/css/kendo").Include("~/Content/kendo/2015.3.930/kendo.common-bootstrap.min.css",
                                                                      "~/Content/kendo/2015.3.930/kendo.bootstrap.min.css",
                                                                      "~/Content/kendo/2015.3.930/kendo.dataviz.min.css",
                                                                      "~/Content/kendo/2015.3.930/kendo.dataviz.bootstrap.min.css"));

            bundles.Add(new StyleImagePathBundle("~/bundles/css/kendocustomized").Include("~/Content/themes/sbs/kendo.customized.2013.01.11.css"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.validate.min.js",
                      "~/Scripts/jquery.validate.unobtrusive.min.js",
                      "~/Scripts/datetimepicker/jqueryui.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kendo").Include("~/Scripts/kendo/2015.3.930/kendo.all.min.js",
                                                                      "~/Scripts/kendo/2015.3.930/kendo.timezones.min.js",
                                                                      "~/Scripts/kendo/2015.3.930/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                       "~/Content/vendor/metisMenu/metisMenu.min.css",
                       "~/Content/sb-admin-2.min.css",
                       "~/Content/Validation.css",
                      "~/Scripts/datetimepicker/jqueryui.css",
                       "~/fonts/font-awesome/css/font-awesome.min.css"));
        }
    }

    public class StyleImagePathBundle : Bundle
    {
        public StyleImagePathBundle(string virtualPath) : base(virtualPath)
        {
            base.Transforms.Add(new StyleRelativePathTransform());
            base.Transforms.Add(new CssMinify());
        }
        public StyleImagePathBundle(string virtualPath, string cdnPath) : base(virtualPath, cdnPath)
        {
            base.Transforms.Add(new StyleRelativePathTransform());
            base.Transforms.Add(new CssMinify());
        }
    }

    public class StyleRelativePathTransform : IBundleTransform
    {
        public StyleRelativePathTransform() { }
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = String.Empty;
            Regex pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
            foreach (BundleFile cssFileInfo in response.Files)
            {
                var _cssFileInfo = new FileInfo(context.HttpContext.Server.MapPath(cssFileInfo.VirtualFile.VirtualPath));
                if (_cssFileInfo.Exists)
                {
                    string contents = File.ReadAllText(_cssFileInfo.FullName);
                    MatchCollection matches = pattern.Matches(contents);
                    if (matches.Count > 0)
                    {
                        string cssFilePath = _cssFileInfo.DirectoryName;
                        string cssVirtualPath = cssFilePath.Replace(context.HttpContext.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty);
                        foreach (Match match in matches)
                        {
                            string relativeToCSS = match.Groups[2].Value;
                            string absoluteToUrl = Path.GetFullPath(Path.Combine(cssFilePath, relativeToCSS));
                            string serverRelativeUrl = "/" + absoluteToUrl.Replace(context.HttpContext.Request.ServerVariables["APPL_PHYSICAL_PATH"], String.Empty).Replace(@"\", "/");
                            string quote = match.Groups[1].Value;
                            string replace = String.Format("url({0}{1}{0})", quote, HttpContext.Current.Request.ApplicationPath + serverRelativeUrl);
                            contents = contents.Replace(match.Groups[0].Value, replace);
                        }
                    }
                    response.Content = String.Format("{0}\r\n{1}", response.Content, contents);
                }
            }
        }
    }
}
