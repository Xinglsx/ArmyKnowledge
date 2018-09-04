using Newtonsoft.Json;
using Mskj.LiteFramework;
using Mskj.LiteFramework.Model;
using Mskj.LiteFramework.WebApi;
using Mskj.LiteFramework.WebApi.AuditLog;
using Mskj.LiteFramework.Log4net;
using Mskj.LiteFramework.TinyMapper;
using System;
using System.Web;
using System.Web.Http;

namespace Mskj.ArmyKnowledge.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            GlobalConfiguration.Configuration
                .ConfigAppInstance(Config)
                .UseFilters()
                .UseHttpAttributeRoutes()
                //.UseRoute("DefaultRoute", "Api/{controller}/{action}/{id}", new { id = RouteParameter.Optional })
                .UseJson(camelCaseProperty: true)
                .UseAllCors()
                .StartTasks()
                .Run();
        }
        private void Config(AppInstance appInstance)
        {
            appInstance
                .UseTinyMapper(IsPropertyNameMatch)
                .UseLog4Net()
                .UseAuditLog();
        }

        /// <summary>
        /// 属性名称匹配规则
        /// </summary>
        /// <param name="x">名称1</param>
        /// <param name="y">名称2</param>
        /// <returns>true 表示满足匹配规则，否则不满足</returns>
        private bool IsPropertyNameMatch(string x, string y)
        {
            if (x.Equals(y, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x + "Model" == y)
                return true;
            if (y + "Model" == x)
                return true;
            if (x.Replace("Models", "s") == y)
                return true;
            if (y.Replace("Models", "s") == x)
                return true;

            return false;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = HttpContext.Current.Error;
            var result = Result.Fail(error, -1);
            var str = JsonConvert.SerializeObject(result);
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.StatusCode = 500;
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
    
}