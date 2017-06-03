namespace VirtualDisk
{
    partial class Explorador
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
            this.LstPropDisco = new System.Windows.Forms.ListBox();
            this.Extraer = new System.Windows.Forms.Button();
            this.Disco = new System.Windows.Forms.Label();
            this.Agregar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LstPropDisco
            // 
            this.LstPropDisco.FormattingEnabled = true;
            this.LstPropDisco.Location = new System.Drawing.Point(12, 23);
            this.LstPropDisco.Name = "LstPropDisco";
            this.LstPropDisco.Size = new System.Drawing.Size(195, 316);
            this.LstPropDisco.TabIndex = 0;
            // 
            // Extraer
            // 
            this.Extraer.Location = new System.Drawing.Point(224, 64);
            this.Extraer.Name = "Extraer";
            this.Extraer.Size = new System.Drawing.Size(105, 37);
            this.Extraer.TabIndex = 7;
            this.Extraer.Text = "Sacar Archivo";
            this.Extraer.UseVisualStyleBackColor = true;
            // 
            // Disco
            // 
            this.Disco.AutoSize = true;
            this.Disco.Location = new System.Drawing.Point(12, 7);
            this.Disco.Name = "Disco";
            this.Disco.Size = new System.Drawing.Size(113, 13);
            this.Disco.TabIndex = 6;
            this.Disco.Text = "Propiedades del Disco";
            // 
            // Agregar
            // 
            this.Agregar.Location = new System.Drawing.Point(224, 23);
            this.Agregar.Name = "Agregar";
            this.Agregar.Size = new System.Drawing.Size(105, 35);
            this.Agregar.TabIndex = 5;
            this.Agregar.Text = "Agregar Archivo";
            this.Agregar.UseVisualStyleBackColor = true;
            // 
            // Explorador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 341);
            this.Controls.Add(this.LstPropDisco);
            this.Controls.Add(this.Extraer);
            this.Controls.Add(this.Disco);
            this.Controls.Add(this.Agregar);
            this.Name = "Explorador";
            this.Text = "Explorador";
            this.Load += new System.EventHandler(this.Explorador_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstPropDisco;
        private System.Windows.Forms.Button Extraer;
        private System.Windows.Forms.Label Disco;
        private System.Windows.Forms.Button Agregar;
    }
}