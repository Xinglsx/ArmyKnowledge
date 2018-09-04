using Mskj.LiteFramework;
using Mskj.LiteFramework.Model;
using Mskj.LiteFramework.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Mskj.ArmyKnowledge.Common
{
    public abstract class BaseController : ApiController
    {
        protected BaseController()
        {

        }
        protected Result TryReturn(Func<object> work)
        {
            try
            {
                var rst = work();
                return Result.OK(rst);
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
        protected Result TryDo(Action work)
        {
            try
            {
                work();
                return Result.OK();
            }
            catch (Exception e)
            {
                return Result.Fail(e);
            }
        }
    }
}
