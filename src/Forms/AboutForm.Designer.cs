using System.Windows.Forms;

namespace RayCarrot.BizHawk.R1Tool
{
    public partial class AboutForm : Form
    {

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.infoLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(297, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(188, 46);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 13);
            this.infoLabel.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RayCarrot.BizHawk.R1Tool.Properties.Resources.AboutAnim;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 109);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 230);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.button1);
            this.MinimumSize = new System.Drawing.Size(376, 186);
            this.Name = "AboutForm";
            this.Text = "About - Rayman 1 Tool";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button button1;
        private Label infoLabel;
        private PictureBox pictureBox1;
    }
}
