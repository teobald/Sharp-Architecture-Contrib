using System.Web.Mvc;

namespace SharpArchContrib.Web.Mvc 
{
    public class RazorAreaViewEngine : RazorViewEngine
    {
        public RazorAreaViewEngine()
        {
            AreaViewLocationFormats = new[] { 
                "~/Views/{2}/{1}/{0}.cshtml", 
                "~/Views/{2}/Shared/{0}.cshtml",
                "~/Views/{2}/{1}/{0}.vbhtml",
                "~/Views/{2}/Shared/{0}.vbhtml",
            };
            AreaMasterLocationFormats = AreaViewLocationFormats;
            AreaPartialViewLocationFormats = AreaViewLocationFormats;
        }
    }
}