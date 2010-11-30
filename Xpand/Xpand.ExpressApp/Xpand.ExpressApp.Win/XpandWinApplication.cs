﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Core.ModelEditor;
using DevExpress.Persistent.Base;
using Xpand.ExpressApp.Core;
using Xpand.ExpressApp.Win.ViewStrategies;

namespace Xpand.ExpressApp.Win {

    public partial class XpandWinApplication : WinApplication,  ISupportModelsManager, ISupportCustomListEditorCreation, IWinApplication, ISupportConfirmationRequired, ISupportAfterViewShown {
        public event EventHandler<ViewShownEventArgs> AfterViewShown;
        public event EventHandler<CreatingListEditorEventArgs> CustomCreateListEditor;
        public event CancelEventHandler ConfirmationRequired;

        protected void OnConfirmationRequired(CancelEventArgs e) {
            CancelEventHandler handler = ConfirmationRequired;
            if (handler != null) handler(this, e);
        }

        public virtual void OnAfterViewShown(Frame frame, Frame sourceFrame) {
            if (AfterViewShown != null) {
                AfterViewShown(this, new ViewShownEventArgs(frame, sourceFrame));
            }
        }

        public void OnCustomCreateListEditor(CreatingListEditorEventArgs e) {
            EventHandler<CreatingListEditorEventArgs> handler = CustomCreateListEditor;
            if (handler != null) handler(this, e);
        }

        protected override void OnCustomProcessShortcut(CustomProcessShortcutEventArgs args) {
            base.OnCustomProcessShortcut(args);
            new ViewShortCutProccesor(this).Proccess(args);

        }
        public override ConfirmationResult AskConfirmation(ConfirmationType confirmationType) {
            var cancelEventArgs = new CancelEventArgs();
            OnConfirmationRequired(cancelEventArgs);
            return cancelEventArgs.Cancel ? ConfirmationResult.No : base.AskConfirmation(confirmationType);
        }
        protected override ShowViewStrategyBase CreateShowViewStrategy() {
            var showViewStrategyBase = base.CreateShowViewStrategy();
            return showViewStrategyBase is ShowInMultipleWindowsStrategy
                       ? new XpandShowInMultipleWindowsStrategy(this)
                       : showViewStrategyBase;
        }

        void OnListViewCreating(object sender, ListViewCreatingEventArgs args) {
            args.View = ViewFactory.CreateListView(this, args.ViewID, args.CollectionSource, args.IsRoot);
        }
        protected override Window CreateWindowCore(TemplateContext context, ICollection<Controller> controllers, bool isMain, bool activateControllersImmediatelly) {
            Tracing.Tracer.LogVerboseValue("WinApplication.CreateWindowCore.activateControllersImmediatelly", activateControllersImmediatelly);
            return new XpandWinWindow(this, context, controllers, isMain, activateControllersImmediatelly);
        }
        protected override Window CreatePopupWindowCore(TemplateContext context, ICollection<Controller> controllers) {
            return new XpandWinWindow(this, context, controllers, false, true);
        }

        void OnDetailViewCreating(object sender, DetailViewCreatingEventArgs args) {
            args.View = ViewFactory.CreateDetailView(this, args.ViewID, args.Obj, args.ObjectSpace, args.IsRoot);
        }

        public ApplicationModelsManager ModelsManager {
            get { return modelsManager; }
        }

        public override IModelTemplate GetTemplateCustomizationModel(IFrameTemplate template) {
            var list = new List<ModelApplicationBase>();
            while (((ModelApplicationBase)Model).LastLayer.Id != "UserDiff" && ((ModelApplicationBase)Model).LastLayer.Id != AfterSetupLayerId) {
                var modelApplicationBase = ((ModelApplicationBase)Model).LastLayer;
                list.Add(modelApplicationBase);
                ((ModelApplicationBase)Model).RemoveLayer(modelApplicationBase);
            }
            var modelTemplate = base.GetTemplateCustomizationModel(template);
            foreach (var modelApplicationBase in list) {
                ((ModelApplicationBase)Model).AddLayer(modelApplicationBase);
            }
            return modelTemplate;
        }

        protected override ListEditor CreateListEditorCore(IModelListView modelListView, CollectionSourceBase collectionSource) {
            var creatingListEditorEventArgs = new CreatingListEditorEventArgs(modelListView, collectionSource);
            OnCustomCreateListEditor(creatingListEditorEventArgs);
            return creatingListEditorEventArgs.Handled ? creatingListEditorEventArgs.ListEditor : base.CreateListEditorCore(modelListView, collectionSource);
        }







        public XpandWinApplication() {
            if (XpandModuleBase.Application == null)
                Application.ThreadException += (sender, args) => HandleException(args.Exception, this);
            else {
                Application.ThreadException += (sender, args) => HandleException(args.Exception, (XpandWinApplication)XpandModuleBase.Application);
            }
            InitializeComponent();
            DetailViewCreating += OnDetailViewCreating;
            ListViewCreating += OnListViewCreating;
            ListViewCreated += OnListViewCreated;
        }

        void OnListViewCreated(object sender, ListViewCreatedEventArgs listViewCreatedEventArgs) {

        }

        public static void HandleException(Exception exception, XpandWinApplication xpandWinApplication) {
            xpandWinApplication.HandleException(exception);
        }

        public XpandWinApplication(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }

        protected override void OnCreateCustomObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            this.CreateCustomObjectSpaceprovider(args);
            base.OnCreateCustomObjectSpaceProvider(args);
        }

    }


    public class ModelEditFormShowningEventArgs : HandledEventArgs {
        public ModelEditFormShowningEventArgs(ModelEditorForm modelEditorForm) {
            ModelEditorForm = modelEditorForm;
        }

        public ModelEditorForm ModelEditorForm { get; set; }
    }
}