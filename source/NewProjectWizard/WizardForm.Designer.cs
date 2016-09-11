namespace Uplift.VSExtensions
{
    partial class WizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardForm));
            this.label1 = new System.Windows.Forms.Label();
            this.serviceInterface = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.serviceImplClass = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.serviceNamespace = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.iisServiceRoot = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.iisPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nextButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service interface (eg \"IAuditService\")";
            // 
            // serviceInterface
            // 
            this.serviceInterface.Location = new System.Drawing.Point(217, 51);
            this.serviceInterface.Name = "serviceInterface";
            this.serviceInterface.Size = new System.Drawing.Size(159, 20);
            this.serviceInterface.TabIndex = 2;
            this.serviceInterface.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClearValidationErrors);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Implementation class (eg \"CAuditSvcImpl\")";
            // 
            // serviceImplClass
            // 
            this.serviceImplClass.Location = new System.Drawing.Point(217, 77);
            this.serviceImplClass.Name = "serviceImplClass";
            this.serviceImplClass.Size = new System.Drawing.Size(159, 20);
            this.serviceImplClass.TabIndex = 3;
            this.serviceImplClass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClearValidationErrors);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.serviceNamespace);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.serviceImplClass);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.serviceInterface);
            this.groupBox1.Location = new System.Drawing.Point(14, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(407, 115);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // serviceNamespace
            // 
            this.serviceNamespace.Location = new System.Drawing.Point(217, 25);
            this.serviceNamespace.Name = "serviceNamespace";
            this.serviceNamespace.Size = new System.Drawing.Size(159, 20);
            this.serviceNamespace.TabIndex = 1;
            this.serviceNamespace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClearValidationErrors);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(189, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Namespace (eg \"globetech.reporting\")";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.iisServiceRoot);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.iisPort);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(14, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 73);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Debug options (IIS Express)";
            // 
            // iisServiceRoot
            // 
            this.iisServiceRoot.Location = new System.Drawing.Point(276, 30);
            this.iisServiceRoot.Name = "iisServiceRoot";
            this.iisServiceRoot.Size = new System.Drawing.Size(100, 20);
            this.iisServiceRoot.TabIndex = 5;
            this.iisServiceRoot.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClearValidationErrors);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(155, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Service root (eg \"test\")";
            // 
            // iisPort
            // 
            this.iisPort.Location = new System.Drawing.Point(46, 30);
            this.iisPort.Name = "iisPort";
            this.iisPort.Size = new System.Drawing.Size(78, 20);
            this.iisPort.TabIndex = 4;
            this.iisPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ClearValidationErrors);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Port";
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(346, 233);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 6;
            this.nextButton.Text = "Next >";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // WizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 268);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WizardForm";
            this.Text = "New C++ SOAP web service";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox serviceInterface;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serviceImplClass;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox serviceNamespace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox iisServiceRoot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox iisPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}