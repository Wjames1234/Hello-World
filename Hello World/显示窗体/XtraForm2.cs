using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Data;
using System.Windows.Forms;

namespace Hello_World.显示窗体
{
    public partial class XtraForm2 : DevExpress.XtraEditors.XtraForm
    {
        // 用于回调主窗体
        public Action<string> OnCellSelected;

        public XtraForm2()
        {
            InitializeComponent();

            // 初始化 gridControl1 内容
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add(1, "选项A");
            dt.Rows.Add(2, "选项B");
            dt.Rows.Add(3, "选项C");
            gridControl1.DataSource = dt;

            // 事件绑定建议放在 Load 或构造函数最后
            //gridView1.RowCellClick += GridView1_RowCellClick;
        }

        //private void GridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        //{
        //    // 只在点击 Value 列时触发
        //    if (e.Column.FieldName == "Value")
        //    {
        //        var value = gridView1.GetRowCellValue(e.RowHandle, "Value")?.ToString();
        //        OnCellSelected?.Invoke(value);
        //        this.Close();
        //    }
        //}

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // 调用回调，传递“123”
            OnCellSelected?.Invoke("123");
            this.Close();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDOWN = 0xA1;
            const int HTCAPTION = 0x2;
            // 禁止拖动标题栏移动窗体
            if (m.Msg == WM_NCLBUTTONDOWN && m.WParam.ToInt32() == HTCAPTION)
                return;
            base.WndProc(ref m);
        }
    }
}