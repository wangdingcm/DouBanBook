using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;

namespace DoubanBook
{
    public partial class bookInfo : Form
    {
        //定义全局bookUrl
        private string _bookUrl = "";
        
        public bookInfo(string url)
        {
            InitializeComponent();

            _bookUrl = url;//赋值
            Bind();
        }

        protected void Bind()
        {
            try
            {
                GetJson getJson = new GetJson();
                JObject file = JObject.Parse(getJson.GetJsonByUrl(_bookUrl));
                
                this.labelTitle.Text = "《" + file["title"].ToString().Trim() + "》";
                this.labelAuthor.Text = file["author"].ToString().Trim() == "[]" ? file["author"].ToString().Trim() : file["author"][0].ToString().Trim();
                this.labelCompany.Text = file["publisher"].ToString().Trim();
                this.labelYear.Text = file["pubdate"].ToString().Trim();
                this.labelPageNum.Text = file["pages"].ToString().Trim();
                this.labelISBN.Text = file["isbn10"].ToString().Trim();
                this.labelPrice.Text = file["price"].ToString().Trim();
                this.pictureBoxBook.ImageLocation = file["image"].ToString().Trim();
                this.textBoxIntro.Text = file["summary"].ToString().Trim();
            }
            catch (Exception)
            {
            }
        }
    }
}
