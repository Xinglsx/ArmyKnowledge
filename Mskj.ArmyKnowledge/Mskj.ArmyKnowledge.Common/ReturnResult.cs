using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Mskj.ArmyKnowledge.Common.DataObject
{
    [DataContract]
    public class ReturnResult<T>
    {
        public ReturnResult(int code,T data,string message = "")
        {
            this.code = code;
            this.data = data;
            this.message = message;
        }
        public ReturnResult(int code,string message)
        {
            this.code = code;
            this.message = message;
        }
        public ReturnResult()
        {

        }
        /// <summary>
        /// 返回编码
        /// 1  正常返回
        /// -1 异常错误
        /// -2 逻辑错误
        /// -3 阿里短信接口错误
        /// -4 参数传递错误
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
