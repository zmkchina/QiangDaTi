using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;

namespace QiangDaTi
{
    public class VMWinMain : INotifyPropertyChanged
    {
        #region 事件
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region 字段
        private int _BtnWidthHeight = 120;
        private int _BtnCount = 20;
        private int _BianHaoFontSize = 60;
        private int _TiMuFontSize = 60;
        private int _DaoJiShiCount = 20;
        private int _DaoJishiShengYu = 0;

        #endregion


        #region 属性
        public DataTable QuestionTable
        {
            get; set;
        }
        public int DaoJishiShengYu
        {
            get { return _DaoJishiShengYu; }
            set { _DaoJishiShengYu = value; OnPropertyChanged("DaoJishiShengYu"); }
        }
        public int DaoJiShiCount
        {
            get { return _DaoJiShiCount; }
            set { _DaoJiShiCount = value; OnPropertyChanged("DaoJiShiCount"); }
        }
        public int BtnWidthHeight
        {
            get { return _BtnWidthHeight; }
            set { _BtnWidthHeight = value; OnPropertyChanged("BtnWidthHeight"); }
        }


        public int BtnCount
        {
            get { return _BtnCount; }
            set { _BtnCount = value; OnPropertyChanged("BtnCount"); }
        }
        public int BianHaoFontSize
        {
            get { return _BianHaoFontSize; }
            set { _BianHaoFontSize = value; OnPropertyChanged("BianHaoFontSize"); }
        }
        public int TiMuFontSize
        {
            get { return _TiMuFontSize; }
            set { _TiMuFontSize = value; OnPropertyChanged("TiMuFontSize"); }
        }
        #endregion

        #region 构造函数
        public VMWinMain()
        {
            string V_QuestionFile = AppDomain.CurrentDomain.BaseDirectory + @"Questions.xlsx";
            QuestionTable = ReadExcelToDataTable(V_QuestionFile, "试题", true);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 将excel文件内容读取到DataTable数据表中
        /// </summary>
        /// <param name="fileName">文件完整路径名</param>
        /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名：true=是，false=否</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable ReadExcelToDataTable(string fileName, string sheetName = null, bool isFirstRowColumn = true)
        {
            //定义要返回的datatable对象
            DataTable data = new DataTable();
            //excel工作表
            ISheet sheet = null;
            //数据开始行(排除标题行)
            int startRow = 0;
            try
            {
                if (!File.Exists(fileName)) { return null; }
                //根据指定路径读取文件
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                //根据文件流创建excel数据结构
                IWorkbook workbook = WorkbookFactory.Create(fs);
                //IWorkbook workbook = new HSSFWorkbook(fs);
                //如果有指定工作表名称
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                    //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    if (sheet == null) { sheet = workbook.GetSheetAt(0); }
                }
                else
                {
                    //如果没有指定的sheetName，则尝试获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    //一行最后一个cell的编号 即总的列数
                    int cellCount = firstRow.LastCellNum;
                    //如果第一行是标题列名
                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }
                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        if (row.Cells.Count > 0)
                        {
                            DataRow dataRow = data.NewRow();
                            for (int j = row.FirstCellNum; j < cellCount; ++j)
                            {
                                if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            data.Rows.Add(dataRow);
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从XML文件中，读取新题目
        /// </summary>
        /// <returns></returns>
        //private int CreateTestTiMu()
        //{
        //    //List<string> _TestTiMu = new List<string>();
        //    //string V_UserIniFile = AppDomain.CurrentDomain.BaseDirectory + @"Questions.xml";
        //    //if (System.IO.File.Exists(V_UserIniFile))
        //    //{
        //    //    XmlDocument V_XmlDoc = new XmlDocument();
        //    //    V_XmlDoc.Load(V_UserIniFile);
        //    //    XmlNodeList nodeList = V_XmlDoc.SelectSingleNode("Question").ChildNodes;
        //    //    foreach (XmlNode xn in nodeList)//遍历所有子节点 
        //    //    {
        //    //        XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
        //    //        if (xe.ChildNodes.Count >= 2)
        //    //        {
        //    //            _TestTiMu.Add(xe.ChildNodes.Item(1).InnerText.Trim());
        //    //        }
        //    //    }
        //    //}
        //    //return _TestTiMu.Count;
        //}

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
