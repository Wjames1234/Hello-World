using DevExpress.XtraEditors;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors.Repository; // 添加此 using 指令
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Hello_World
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public bool loaded = false;
        public XtraForm1()
        {
            InitializeComponent();

       
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            //XtraMessageBox.Show("Hello, World!", "Greeting", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 假设 treeList1 已经在 Designer 中添加到窗体
            // 创建数据源
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ParentID", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            // 添加组（父节点）
            dt.Rows.Add(1, DBNull.Value, "组A");
            dt.Rows.Add(2, DBNull.Value, "组B");

            // 添加子项
            dt.Rows.Add(3, 1, "组A-子项1");
            dt.Rows.Add(4, 1, "组A-子项2");
            dt.Rows.Add(5, 2, "组B-子项1");

            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            treeList1.DataSource = dt;
            treeList1.ExpandAll();
            loaded = true; // 标记已加载
        }

        // 假设你已经有一个 DevExpress.XtraTreeList.TreeList 控件名为 treeList1
        // 你可以在 TreeList 的 CustomNodeCellEdit 事件中动态显示 TablePanel 和 PanelControl

        private void treeList1_CustomNodeCellEdit(object sender, DevExpress.XtraTreeList.GetCustomNodeCellEditEventArgs e)
        {
            // 在 treeList1_CustomNodeCellEdit 方法内，记录当前节点和列，并传递给回调
            if (loaded == false) return;
            if (e.Column.FieldName == "Name")
            {
                var repositoryItem = new RepositoryItemButtonEdit();
                repositoryItem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                repositoryItem.Buttons.Clear();
                repositoryItem.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(
                    DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)
                {
                    Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis,
                    Visible = true,
                    IsLeft = false,
                    Width = 8
                });
                repositoryItem.Buttons.Add(new DevExpress.XtraEditors.Controls.EditorButton(
                    DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
                {
                    Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Delete,
                    Visible = true,
                    IsLeft = false,
                    Width = 8
                });

                // 记录当前节点和列
                var currentNode = treeList1.FocusedNode;
                var currentColumn = e.Column;

                repositoryItem.ButtonClick += (s, args) =>
                {
                    var editor = s as DevExpress.XtraEditors.ButtonEdit;
                    if (args.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)
                    {
                        Rectangle editorRect = editor.RectangleToScreen(editor.ClientRectangle);
                        var popupForm = new Hello_World.显示窗体.XtraForm2();
                        popupForm.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                        popupForm.StartPosition = FormStartPosition.Manual;

                        int screenHeight = Screen.FromControl(editor).WorkingArea.Height;
                        int mouseY = Control.MousePosition.Y;
                        int popupHeight = popupForm.Height > 0 ? popupForm.Height : 200;
                        Point location;
                        if (mouseY + popupHeight < screenHeight)
                        {
                            location = new Point(editorRect.Left, editorRect.Bottom);
                        }
                        else
                        {
                            location = new Point(editorRect.Left, editorRect.Top - popupHeight);
                        }
                        popupForm.Location = location;

                        // 回调：设置当前节点和列的内容
                        popupForm.OnCellSelected = (val) =>
                        {
                            if (currentNode != null && currentColumn != null)
                            {
                                currentNode.SetValue(currentColumn, val);
                            }
                        };
                        // 禁止拖动窗体
                        //popupForm.MaximizeBox = false;
                        //popupForm.MinimizeBox = false;
                        //popupForm.ControlBox = false; // 去掉标题栏按钮
                        popupForm.ShowDialog();
                    }
                    else if (args.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
                    {
                        if (currentNode != null && currentColumn != null)
                        {
                            currentNode.SetValue(currentColumn, string.Empty);
                        }
                    }
                };

                e.RepositoryItem = repositoryItem;
            }
        }
    }
}