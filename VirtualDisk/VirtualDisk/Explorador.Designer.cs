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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Explorador));
            this.LstPropDisco = new System.Windows.Forms.ListBox();
            this.Disco = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.NuevaCarpeta = new System.Windows.Forms.ToolStripButton();
            this.Agregar = new System.Windows.Forms.ToolStripButton();
            this.Extraer = new System.Windows.Forms.ToolStripButton();
            this.Borrar = new System.Windows.Forms.ToolStripButton();
            this.viewGeneral = new System.Windows.Forms.ListView();
            this.Nombre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Fecha_Hora = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folder = new System.Windows.Forms.Label();
            this.BorrarCarpeta = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LstPropDisco
            // 
            this.LstPropDisco.FormattingEnabled = true;
            this.LstPropDisco.Location = new System.Drawing.Point(12, 28);
            this.LstPropDisco.Name = "LstPropDisco";
            this.LstPropDisco.Size = new System.Drawing.Size(195, 316);
            this.LstPropDisco.TabIndex = 0;
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
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NuevaCarpeta,
            this.BorrarCarpeta,
            this.Agregar,
            this.Extraer,
            this.Borrar});
            this.toolStrip1.Location = new System.Drawing.Point(210, 55);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(439, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // NuevaCarpeta
            // 
            this.NuevaCarpeta.Image = ((System.Drawing.Image)(resources.GetObject("NuevaCarpeta.Image")));
            this.NuevaCarpeta.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.NuevaCarpeta.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NuevaCarpeta.Name = "NuevaCarpeta";
            this.NuevaCarpeta.Size = new System.Drawing.Size(105, 22);
            this.NuevaCarpeta.Text = "Nueva Carpeta";
            this.NuevaCarpeta.Click += new System.EventHandler(this.NuevaCarpeta_Click);
            // 
            // Agregar
            // 
            this.Agregar.Image = ((System.Drawing.Image)(resources.GetObject("Agregar.Image")));
            this.Agregar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Agregar.Name = "Agregar";
            this.Agregar.Size = new System.Drawing.Size(113, 22);
            this.Agregar.Text = "Agregar Archivo";
            this.Agregar.Click += new System.EventHandler(this.Agregar_Click);
            // 
            // Extraer
            // 
            this.Extraer.Image = ((System.Drawing.Image)(resources.GetObject("Extraer.Image")));
            this.Extraer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Extraer.Name = "Extraer";
            this.Extraer.Size = new System.Drawing.Size(106, 22);
            this.Extraer.Text = "Extraer Archivo";
            this.Extraer.Click += new System.EventHandler(this.Extraer_Click);
            // 
            // Borrar
            // 
            this.Borrar.Image = ((System.Drawing.Image)(resources.GetObject("Borrar.Image")));
            this.Borrar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Borrar.Name = "Borrar";
            this.Borrar.Size = new System.Drawing.Size(103, 22);
            this.Borrar.Text = "Borrar Archivo";
            this.Borrar.Click += new System.EventHandler(this.Borrar_Click);
            // 
            // viewGeneral
            // 
            this.viewGeneral.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nombre,
            this.Fecha_Hora,
            this.Size});
            this.viewGeneral.Location = new System.Drawing.Point(210, 83);
            this.viewGeneral.MultiSelect = false;
            this.viewGeneral.Name = "viewGeneral";
            this.viewGeneral.Size = new System.Drawing.Size(462, 261);
            this.viewGeneral.SmallImageList = this.imageList1;
            this.viewGeneral.TabIndex = 9;
            this.viewGeneral.UseCompatibleStateImageBehavior = false;
            this.viewGeneral.View = System.Windows.Forms.View.Details;
            this.viewGeneral.SelectedIndexChanged += new System.EventHandler(this.viewGeneral_SelectedIndexChanged);
            this.viewGeneral.DoubleClick += new System.EventHandler(this.viewGeneral_DoubleClick_1);
            // 
            // Nombre
            // 
            this.Nombre.Text = "Nombre";
            this.Nombre.Width = 134;
            // 
            // Fecha_Hora
            // 
            this.Fecha_Hora.Text = "Fecha_Hora_Creacion";
            this.Fecha_Hora.Width = 138;
            // 
            // Size
            // 
            this.Size.Text = "Tamaño";
            this.Size.Width = 104;
            // 
            // folder
            // 
            this.folder.AutoSize = true;
            this.folder.Location = new System.Drawing.Point(213, 28);
            this.folder.Name = "folder";
            this.folder.Size = new System.Drawing.Size(0, 13);
            this.folder.TabIndex = 10;
            // 
            // BorrarCarpeta
            // 
            this.BorrarCarpeta.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BorrarCarpeta.Image = ((System.Drawing.Image)(resources.GetObject("BorrarCarpeta.Image")));
            this.BorrarCarpeta.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BorrarCarpeta.Name = "BorrarCarpeta";
            this.BorrarCarpeta.Size = new System.Drawing.Size(23, 22);
            this.BorrarCarpeta.Text = "Borrar Carpeta";
            this.BorrarCarpeta.ToolTipText = "Borrar Carpeta";
            this.BorrarCarpeta.Visible = false;
            this.BorrarCarpeta.Click += new System.EventHandler(this.BorrarCarpeta_Click_1);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder.png");
            // 
            // Explorador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 416);
            this.Controls.Add(this.folder);
            this.Controls.Add(this.viewGeneral);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.LstPropDisco);
            this.Controls.Add(this.Disco);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Explorador";
            this.Text = "Explorador";
            this.Load += new System.EventHandler(this.Explorador_Load_1);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LstPropDisco;
        private System.Windows.Forms.Label Disco;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton NuevaCarpeta;
        private System.Windows.Forms.ToolStripButton Agregar;
        private System.Windows.Forms.ToolStripButton Extraer;
        private System.Windows.Forms.ToolStripButton Borrar;
        private System.Windows.Forms.ListView viewGeneral;
        private System.Windows.Forms.ColumnHeader Nombre;
        private System.Windows.Forms.ColumnHeader Fecha_Hora;
        private System.Windows.Forms.ColumnHeader Size;
        private System.Windows.Forms.Label folder;
        private System.Windows.Forms.ToolStripButton BorrarCarpeta;
        private System.Windows.Forms.ImageList imageList1;
    }
}