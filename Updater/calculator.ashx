<%@ WebHandler Language="C#" Class="JBosh.CalculatorUpdate" %>
namespace JBosh
{
	using System;
	using System.IO;
	using System.Web;
	using System.Text;
	using System.Net;
	using System.Xml;
	using System.Collections.Generic;
	
	public partial class CalculatorUpdate : IHttpHandler
	{
		private static Version LatestVersion = new Version("3.0.0.1");
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text";
			context.Response.ContentEncoding = System.Text.Encoding.UTF8;

			var bytes = context.Request.BinaryRead(context.Request.TotalBytes);	
			var request = System.Text.Encoding.UTF8.GetString(bytes);
			Version version = null;
			try
			{
				version = new Version(request);
			}
			catch(Exception){}			
			context.Response.Cache.SetExpires(DateTime.Now);
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			if(version != null && version < LatestVersion)
			{
				context.Response.WriteFile("/srv/www/htdocs/updates/calculator.zip");
			}
		}
		public bool IsReusable
		{
			get { return false; }
		}
	}
}
