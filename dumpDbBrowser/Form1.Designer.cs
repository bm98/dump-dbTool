namespace dumpDbBrowser
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
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) ) {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.btLoad = new System.Windows.Forms.Button();
      this.OFD = new System.Windows.Forms.OpenFileDialog();
      this.btLoadTsv = new System.Windows.Forms.Button();
      this.RTB = new System.Windows.Forms.RichTextBox();
      this.btWriteDb = new System.Windows.Forms.Button();
      this.btLoadBS = new System.Windows.Forms.Button();
      this.btLoadAircraftCsv = new System.Windows.Forms.Button();
      this.btWriteAircraftDB = new System.Windows.Forms.Button();
      this.btWriteRouteDb = new System.Windows.Forms.Button();
      this.btLoadRouteTsv = new System.Windows.Forms.Button();
      this.btWriteRtCSV = new System.Windows.Forms.Button();
      this.btWriteAcCSV = new System.Windows.Forms.Button();
      this.btWriteIcaoCSV = new System.Windows.Forms.Button();
      this.btWriteAirportCsv = new System.Windows.Forms.Button();
      this.btLoadAirportCsv = new System.Windows.Forms.Button();
      this.btWriteIcaoJsonCSV = new System.Windows.Forms.Button();
      this.btWriteNavGJson = new System.Windows.Forms.Button();
      this.btLoadNavCSV = new System.Windows.Forms.Button();
      this.btWriteAirportGJson = new System.Windows.Forms.Button();
      this.chkRangeLimit = new System.Windows.Forms.CheckBox();
      this.txRangeLimit = new System.Windows.Forms.TextBox();
      this.txMyLat = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txMyLon = new System.Windows.Forms.TextBox();
      this.btXP11nav = new System.Windows.Forms.Button();
      this.btXP11fix = new System.Windows.Forms.Button();
      this.btXP11awy = new System.Windows.Forms.Button();
      this.btWriteAwyGJson = new System.Windows.Forms.Button();
      this.btWriteIcaoSQDB = new System.Windows.Forms.Button();
      this.btWriteFlightsSQDB = new System.Windows.Forms.Button();
      this.btLoadIcaoAct = new System.Windows.Forms.Button();
      this.btWriteIcaoAct = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // btLoad
      // 
      this.btLoad.Location = new System.Drawing.Point(12, 334);
      this.btLoad.Name = "btLoad";
      this.btLoad.Size = new System.Drawing.Size(124, 36);
      this.btLoad.TabIndex = 0;
      this.btLoad.Text = "Load Json";
      this.btLoad.UseVisualStyleBackColor = true;
      this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
      // 
      // OFD
      // 
      this.OFD.FileName = "dbFolder";
      // 
      // btLoadTsv
      // 
      this.btLoadTsv.Location = new System.Drawing.Point(142, 334);
      this.btLoadTsv.Name = "btLoadTsv";
      this.btLoadTsv.Size = new System.Drawing.Size(76, 36);
      this.btLoadTsv.TabIndex = 1;
      this.btLoadTsv.Text = "Load tsv...";
      this.btLoadTsv.UseVisualStyleBackColor = true;
      this.btLoadTsv.Click += new System.EventHandler(this.btLoadTsv_Click);
      // 
      // RTB
      // 
      this.RTB.Location = new System.Drawing.Point(12, 376);
      this.RTB.Name = "RTB";
      this.RTB.Size = new System.Drawing.Size(761, 279);
      this.RTB.TabIndex = 2;
      this.RTB.Text = "";
      // 
      // btWriteDb
      // 
      this.btWriteDb.Location = new System.Drawing.Point(272, 12);
      this.btWriteDb.Name = "btWriteDb";
      this.btWriteDb.Size = new System.Drawing.Size(124, 36);
      this.btWriteDb.TabIndex = 3;
      this.btWriteDb.Text = "Write FA Json DB";
      this.btWriteDb.UseVisualStyleBackColor = true;
      this.btWriteDb.Click += new System.EventHandler(this.btWriteDb_Click);
      // 
      // btLoadBS
      // 
      this.btLoadBS.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btLoadBS.Location = new System.Drawing.Point(12, 12);
      this.btLoadBS.Name = "btLoadBS";
      this.btLoadBS.Size = new System.Drawing.Size(124, 36);
      this.btLoadBS.TabIndex = 4;
      this.btLoadBS.Text = "Load BaseStation...";
      this.btLoadBS.UseVisualStyleBackColor = true;
      this.btLoadBS.Click += new System.EventHandler(this.btLoadBS_Click);
      // 
      // btLoadAircraftCsv
      // 
      this.btLoadAircraftCsv.Location = new System.Drawing.Point(12, 54);
      this.btLoadAircraftCsv.Name = "btLoadAircraftCsv";
      this.btLoadAircraftCsv.Size = new System.Drawing.Size(124, 36);
      this.btLoadAircraftCsv.TabIndex = 5;
      this.btLoadAircraftCsv.Text = "Load Aircraft CSV...";
      this.btLoadAircraftCsv.UseVisualStyleBackColor = true;
      this.btLoadAircraftCsv.Click += new System.EventHandler(this.btLoadAircraftCsv_Click);
      // 
      // btWriteAircraftDB
      // 
      this.btWriteAircraftDB.Location = new System.Drawing.Point(272, 54);
      this.btWriteAircraftDB.Name = "btWriteAircraftDB";
      this.btWriteAircraftDB.Size = new System.Drawing.Size(124, 36);
      this.btWriteAircraftDB.TabIndex = 6;
      this.btWriteAircraftDB.Text = "Write Aircraft Json";
      this.btWriteAircraftDB.UseVisualStyleBackColor = true;
      this.btWriteAircraftDB.Click += new System.EventHandler(this.btWriteAircraftDB_Click);
      // 
      // btWriteRouteDb
      // 
      this.btWriteRouteDb.Location = new System.Drawing.Point(272, 138);
      this.btWriteRouteDb.Name = "btWriteRouteDb";
      this.btWriteRouteDb.Size = new System.Drawing.Size(124, 36);
      this.btWriteRouteDb.TabIndex = 8;
      this.btWriteRouteDb.Text = "Write Route Json";
      this.btWriteRouteDb.UseVisualStyleBackColor = true;
      this.btWriteRouteDb.Click += new System.EventHandler(this.btWriteRouteDb_Click);
      // 
      // btLoadRouteTsv
      // 
      this.btLoadRouteTsv.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btLoadRouteTsv.Location = new System.Drawing.Point(12, 138);
      this.btLoadRouteTsv.Name = "btLoadRouteTsv";
      this.btLoadRouteTsv.Size = new System.Drawing.Size(124, 36);
      this.btLoadRouteTsv.TabIndex = 7;
      this.btLoadRouteTsv.Text = "Load Route TSV...";
      this.btLoadRouteTsv.UseVisualStyleBackColor = true;
      this.btLoadRouteTsv.Click += new System.EventHandler(this.btLoadRouteTsv_Click);
      // 
      // btWriteRtCSV
      // 
      this.btWriteRtCSV.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteRtCSV.Location = new System.Drawing.Point(412, 138);
      this.btWriteRtCSV.Name = "btWriteRtCSV";
      this.btWriteRtCSV.Size = new System.Drawing.Size(124, 36);
      this.btWriteRtCSV.TabIndex = 11;
      this.btWriteRtCSV.Text = "Write Route CSV";
      this.btWriteRtCSV.UseVisualStyleBackColor = true;
      this.btWriteRtCSV.Click += new System.EventHandler(this.btWriteRtCSV_Click);
      // 
      // btWriteAcCSV
      // 
      this.btWriteAcCSV.Location = new System.Drawing.Point(412, 54);
      this.btWriteAcCSV.Name = "btWriteAcCSV";
      this.btWriteAcCSV.Size = new System.Drawing.Size(124, 36);
      this.btWriteAcCSV.TabIndex = 10;
      this.btWriteAcCSV.Text = "Write Aircraft CSV";
      this.btWriteAcCSV.UseVisualStyleBackColor = true;
      this.btWriteAcCSV.Click += new System.EventHandler(this.btWriteAcCSV_Click);
      // 
      // btWriteIcaoCSV
      // 
      this.btWriteIcaoCSV.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteIcaoCSV.Location = new System.Drawing.Point(412, 12);
      this.btWriteIcaoCSV.Name = "btWriteIcaoCSV";
      this.btWriteIcaoCSV.Size = new System.Drawing.Size(124, 36);
      this.btWriteIcaoCSV.TabIndex = 9;
      this.btWriteIcaoCSV.Text = "Write FA CSV";
      this.btWriteIcaoCSV.UseVisualStyleBackColor = true;
      this.btWriteIcaoCSV.Click += new System.EventHandler(this.btWriteIcaoCSV_Click);
      // 
      // btWriteAirportCsv
      // 
      this.btWriteAirportCsv.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteAirportCsv.Location = new System.Drawing.Point(412, 96);
      this.btWriteAirportCsv.Name = "btWriteAirportCsv";
      this.btWriteAirportCsv.Size = new System.Drawing.Size(124, 36);
      this.btWriteAirportCsv.TabIndex = 14;
      this.btWriteAirportCsv.Text = "Write Airport CSV";
      this.btWriteAirportCsv.UseVisualStyleBackColor = true;
      this.btWriteAirportCsv.Click += new System.EventHandler(this.btWriteAirportCsv_Click);
      // 
      // btLoadAirportCsv
      // 
      this.btLoadAirportCsv.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btLoadAirportCsv.Location = new System.Drawing.Point(12, 96);
      this.btLoadAirportCsv.Name = "btLoadAirportCsv";
      this.btLoadAirportCsv.Size = new System.Drawing.Size(124, 36);
      this.btLoadAirportCsv.TabIndex = 12;
      this.btLoadAirportCsv.Text = "Load Airport CSV...";
      this.btLoadAirportCsv.UseVisualStyleBackColor = true;
      this.btLoadAirportCsv.Click += new System.EventHandler(this.btLoadAirportCsv_Click);
      // 
      // btWriteIcaoJsonCSV
      // 
      this.btWriteIcaoJsonCSV.Location = new System.Drawing.Point(142, 12);
      this.btWriteIcaoJsonCSV.Name = "btWriteIcaoJsonCSV";
      this.btWriteIcaoJsonCSV.Size = new System.Drawing.Size(124, 36);
      this.btWriteIcaoJsonCSV.TabIndex = 15;
      this.btWriteIcaoJsonCSV.Text = "Write FA Json CSV";
      this.btWriteIcaoJsonCSV.UseVisualStyleBackColor = true;
      this.btWriteIcaoJsonCSV.Click += new System.EventHandler(this.btWriteIcaoJsonCSV_Click);
      // 
      // btWriteNavGJson
      // 
      this.btWriteNavGJson.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteNavGJson.Location = new System.Drawing.Point(695, 180);
      this.btWriteNavGJson.Name = "btWriteNavGJson";
      this.btWriteNavGJson.Size = new System.Drawing.Size(124, 36);
      this.btWriteNavGJson.TabIndex = 17;
      this.btWriteNavGJson.Text = "Write Navaid GeoJson";
      this.btWriteNavGJson.UseVisualStyleBackColor = true;
      this.btWriteNavGJson.Click += new System.EventHandler(this.btWriteNavGJson_Click);
      // 
      // btLoadNavCSV
      // 
      this.btLoadNavCSV.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btLoadNavCSV.Location = new System.Drawing.Point(12, 180);
      this.btLoadNavCSV.Name = "btLoadNavCSV";
      this.btLoadNavCSV.Size = new System.Drawing.Size(124, 36);
      this.btLoadNavCSV.TabIndex = 16;
      this.btLoadNavCSV.Text = "Load Navaid CSV...";
      this.btLoadNavCSV.UseVisualStyleBackColor = true;
      this.btLoadNavCSV.Click += new System.EventHandler(this.btLoadNavCSV_Click);
      // 
      // btWriteAirportGJson
      // 
      this.btWriteAirportGJson.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteAirportGJson.Location = new System.Drawing.Point(695, 96);
      this.btWriteAirportGJson.Name = "btWriteAirportGJson";
      this.btWriteAirportGJson.Size = new System.Drawing.Size(124, 36);
      this.btWriteAirportGJson.TabIndex = 18;
      this.btWriteAirportGJson.Text = "Write Airport GeoJson";
      this.btWriteAirportGJson.UseVisualStyleBackColor = true;
      this.btWriteAirportGJson.Click += new System.EventHandler(this.btWriteAirportGJson_Click);
      // 
      // chkRangeLimit
      // 
      this.chkRangeLimit.AutoSize = true;
      this.chkRangeLimit.Checked = true;
      this.chkRangeLimit.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkRangeLimit.Location = new System.Drawing.Point(672, 325);
      this.chkRangeLimit.Name = "chkRangeLimit";
      this.chkRangeLimit.Size = new System.Drawing.Size(101, 17);
      this.chkRangeLimit.TabIndex = 19;
      this.chkRangeLimit.Text = "Limit by R (Nm)";
      this.chkRangeLimit.UseVisualStyleBackColor = true;
      // 
      // txRangeLimit
      // 
      this.txRangeLimit.Location = new System.Drawing.Point(673, 348);
      this.txRangeLimit.Name = "txRangeLimit";
      this.txRangeLimit.Size = new System.Drawing.Size(79, 22);
      this.txRangeLimit.TabIndex = 20;
      this.txRangeLimit.Text = "300";
      // 
      // txMyLat
      // 
      this.txMyLat.Location = new System.Drawing.Point(494, 348);
      this.txMyLat.Name = "txMyLat";
      this.txMyLat.Size = new System.Drawing.Size(79, 22);
      this.txMyLat.TabIndex = 20;
      this.txMyLat.Text = "47.29095";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(404, 351);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(69, 13);
      this.label1.TabIndex = 21;
      this.label1.Text = "My Lat / Lon";
      // 
      // txMyLon
      // 
      this.txMyLon.Location = new System.Drawing.Point(579, 348);
      this.txMyLon.Name = "txMyLon";
      this.txMyLon.Size = new System.Drawing.Size(79, 22);
      this.txMyLon.TabIndex = 20;
      this.txMyLon.Text = "8.26167";
      // 
      // btXP11nav
      // 
      this.btXP11nav.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btXP11nav.Location = new System.Drawing.Point(12, 222);
      this.btXP11nav.Name = "btXP11nav";
      this.btXP11nav.Size = new System.Drawing.Size(124, 36);
      this.btXP11nav.TabIndex = 22;
      this.btXP11nav.Text = "Load Navaid XP11...";
      this.btXP11nav.UseVisualStyleBackColor = true;
      this.btXP11nav.Click += new System.EventHandler(this.btXP11nav_Click);
      // 
      // btXP11fix
      // 
      this.btXP11fix.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btXP11fix.Location = new System.Drawing.Point(142, 222);
      this.btXP11fix.Name = "btXP11fix";
      this.btXP11fix.Size = new System.Drawing.Size(124, 36);
      this.btXP11fix.TabIndex = 23;
      this.btXP11fix.Text = "Load Fixes XP11...";
      this.btXP11fix.UseVisualStyleBackColor = true;
      this.btXP11fix.Click += new System.EventHandler(this.btXP11fix_Click);
      // 
      // btXP11awy
      // 
      this.btXP11awy.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btXP11awy.Location = new System.Drawing.Point(272, 222);
      this.btXP11awy.Name = "btXP11awy";
      this.btXP11awy.Size = new System.Drawing.Size(124, 36);
      this.btXP11awy.TabIndex = 24;
      this.btXP11awy.Text = "Load Airways XP11...";
      this.btXP11awy.UseVisualStyleBackColor = true;
      this.btXP11awy.Click += new System.EventHandler(this.btXP11awy_Click);
      // 
      // btWriteAwyGJson
      // 
      this.btWriteAwyGJson.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteAwyGJson.Location = new System.Drawing.Point(695, 222);
      this.btWriteAwyGJson.Name = "btWriteAwyGJson";
      this.btWriteAwyGJson.Size = new System.Drawing.Size(124, 36);
      this.btWriteAwyGJson.TabIndex = 25;
      this.btWriteAwyGJson.Text = "Write Airways GeoJson";
      this.btWriteAwyGJson.UseVisualStyleBackColor = true;
      this.btWriteAwyGJson.Click += new System.EventHandler(this.btWriteAwyGJson_Click);
      // 
      // btWriteIcaoSQDB
      // 
      this.btWriteIcaoSQDB.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteIcaoSQDB.Location = new System.Drawing.Point(565, 12);
      this.btWriteIcaoSQDB.Name = "btWriteIcaoSQDB";
      this.btWriteIcaoSQDB.Size = new System.Drawing.Size(124, 36);
      this.btWriteIcaoSQDB.TabIndex = 26;
      this.btWriteIcaoSQDB.Text = "Write FA SQ DB";
      this.btWriteIcaoSQDB.UseVisualStyleBackColor = true;
      this.btWriteIcaoSQDB.Click += new System.EventHandler(this.btWriteIcaoSQDB_Click);
      // 
      // btWriteFlightsSQDB
      // 
      this.btWriteFlightsSQDB.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteFlightsSQDB.Location = new System.Drawing.Point(565, 138);
      this.btWriteFlightsSQDB.Name = "btWriteFlightsSQDB";
      this.btWriteFlightsSQDB.Size = new System.Drawing.Size(124, 36);
      this.btWriteFlightsSQDB.TabIndex = 27;
      this.btWriteFlightsSQDB.Text = "Write Flights SQ DB";
      this.btWriteFlightsSQDB.UseVisualStyleBackColor = true;
      this.btWriteFlightsSQDB.Click += new System.EventHandler(this.btWriteFlightsSQDB_Click);
      // 
      // btLoadIcaoAct
      // 
      this.btLoadIcaoAct.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btLoadIcaoAct.Location = new System.Drawing.Point(12, 264);
      this.btLoadIcaoAct.Name = "btLoadIcaoAct";
      this.btLoadIcaoAct.Size = new System.Drawing.Size(124, 36);
      this.btLoadIcaoAct.TabIndex = 28;
      this.btLoadIcaoAct.Text = "Load ICAO ACT Json";
      this.btLoadIcaoAct.UseVisualStyleBackColor = true;
      this.btLoadIcaoAct.Click += new System.EventHandler(this.btLoadIcaoAct_Click);
      // 
      // btWriteIcaoAct
      // 
      this.btWriteIcaoAct.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btWriteIcaoAct.Location = new System.Drawing.Point(565, 264);
      this.btWriteIcaoAct.Name = "btWriteIcaoAct";
      this.btWriteIcaoAct.Size = new System.Drawing.Size(124, 36);
      this.btWriteIcaoAct.TabIndex = 29;
      this.btWriteIcaoAct.Text = "Write ICAO ACT Json";
      this.btWriteIcaoAct.UseVisualStyleBackColor = true;
      this.btWriteIcaoAct.Click += new System.EventHandler(this.btWriteIcaoAct_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(826, 667);
      this.Controls.Add(this.btWriteIcaoAct);
      this.Controls.Add(this.btLoadIcaoAct);
      this.Controls.Add(this.btWriteFlightsSQDB);
      this.Controls.Add(this.btWriteIcaoSQDB);
      this.Controls.Add(this.btWriteAwyGJson);
      this.Controls.Add(this.btXP11awy);
      this.Controls.Add(this.btXP11fix);
      this.Controls.Add(this.btXP11nav);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.txMyLon);
      this.Controls.Add(this.txMyLat);
      this.Controls.Add(this.txRangeLimit);
      this.Controls.Add(this.chkRangeLimit);
      this.Controls.Add(this.btWriteAirportGJson);
      this.Controls.Add(this.btWriteNavGJson);
      this.Controls.Add(this.btLoadNavCSV);
      this.Controls.Add(this.btWriteIcaoJsonCSV);
      this.Controls.Add(this.btWriteAirportCsv);
      this.Controls.Add(this.btLoadAirportCsv);
      this.Controls.Add(this.btWriteRtCSV);
      this.Controls.Add(this.btWriteAcCSV);
      this.Controls.Add(this.btWriteIcaoCSV);
      this.Controls.Add(this.btWriteRouteDb);
      this.Controls.Add(this.btLoadRouteTsv);
      this.Controls.Add(this.btWriteAircraftDB);
      this.Controls.Add(this.btLoadAircraftCsv);
      this.Controls.Add(this.btLoadBS);
      this.Controls.Add(this.btWriteDb);
      this.Controls.Add(this.RTB);
      this.Controls.Add(this.btLoadTsv);
      this.Controls.Add(this.btLoad);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Name = "Form1";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btLoad;
    private System.Windows.Forms.OpenFileDialog OFD;
    private System.Windows.Forms.Button btLoadTsv;
    private System.Windows.Forms.RichTextBox RTB;
    private System.Windows.Forms.Button btWriteDb;
    private System.Windows.Forms.Button btLoadBS;
    private System.Windows.Forms.Button btLoadAircraftCsv;
    private System.Windows.Forms.Button btWriteAircraftDB;
    private System.Windows.Forms.Button btWriteRouteDb;
    private System.Windows.Forms.Button btLoadRouteTsv;
    private System.Windows.Forms.Button btWriteRtCSV;
    private System.Windows.Forms.Button btWriteAcCSV;
    private System.Windows.Forms.Button btWriteIcaoCSV;
    private System.Windows.Forms.Button btWriteAirportCsv;
    private System.Windows.Forms.Button btLoadAirportCsv;
    private System.Windows.Forms.Button btWriteIcaoJsonCSV;
    private System.Windows.Forms.Button btWriteNavGJson;
    private System.Windows.Forms.Button btLoadNavCSV;
    private System.Windows.Forms.Button btWriteAirportGJson;
    private System.Windows.Forms.CheckBox chkRangeLimit;
    private System.Windows.Forms.TextBox txRangeLimit;
    private System.Windows.Forms.TextBox txMyLat;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txMyLon;
    private System.Windows.Forms.Button btXP11nav;
    private System.Windows.Forms.Button btXP11fix;
    private System.Windows.Forms.Button btXP11awy;
    private System.Windows.Forms.Button btWriteAwyGJson;
    private System.Windows.Forms.Button btWriteIcaoSQDB;
    private System.Windows.Forms.Button btWriteFlightsSQDB;
    private System.Windows.Forms.Button btLoadIcaoAct;
    private System.Windows.Forms.Button btWriteIcaoAct;
  }
}

