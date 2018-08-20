using Mskj.ArmyKnowledge.All.Common.DataObj;
using Newtonsoft.Json;

namespace Mskj.ArmyKnowledge.All.Common.PostData
{
    /// <summary>
    /// 问题上传类
    /// </summary>
    public class PostQuestion
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 作者ID
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }
        /// <summary>
        /// 作者昵称
        /// </summary>
        [JsonProperty("authornickname")]
        public string AuthorNickname { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
        /// <summary>
        /// 图片集合
        /// </summary>
        [JsonProperty("images")]
        public string Images { get; set; }
        /// <summary>
        /// 首页图片
        /// </summary>
        [JsonProperty("homeimage")]
        public string HomeImage { get; set; }
    }

    public static class PostQuestionExtensions
    {
        public static QuestionModel ToModel(this PostQuestion question)
        {
            return new QuestionModel
            {
                Title = question.Title,
                Author = question.Author,
                AuthorNickname = question.AuthorNickname,
                Introduction = question.Introduction,
                Content = question.Content,
                Images = question.Images,
                HomeImage = question.HomeImage,
            };
        }
    }
}
