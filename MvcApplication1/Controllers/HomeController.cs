using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                string userList = "";
                HttpPostedFileBase file = Request.Files["file"];
                string FileName;
                string savePath;
                string filename = Path.GetFileName(file.FileName);
                int filesize = file.ContentLength; //获取上传文件的大小单位为字节byte
                string fileEx = System.IO.Path.GetExtension(filename); //获取上传文件的扩展名
                string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename); //获取无扩展名的文件名
                int Maxsize = 4000*1024; //定义上传文件的最大空间大小为4M
                string FileType = ".xls,.xlsx"; //定义上传文件的类型字符串

                FileName = NoFileName + DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                if (!FileType.Contains(fileEx))
                {
                    ViewBag.error = "文件类型不对，只能导入xls和xlsx格式的文件";
                }
                if (filesize >= Maxsize)
                {
                    ViewBag.error = "上传文件超过4M，不能上传";
                }
                string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";
                savePath = Path.Combine(path, FileName);
                file.SaveAs(savePath);

                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + savePath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
                //备注： "HDR=yes;"是说Excel文件的第一行是列名而不是数据，"HDR=No;"正好与前面的相反。
                //      "IMEX=1 "如果列中的数据类型不一致，使用"IMEX=1"可必免数据类型冲突。 
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
                DataSet myDataSet = new DataSet();
                try
                {
                    myCommand.Fill(myDataSet, "ExcelInfo");
                }
                catch (Exception ex)
                {
                    ViewBag.error = ex.Message;
                }
                DataTable table = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();
                foreach (DataRow dr in table.Rows)
                {
                    int userId = 0;
                    if (!String.IsNullOrEmpty(dr[0].ToString()) && int.TryParse(dr[0].ToString(), out userId))
                    {
                        userList += dr[0].ToString() + ",";
                    }
                }
                if (userList.Length<=0)
                {
                    return Json(new { Success = false, Msg = "导入用户列表失败！", Data = "" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new {Success = true, Data = userList.Substring(0, userList.Length - 1)},JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new {Success = false, Msg = "导入用户列表失败！", Data = ""}, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
