using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IQuestionService : IBaseService<Question>,IServiceContract
    {
        /// <summary>
        /// 新增问题
        /// </summary>
        ReturnResult<bool> AddQuestion(Question question);
        /// <summary>
        /// 更新问题
        /// </summary>
        /// <param name="cert">用户认证信息</param>
        ReturnResult<bool> UpdateQuestion(Question question);
    }
}
