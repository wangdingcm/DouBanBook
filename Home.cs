using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


namespace DoubanBook
{
    public partial class Home : Form
    {

        public Home()
        {
            InitializeComponent();
            this.pictureBox1.ImageLocation = System.AppDomain.CurrentDomain.BaseDirectory + "douban.png";
        }

        /// <summary>
        /// 搜索按钮的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string tagString = textBoxSearch.Text.Trim();
            if (tagString != "输入图书关键字进行查询" && tagString != "")
            {
                try
                {
                    //创建一个table储存读到的数据
                    DataTable dt = new DataTable();
                    dt.Columns.Add("序列", typeof(int));
                    dt.Columns.Add("图书名称", typeof(string));
                    dt.Columns.Add("作者",typeof(string));
                    dt.Columns.Add("出版社", typeof(string));
                    dt.Columns.Add("出版年", typeof(string));
                    dt.Columns.Add("页数", typeof(string));
                    dt.Columns.Add("ISBN号", typeof(string));
                    dt.Columns.Add("售价", typeof(string));
                    dt.Columns.Add("bookUrl", typeof(string));
               
                    int order = 0;
                    string name = "";
                    string author = "";
                    string company = "";
                    string year = "";
                    string pageNum = "";
                    string ISBN = "";
                    string price = "";
                    string bookUrl = "";

                    GetJson getJson = new GetJson();
                    //利用Newtonsoft解析Json
                    JObject jobject = JObject.Parse(getJson.GetJsonByTag(tagString));
                    var jsonFile = from file in jobject["books"].Children() select file;
                    foreach (var file in jsonFile)
                    {
                        order++;//序列递增
                        name = file["title"].ToString().Trim();
                        author = file["author"].ToString().Trim() == "[]" ? file["author"].ToString().Trim() : file["author"][0].ToString().Trim();
                        company = file["publisher"].ToString().Trim() ;
                        year = file["pubdate"].ToString().Trim();
                        pageNum = file["pages"].ToString().Trim();
                        ISBN = file["isbn10"].ToString().Trim() ;
                        price = file["price"].ToString().Trim();
                        bookUrl = file["url"].ToString().Trim();

                        //循环向table里面写入数据
                        dt.Rows.Add(order, name, author, company, year, pageNum, ISBN, price,bookUrl);
                    }

                    if (order == 0)
                    {
                        MessageBox.Show("没有查询到相关书籍！");
                    }
                    else
                    {
                        //绑定控件
                        this.dataGridView1.AutoGenerateColumns = false;
                        this.dataGridView1.DataSource = dt;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("查询出错,请检查您的网络连接！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入相应的图书关键字！");
                return;
            }

        }

        /// <summary>
        /// GridView的双击事件，可以打开详细信息Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                string url = "";
                url = dataGridView1[8, e.RowIndex].Value.ToString().Trim(); //此处数字8不稳定，应换成字符串"bookUrl"
                //url = dataGridView1.Rows[e.RowIndex].Cells["bookUrl"].Value.ToString().Trim(); //此处未经调试验证
                if (!string.IsNullOrEmpty(url))
                {
                    Form form = new bookInfo(url);
                    form.Show();
                }
                else
                {
                    MessageBox.Show("该书没有详细信息");
                }
            }
        }

        /// <summary>
        /// 用于显示ToolTip提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                e.ToolTipText="双击显示"+this.dataGridView1.Rows[e.RowIndex].Cells["name"].Value.ToString()+"详细信息";
            }
        }
    }
}
