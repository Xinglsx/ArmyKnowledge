using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Mskj.ArmyKnowledge.Common.DataObject
{
    [DataContract]
    public class ReturnResult<T>
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        [DataMember(Order = 0)]
        public int code { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember(Order = 1)]
        public T data { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        [DataMember(Order = 2)]
        public string message { get; set; }
    }
}
