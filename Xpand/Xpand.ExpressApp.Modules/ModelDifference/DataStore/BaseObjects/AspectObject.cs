﻿using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

using Xpand.Xpo;

namespace Xpand.ExpressApp.ModelDifference.DataStore.BaseObjects {
    [RuleCombinationOfPropertiesIsUnique("AspectObject_Name_MDO",DefaultContexts.Save,"Name,ModelDifferenceObject" )]
    public class AspectObject:XpandCustomObject {
        public AspectObject(Session session) : base(session) {
        }
        private string _name;

        
        [RuleRequiredField]
        public string Name {
            get { return _name; }
            set { SetPropertyValue("Name", ref _name, value); }
        }
        private ModelDifferenceObject _modelDifferenceObject;

        [Association("ModelDifferenceObject-AspectObjects")]
        public ModelDifferenceObject ModelDifferenceObject {
            get { return _modelDifferenceObject; }
            set { SetPropertyValue("ModelDifferenceObject", ref _modelDifferenceObject, value); }
        }
        private string _xml;
        [Size(SizeAttribute.Unlimited)]
        public string Xml {
            get { return _xml; }
            set { SetPropertyValue("Xml", ref _xml, value); }
        }
    }
}