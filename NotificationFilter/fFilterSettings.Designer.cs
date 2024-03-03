using System.Drawing;
using System.Windows.Forms;

namespace NotificationFilter
{
    partial class fFilterSettings
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataGridView1 = new DataGridView();
            bindingSource1 = new BindingSource(components);
            ruleListBindingSource = new BindingSource(components);
            messageTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            defaultActionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            ItemRuleId = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ruleListBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { messageTypeDataGridViewTextBoxColumn, defaultActionDataGridViewTextBoxColumn, ItemRuleId });
            dataGridView1.DataSource = ruleListBindingSource;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(731, 401);
            dataGridView1.TabIndex = 0;
            // 
            // bindingSource1
            // 
            bindingSource1.DataSource = typeof(Rules);
            // 
            // ruleListBindingSource
            // 
            ruleListBindingSource.DataMember = "RuleList";
            ruleListBindingSource.DataSource = bindingSource1;
            // 
            // messageTypeDataGridViewTextBoxColumn
            // 
            messageTypeDataGridViewTextBoxColumn.DataPropertyName = "MessageType";
            messageTypeDataGridViewTextBoxColumn.HeaderText = "MessageType";
            messageTypeDataGridViewTextBoxColumn.Name = "messageTypeDataGridViewTextBoxColumn";
            // 
            // defaultActionDataGridViewTextBoxColumn
            // 
            defaultActionDataGridViewTextBoxColumn.DataPropertyName = "DefaultAction";
            defaultActionDataGridViewTextBoxColumn.HeaderText = "DefaultAction";
            defaultActionDataGridViewTextBoxColumn.Name = "defaultActionDataGridViewTextBoxColumn";
            // 
            // ItemRuleId
            // 
            ItemRuleId.DataPropertyName = "ItemRules.Id";
            ItemRuleId.HeaderText = "ItemRule Id";
            ItemRuleId.Name = "ItemRuleId";
            // 
            // fFilterSettings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(731, 452);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "fFilterSettings";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            VisibleChanged += fFilterSettings_VisibleChanged;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ruleListBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private BindingSource bindingSource1;
        private BindingSource ruleListBindingSource;
        private DataGridViewTextBoxColumn messageTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn defaultActionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn ItemRuleId;
    }
}
