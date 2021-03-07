
namespace Dek.Bel.Services.Categories
{
    partial class CategoryUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryUserControl));
            this.button_Categories = new System.Windows.Forms.Button();
            this.button_CategoryAddCreate = new System.Windows.Forms.Button();
            this.listBox_Categories = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_CategoryWeight = new System.Windows.Forms.ComboBox();
            this.textBox_CategorySearch = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel_Categories = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip_Category = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setAsMainCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWeight1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWeight2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWeight3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWeight4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWeight5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip_Category.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Categories
            // 
            this.button_Categories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Categories.Location = new System.Drawing.Point(476, -1);
            this.button_Categories.Name = "button_Categories";
            this.button_Categories.Size = new System.Drawing.Size(24, 24);
            this.button_Categories.TabIndex = 4;
            this.button_Categories.Text = "...";
            this.button_Categories.UseVisualStyleBackColor = true;
            this.button_Categories.Click += new System.EventHandler(this.button_Categories_Click);
            // 
            // button_CategoryAddCreate
            // 
            this.button_CategoryAddCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_CategoryAddCreate.Location = new System.Drawing.Point(401, -1);
            this.button_CategoryAddCreate.Name = "button_CategoryAddCreate";
            this.button_CategoryAddCreate.Size = new System.Drawing.Size(73, 24);
            this.button_CategoryAddCreate.TabIndex = 3;
            this.button_CategoryAddCreate.Text = "Add";
            this.button_CategoryAddCreate.UseVisualStyleBackColor = true;
            this.button_CategoryAddCreate.Click += new System.EventHandler(this.button_CategoryAddCreate_Click);
            // 
            // listBox_Categories
            // 
            this.listBox_Categories.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Categories.FormattingEnabled = true;
            this.listBox_Categories.ItemHeight = 14;
            this.listBox_Categories.Location = new System.Drawing.Point(112, 5);
            this.listBox_Categories.Name = "listBox_Categories";
            this.listBox_Categories.ScrollAlwaysVisible = true;
            this.listBox_Categories.Size = new System.Drawing.Size(138, 60);
            this.listBox_Categories.TabIndex = 10;
            this.listBox_Categories.TabStop = false;
            this.listBox_Categories.Visible = false;
            this.listBox_Categories.Click += new System.EventHandler(this.listBox_Categories_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 9999;
            this.label1.Text = "Weight";
            // 
            // comboBox_CategoryWeight
            // 
            this.comboBox_CategoryWeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_CategoryWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CategoryWeight.FormattingEnabled = true;
            this.comboBox_CategoryWeight.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBox_CategoryWeight.Location = new System.Drawing.Point(358, 0);
            this.comboBox_CategoryWeight.Name = "comboBox_CategoryWeight";
            this.comboBox_CategoryWeight.Size = new System.Drawing.Size(40, 22);
            this.comboBox_CategoryWeight.TabIndex = 2;
            // 
            // textBox_CategorySearch
            // 
            this.textBox_CategorySearch.AcceptsReturn = true;
            this.textBox_CategorySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_CategorySearch.Location = new System.Drawing.Point(1, 0);
            this.textBox_CategorySearch.Name = "textBox_CategorySearch";
            this.textBox_CategorySearch.Size = new System.Drawing.Size(296, 22);
            this.textBox_CategorySearch.TabIndex = 1;
            this.textBox_CategorySearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_CategorySearch_KeyDown);
            this.textBox_CategorySearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_CategorySearch_KeyUp);
            // 
            // flowLayoutPanel_Categories
            // 
            this.flowLayoutPanel_Categories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel_Categories.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel_Categories.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flowLayoutPanel_Categories.BackgroundImage")));
            this.flowLayoutPanel_Categories.Location = new System.Drawing.Point(1, 24);
            this.flowLayoutPanel_Categories.Name = "flowLayoutPanel_Categories";
            this.flowLayoutPanel_Categories.Size = new System.Drawing.Size(499, 131);
            this.flowLayoutPanel_Categories.TabIndex = 6;
            // 
            // contextMenuStrip_Category
            // 
            this.contextMenuStrip_Category.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setAsMainCategoryToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.setWeight1ToolStripMenuItem,
            this.setWeight2ToolStripMenuItem,
            this.setWeight3ToolStripMenuItem,
            this.setWeight4ToolStripMenuItem,
            this.setWeight5ToolStripMenuItem});
            this.contextMenuStrip_Category.Name = "contextMenuStrip_Category";
            this.contextMenuStrip_Category.Size = new System.Drawing.Size(184, 158);
            // 
            // setAsMainCategoryToolStripMenuItem
            // 
            this.setAsMainCategoryToolStripMenuItem.Name = "setAsMainCategoryToolStripMenuItem";
            this.setAsMainCategoryToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setAsMainCategoryToolStripMenuItem.Text = "Set as main category";
            this.setAsMainCategoryToolStripMenuItem.Click += new System.EventHandler(this.setAsMainCategoryToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // setWeight1ToolStripMenuItem
            // 
            this.setWeight1ToolStripMenuItem.Name = "setWeight1ToolStripMenuItem";
            this.setWeight1ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setWeight1ToolStripMenuItem.Text = "Set weight 1";
            this.setWeight1ToolStripMenuItem.Click += new System.EventHandler(this.setWeight1ToolStripMenuItem_Click);
            // 
            // setWeight2ToolStripMenuItem
            // 
            this.setWeight2ToolStripMenuItem.Name = "setWeight2ToolStripMenuItem";
            this.setWeight2ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setWeight2ToolStripMenuItem.Text = "Set weight 2";
            this.setWeight2ToolStripMenuItem.Click += new System.EventHandler(this.setWeight2ToolStripMenuItem_Click);
            // 
            // setWeight3ToolStripMenuItem
            // 
            this.setWeight3ToolStripMenuItem.Name = "setWeight3ToolStripMenuItem";
            this.setWeight3ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setWeight3ToolStripMenuItem.Text = "Set weight 3";
            this.setWeight3ToolStripMenuItem.Click += new System.EventHandler(this.setWeight3ToolStripMenuItem_Click);
            // 
            // setWeight4ToolStripMenuItem
            // 
            this.setWeight4ToolStripMenuItem.Name = "setWeight4ToolStripMenuItem";
            this.setWeight4ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setWeight4ToolStripMenuItem.Text = "Set weight 4";
            this.setWeight4ToolStripMenuItem.Click += new System.EventHandler(this.setWeight4ToolStripMenuItem_Click);
            // 
            // setWeight5ToolStripMenuItem
            // 
            this.setWeight5ToolStripMenuItem.Name = "setWeight5ToolStripMenuItem";
            this.setWeight5ToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.setWeight5ToolStripMenuItem.Text = "Set weight 5";
            this.setWeight5ToolStripMenuItem.Click += new System.EventHandler(this.setWeight5ToolStripMenuItem_Click);
            // 
            // CategoryUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.listBox_Categories);
            this.Controls.Add(this.flowLayoutPanel_Categories);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_CategoryWeight);
            this.Controls.Add(this.button_CategoryAddCreate);
            this.Controls.Add(this.button_Categories);
            this.Controls.Add(this.textBox_CategorySearch);
            this.Font = new System.Drawing.Font("Corbel", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CategoryUserControl";
            this.Size = new System.Drawing.Size(502, 156);
            this.Load += new System.EventHandler(this.UserControl_Category_Load);
            this.Enter += new System.EventHandler(this.UserControl_Category_Enter);
            this.contextMenuStrip_Category.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Categories;
        private System.Windows.Forms.Button button_CategoryAddCreate;
        private System.Windows.Forms.ListBox listBox_Categories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_CategoryWeight;
        private System.Windows.Forms.TextBox textBox_CategorySearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Categories;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Category;
        private System.Windows.Forms.ToolStripMenuItem setAsMainCategoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeight1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeight2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeight3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeight4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeight5ToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
