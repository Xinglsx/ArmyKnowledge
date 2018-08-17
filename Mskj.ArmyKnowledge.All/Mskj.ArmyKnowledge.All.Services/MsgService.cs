using Mskj.ArmyKnowledge.All.ServiceContracts;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using System;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class MsgService : BaseService<Msg, Msg>, IMsgService
    {
        private readonly IRepository<Msg> _MsgRepository;
        private readonly IRepository<MsgDetail> _MsgDetailRepository;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public MsgService(IRepository<Msg> msgRepository,
            IRepository<MsgDetail> msgDetailRepository):
            base(msgRepository)
        {
            this._MsgRepository = msgRepository;
            this._MsgDetailRepository = msgDetailRepository;
        }

        public ReturnResult<bool> AddMsg(Msg msg)
        {
            msg.id = Guid.NewGuid().ToString();
            _MsgDetailRepository.Add(new MsgDetail { id = Guid.NewGuid().ToString(),msgid=msg.id,sendtime = DateTime.Now });
            return new ReturnResult<bool>(1,_MsgRepository.Add(msg));
        }
    }
}
