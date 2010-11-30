using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace Xpand.ExpressApp.MemberLevelSecurity {
    public class Updater : ModuleUpdater {
        public Updater(ObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion) {
        }

        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
