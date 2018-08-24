using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mskj.ArmyKnowledge.ManageWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ProductAudit()
        {
            ViewBag.Message = "产品审核";

            return View();
        }

        public ActionResult ProductManage()
        {
            ViewBag.Message = "产品管理";

            return View();
        }
        public ActionResult QuestionAudit()
        {
            ViewBag.Message = "问题审核";

            return View();
        }
        public ActionResult ProductAuditDetail()
        {
            ViewBag.Message = "产品明细审核";
            return View();
        }
    }
}