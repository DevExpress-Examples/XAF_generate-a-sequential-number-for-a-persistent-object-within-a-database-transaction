namespace GenerateUserFriendlyId.Win {
    partial class GenerateUserFriendlyIdWindowsFormsApplication {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
			this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
			this.module3 = new GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule();
			this.businessClassLibraryCustomizationModule1 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
			this.demoModule1 = new Demo.Module.DemoModule();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// GenerateUserFriendlyIdWindowsFormsApplication
			// 
			this.ApplicationName = "GenerateUserFriendlyId";
			this.Modules.Add(this.module1);
			this.Modules.Add(this.module2);
			this.Modules.Add(this.businessClassLibraryCustomizationModule1);
			this.Modules.Add(this.module3);
			this.Modules.Add(this.demoModule1);
			this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.GenerateUserFriendlyIdWindowsFormsApplication_DatabaseVersionMismatch);
			this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.GenerateUserFriendlyIdWindowsFormsApplication_CustomizeLanguagesList);
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
		private GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule module3;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule businessClassLibraryCustomizationModule1;
        private Demo.Module.DemoModule demoModule1;
    }
}
