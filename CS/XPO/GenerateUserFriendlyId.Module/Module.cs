using DevExpress.ExpressApp;
using GenerateUserFriendlyId.Module.BusinessObjects;
using GenerateUserFriendlyId.Module;

namespace GenerateUserFriendlyId.Module {
	public sealed partial class GenerateUserFriendlyIdModule : ModuleBase {
		public GenerateUserFriendlyIdModule() {
			InitializeComponent();
		}
		protected override System.Collections.Generic.IEnumerable<System.Type> GetDeclaredExportedTypes() {
			return new System.Type[] { typeof(Sequence) };
		}
	}
}