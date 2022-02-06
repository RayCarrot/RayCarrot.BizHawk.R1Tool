using System;
using System.Windows.Forms;

namespace RayCarrot.BizHawk.R1Tool;

public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        infoLabel.Text = $"{AppConstants.Title} - Version {AppConstants.Version.ToString(3)}{Environment.NewLine}Created by RayCarrot";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Close();
    }
}