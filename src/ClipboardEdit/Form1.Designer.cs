namespace ClipboardEdit
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.menuItem22 = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItem24 = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.menuItem41 = new System.Windows.Forms.MenuItem();
            this.menuItem30 = new System.Windows.Forms.MenuItem();
            this.menuItem31 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem32 = new System.Windows.Forms.MenuItem();
            this.menuItem33 = new System.Windows.Forms.MenuItem();
            this.menuItem34 = new System.Windows.Forms.MenuItem();
            this.menuItem35 = new System.Windows.Forms.MenuItem();
            this.menuItem36 = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.menuItem37 = new System.Windows.Forms.MenuItem();
            this.menuItem38 = new System.Windows.Forms.MenuItem();
            this.menuItem40 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem39 = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new WindowsFormsAero.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.hexBox1 = new Be.Windows.Forms.HexBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStatusDescriptionManager1 = new ClipboardEdit.Controls.MenuStatusDescriptionManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem11,
            this.menuItem12,
            this.menuItem14,
            this.menuItem13});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7,
            this.menuItem6,
            this.menuItem8,
            this.menuItem9,
            this.menuItem4,
            this.menuItem5,
            this.menuItem2,
            this.menuItem10,
            this.menuItem3});
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem1, null);
            this.menuItem1.Text = "&File";
            this.menuItem1.Popup += new System.EventHandler(this.menuItem1_Popup);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem7, "Open a snapshot file to view the clipboard contents as they were at the time it w" +
        "as created.");
            this.menuItem7.Text = "&Open snapshot...";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem6, "Save a snapshot to save the current clipboard data to view later.");
            this.menuItem6.Text = "&Save snapshot...";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 2;
            this.menuItem8.Text = "-";
            // 
            // menuItem9
            // 
            this.menuItem9.Enabled = false;
            this.menuItem9.Index = 3;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem9, "Close the current snapshot and return to the live clipboard view.");
            this.menuItem9.Text = "&Close snapshot";
            this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Shortcut = System.Windows.Forms.Shortcut.F9;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem4, "Pause or resume checking for clipboard changes.");
            this.menuItem4.Text = "&Pause updates";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 5;
            this.menuItem5.Text = "-";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 6;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem2, "View more information about ClipboardEdit.");
            this.menuItem2.Text = "&About ClipboardEdit...";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 7;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem10, "Configure your ClipboardEdit preferences.");
            this.menuItem10.Text = "S&ettings...";
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 8;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem3, "Exit ClipboardEdit.");
            this.menuItem3.Text = "E&xit";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem21,
            this.menuItem22,
            this.menuItem28,
            this.menuItem23,
            this.menuItem24,
            this.menuItem25,
            this.menuItem26,
            this.menuItem27,
            this.menuItem29,
            this.menuItem41,
            this.menuItem30,
            this.menuItem31});
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem11, null);
            this.menuItem11.Text = "&Edit";
            this.menuItem11.Popup += new System.EventHandler(this.menuItem11_Popup);
            // 
            // menuItem21
            // 
            this.menuItem21.Enabled = false;
            this.menuItem21.Index = 0;
            this.menuItem21.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem21, null);
            this.menuItem21.Text = "&Cut";
            this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
            // 
            // menuItem22
            // 
            this.menuItem22.Enabled = false;
            this.menuItem22.Index = 1;
            this.menuItem22.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem22, null);
            this.menuItem22.Text = "C&opy";
            this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
            // 
            // menuItem28
            // 
            this.menuItem28.Enabled = false;
            this.menuItem28.Index = 2;
            this.menuItem28.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftC;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem28, null);
            this.menuItem28.Text = "Copy h&ex";
            this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
            // 
            // menuItem23
            // 
            this.menuItem23.Enabled = false;
            this.menuItem23.Index = 3;
            this.menuItem23.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem23, null);
            this.menuItem23.Text = "&Paste";
            this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
            // 
            // menuItem24
            // 
            this.menuItem24.Enabled = false;
            this.menuItem24.Index = 4;
            this.menuItem24.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftV;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem24, null);
            this.menuItem24.Text = "Paste &hex data";
            this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 5;
            this.menuItem25.Text = "-";
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 6;
            this.menuItem26.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem26, null);
            this.menuItem26.Text = "&Select all";
            this.menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 7;
            this.menuItem27.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem27, null);
            this.menuItem27.Text = "&Find";
            this.menuItem27.Click += new System.EventHandler(this.menuItem27_Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 8;
            this.menuItem29.Text = "-";
            // 
            // menuItem41
            // 
            this.menuItem41.Index = 9;
            this.menuItem41.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem41, null);
            this.menuItem41.Text = "&Refresh";
            this.menuItem41.Click += new System.EventHandler(this.menuItem41_Click);
            // 
            // menuItem30
            // 
            this.menuItem30.Index = 10;
            this.menuItem30.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem30, null);
            this.menuItem30.Text = "&Show documentation...";
            this.menuItem30.Click += new System.EventHandler(this.menuItem37_Click);
            // 
            // menuItem31
            // 
            this.menuItem31.Enabled = false;
            this.menuItem31.Index = 11;
            this.menuItem31.Shortcut = System.Windows.Forms.Shortcut.F8;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem31, null);
            this.menuItem31.Text = "&Write changes";
            this.menuItem31.Click += new System.EventHandler(this.menuItem31_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem32,
            this.menuItem33,
            this.menuItem34,
            this.menuItem35,
            this.menuItem36});
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem12, null);
            this.menuItem12.Text = "&View";
            // 
            // menuItem32
            // 
            this.menuItem32.Checked = true;
            this.menuItem32.Index = 0;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem32, null);
            this.menuItem32.Text = "&Format list";
            this.menuItem32.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem33
            // 
            this.menuItem33.Checked = true;
            this.menuItem33.Index = 1;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem33, null);
            this.menuItem33.Text = "&Property window";
            this.menuItem33.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem34
            // 
            this.menuItem34.Checked = true;
            this.menuItem34.Index = 2;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem34, null);
            this.menuItem34.Text = "&Hex view";
            this.menuItem34.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem35
            // 
            this.menuItem35.Checked = true;
            this.menuItem35.Index = 3;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem35, null);
            this.menuItem35.Text = "&Toolbar";
            this.menuItem35.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem36
            // 
            this.menuItem36.Checked = true;
            this.menuItem36.Index = 4;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem36, null);
            this.menuItem36.Text = "&Status bar";
            this.menuItem36.Click += new System.EventHandler(this.menuItem32_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 3;
            this.menuItem14.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem15,
            this.menuItem37,
            this.menuItem38,
            this.menuItem40,
            this.menuItem16});
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem14, null);
            this.menuItem14.Text = "&Format";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 0;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem15, null);
            this.menuItem15.Text = "&View registered formats...";
            this.menuItem15.Click += new System.EventHandler(this.menuItem15_Click);
            // 
            // menuItem37
            // 
            this.menuItem37.Index = 1;
            this.menuItem37.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem37, null);
            this.menuItem37.Text = "&Show documentation...";
            this.menuItem37.Click += new System.EventHandler(this.menuItem37_Click);
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 2;
            this.menuItem38.Text = "-";
            // 
            // menuItem40
            // 
            this.menuItem40.Enabled = false;
            this.menuItem40.Index = 3;
            this.menuItem40.Shortcut = System.Windows.Forms.Shortcut.F8;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem40, null);
            this.menuItem40.Text = "&Write changes";
            this.menuItem40.Click += new System.EventHandler(this.menuItem31_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 4;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem16, null);
            this.menuItem16.Text = "&Fill formats (!)";
            this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 4;
            this.menuItem13.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem20,
            this.menuItem39,
            this.menuItem19,
            this.menuItem17,
            this.menuItem18});
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem13, null);
            this.menuItem13.Text = "&Help";
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 0;
            this.menuItem20.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem20, null);
            this.menuItem20.Text = "&Help Contents";
            this.menuItem20.Click += new System.EventHandler(this.menuItem20_Click);
            // 
            // menuItem39
            // 
            this.menuItem39.Index = 1;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem39, null);
            this.menuItem39.Text = "&Update documentation...";
            this.menuItem39.Click += new System.EventHandler(this.menuItem39_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 2;
            this.menuItem19.Text = "-";
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 3;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem17, null);
            this.menuItem17.Text = "&About ClipboardEdit...";
            this.menuItem17.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 4;
            this.menuStatusDescriptionManager1.SetStatusDescription(this.menuItem18, null);
            this.menuItem18.Text = "&ClipboardEdit GitHub...";
            this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.toolBarButton2,
            this.toolBarButton3,
            this.toolBarButton4,
            this.toolBarButton5,
            this.toolBarButton9,
            this.toolBarButton8,
            this.toolBarButton7,
            this.toolBarButton6});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(736, 44);
            this.toolBar1.TabIndex = 1;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.ToolTipText = "Open snapshot";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 1;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.ToolTipText = "Save snapshot";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.ImageIndex = 5;
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.toolBarButton4.ToolTipText = "Pause updates";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.ImageIndex = 2;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.ToolTipText = "View registered formats";
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.Enabled = false;
            this.toolBarButton9.ImageIndex = 7;
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.ToolTipText = "Write changes";
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 3;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.ToolTipText = "Settings";
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.ImageIndex = 4;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.ToolTipText = "About ClipboardEdit";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "open.png");
            this.imageList1.Images.SetKeyName(1, "save.png");
            this.imageList1.Images.SetKeyName(2, "checkboard.png");
            this.imageList1.Images.SetKeyName(3, "config.png");
            this.imageList1.Images.SetKeyName(4, "info.png");
            this.imageList1.Images.SetKeyName(5, "pause.png");
            this.imageList1.Images.SetKeyName(6, "play.png");
            this.imageList1.Images.SetKeyName(7, "writeboard.png");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 44);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.hexBox1);
            this.splitContainer1.Size = new System.Drawing.Size(736, 348);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(736, 178);
            this.splitContainer2.SplitterDistance = 489;
            this.splitContainer2.TabIndex = 0;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(489, 178);
            this.listView1.TabIndex = 0;
            this.listView1.TileSize = new System.Drawing.Size(228, 51);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.DisabledItemForeColor = System.Drawing.SystemColors.ControlText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(243, 178);
            this.propertyGrid1.TabIndex = 0;
            // 
            // hexBox1
            // 
            // 
            // 
            // 
            this.hexBox1.BuiltInContextMenu.CopyMenuItemText = "&Copy";
            this.hexBox1.BuiltInContextMenu.CutMenuItemText = "C&ut";
            this.hexBox1.BuiltInContextMenu.PasteMenuItemText = "&Paste";
            this.hexBox1.BuiltInContextMenu.SelectAllMenuItemText = "&Select all";
            this.hexBox1.ColumnInfoVisible = true;
            this.hexBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hexBox1.LineInfoVisible = true;
            this.hexBox1.Location = new System.Drawing.Point(0, 0);
            this.hexBox1.Name = "hexBox1";
            this.hexBox1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.hexBox1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.hexBox1.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hexBox1.Size = new System.Drawing.Size(736, 166);
            this.hexBox1.StringViewVisible = true;
            this.hexBox1.TabIndex = 0;
            this.hexBox1.VScrollBarVisible = true;
            // 
            // statusBar1
            // 
            this.statusBar1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.statusBar1.Location = new System.Drawing.Point(0, 392);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(736, 22);
            this.statusBar1.TabIndex = 3;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "snapshot";
            this.saveFileDialog1.Filter = "Clipboard Snapshot (*.csnap)|*.csnap|Compressed Clipboard Snapshot (*.csnapz)|*.c" +
    "snapz";
            this.saveFileDialog1.Title = "Save Snapshot";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "snapshot";
            this.openFileDialog1.Filter = "Snapshot files (*.csnap; *.csnapz)|*.csnap;*.csnapz|Clipboard Snapshot (*.csnap)|" +
    "*.csnap|Compressed Clipboard Snapshot (*.csnapz)|*.csnapz";
            this.openFileDialog1.Title = "Open Snapshot";
            // 
            // menuStatusDescriptionManager1
            // 
            this.menuStatusDescriptionManager1.HideDisabled = true;
            this.menuStatusDescriptionManager1.StatusBar = this.statusBar1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 414);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.toolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "ClipboardEdit";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private WindowsFormsAero.ListView listView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private Be.Windows.Forms.HexBox hexBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.MenuItem menuItem20;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem menuItem18;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private System.Windows.Forms.ToolBarButton toolBarButton3;
        private System.Windows.Forms.ToolBarButton toolBarButton4;
        private System.Windows.Forms.ToolBarButton toolBarButton5;
        private System.Windows.Forms.ToolBarButton toolBarButton8;
        private System.Windows.Forms.ToolBarButton toolBarButton6;
        private System.Windows.Forms.ToolBarButton toolBarButton7;
        private System.Windows.Forms.ToolBarButton toolBarButton9;
        private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem menuItem22;
        private System.Windows.Forms.MenuItem menuItem23;
        private System.Windows.Forms.MenuItem menuItem24;
        private System.Windows.Forms.MenuItem menuItem25;
        private System.Windows.Forms.MenuItem menuItem26;
        private System.Windows.Forms.MenuItem menuItem27;
        private System.Windows.Forms.MenuItem menuItem29;
        private System.Windows.Forms.MenuItem menuItem30;
        private System.Windows.Forms.MenuItem menuItem31;
        private System.Windows.Forms.MenuItem menuItem32;
        private System.Windows.Forms.MenuItem menuItem33;
        private System.Windows.Forms.MenuItem menuItem34;
        private System.Windows.Forms.MenuItem menuItem35;
        private System.Windows.Forms.MenuItem menuItem36;
        private System.Windows.Forms.MenuItem menuItem37;
        private System.Windows.Forms.MenuItem menuItem38;
        private System.Windows.Forms.MenuItem menuItem39;
        private System.Windows.Forms.MenuItem menuItem40;
        private System.Windows.Forms.StatusBar statusBar1;
        private Controls.MenuStatusDescriptionManager menuStatusDescriptionManager1;
        private System.Windows.Forms.MenuItem menuItem41;
        private System.Windows.Forms.MenuItem menuItem28;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

