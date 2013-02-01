using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace DoubanBook
{
    public class GetJson
    {
        /// <summary>
        /// 通过关键词搜索图书
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>Json字符串</returns>
        public string GetJsonByTag(string tag)
        {
            //构造URL 默认传入参数
            string url = "https://api.douban.com/v2/book/search?q=" + tag + "&start=0&count=20";
            //Get方式建立HTTP连接
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpResponse = (HttpWebResponse)myRequest.GetResponse();
            //豆瓣接口返回的Stream
            StreamReader reader = new StreamReader(myHttpResponse.GetResponseStream(), Encoding.UTF8);
            //将流读出成string
            string jsonString = reader.ReadToEnd();
            reader.Close();
            myHttpResponse.Close();
            return jsonString;
        }

        /// <summary>
        ///通过API获取图书具体信息 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetJsonByUrl(string url)
        {
            HttpWebRequest myRequest = null;
            HttpWebResponse myHttpResponse = null;
            string getUrl = url.Replace(@"\", "").ToString().Trim();
            myRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            myHttpResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myHttpResponse.GetResponseStream(), Encoding.UTF8);
            string jsonString = reader.ReadToEnd();
            reader.Close();
            myHttpResponse.Close();
            return jsonString;
        }
    }
}
