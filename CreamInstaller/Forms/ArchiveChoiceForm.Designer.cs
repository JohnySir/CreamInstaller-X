using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CreamInstaller.Forms
{
    partial class ArchiveChoiceForm
    {
        private IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components is not null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cancelButton = new Button();
            noButton = new Button();
            acceptButton = new Button();
            descriptionLabel = new Label();
            icon = new PictureBox();
            descriptionPanel = new FlowLayoutPanel();
            buttonPanel = new FlowLayoutPanel();
            ((ISupportInitialize)icon).BeginInit();
            descriptionPanel.SuspendLayout();
            buttonPanel.SuspendLayout();
            SuspendLayout();
            //
            // cancelButton
            //
            cancelButton.AutoSize = true;
            cancelButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Location = new Point(10, 10);
            cancelButton.Name = "cancelButton";
            cancelButton.Padding = new Padding(12, 0, 12, 0);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            //
            // noButton
            //
            noButton.AutoSize = true;
            noButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            noButton.DialogResult = DialogResult.No;
            noButton.Location = new Point(10, 10);
            noButton.Name = "noButton";
            noButton.Padding = new Padding(12, 0, 12, 0);
            noButton.TabIndex = 1;
            noButton.Text = "Install Without Archive";
            noButton.UseVisualStyleBackColor = true;
            //
            // acceptButton
            //
            acceptButton.AutoSize = true;
            acceptButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            acceptButton.DialogResult = DialogResult.Yes;
            acceptButton.Location = new Point(10, 10);
            acceptButton.Name = "acceptButton";
            acceptButton.Padding = new Padding(12, 0, 12, 0);
            acceptButton.TabIndex = 0;
            acceptButton.Text = "Install and Create Archive";
            acceptButton.UseVisualStyleBackColor = true;
            //
            // descriptionLabel
            //
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(66, 0);
            descriptionLabel.Margin = new Padding(9, 0, 3, 0);
            descriptionLabel.MaximumSize = new Size(500, 0);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(400, 15);
            descriptionLabel.TabIndex = 3;
            descriptionLabel.Text =
                "Replace the game files and install the DLC unlocker(s).\n\n"
                + "Should a ZIP archive of all installed unlocker files (including the original DLL backups) also be created for later use?\n\n"
                + "The archive is stored in a separate \"_DLC Unlocker\" folder inside the game folder, keeping the original file structure.";
            //
            // icon
            //
            icon.Location = new Point(15, 15);
            icon.Name = "icon";
            icon.Size = new Size(48, 48);
            icon.SizeMode = PictureBoxSizeMode.AutoSize;
            icon.TabIndex = 4;
            icon.TabStop = false;
            //
            // descriptionPanel
            //
            descriptionPanel.AutoSize = true;
            descriptionPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            descriptionPanel.Controls.Add(icon);
            descriptionPanel.Controls.Add(descriptionLabel);
            descriptionPanel.Dock = DockStyle.Fill;
            descriptionPanel.Location = new Point(0, 0);
            descriptionPanel.Margin = new Padding(0);
            descriptionPanel.Name = "descriptionPanel";
            descriptionPanel.Padding = new Padding(12, 12, 12, 6);
            descriptionPanel.Size = new Size(384, 72);
            descriptionPanel.TabIndex = 5;
            //
            // buttonPanel
            //
            buttonPanel.AutoSize = true;
            buttonPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            buttonPanel.Controls.Add(acceptButton);
            buttonPanel.Controls.Add(noButton);
            buttonPanel.Controls.Add(cancelButton);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonPanel.Location = new Point(0, 72);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(12, 6, 0, 12);
            buttonPanel.Size = new Size(384, 49);
            buttonPanel.TabIndex = 6;
            //
            // ArchiveChoiceForm
            //
            AcceptButton = acceptButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = cancelButton;
            ClientSize = new Size(384, 121);
            Controls.Add(descriptionPanel);
            Controls.Add(buttonPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new Size(1600, 900);
            MinimizeBox = false;
            MinimumSize = new Size(400, 160);
            Name = "ArchiveChoiceForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Archive Choice";
            ((ISupportInitialize)icon).EndInit();
            descriptionPanel.ResumeLayout(false);
            descriptionPanel.PerformLayout();
            buttonPanel.ResumeLayout(false);
            buttonPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button cancelButton;
        private Button noButton;
        private Button acceptButton;
        private PictureBox icon;
        private FlowLayoutPanel descriptionPanel;
        private FlowLayoutPanel buttonPanel;
        private Label descriptionLabel;
    }
}
