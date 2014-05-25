using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.ObjectModel;

namespace Eqstra.BusinessLogic.Base
{
    abstract public class VIBase : ValidatableBindableBase
    {
        private string caseNumber;
        [SQLite.Column("CaseNumber"), PrimaryKey]
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
        }

        private ObservableCollection<ValidationError> _errors = new ObservableCollection<ValidationError>();


        public ObservableCollection<ValidationError> Errors
        {
            get { return _errors; }
            private set { SetProperty(ref _errors, value); }
        }


        public abstract System.Threading.Tasks.Task<VIBase> GetDataAsync(string caseNumber);


        public bool ValidateModel()
        {
            _errors.Clear();
            var propertiesToValidate = this.GetType().GetRuntimeProperties().Where(o => o.GetCustomAttributes(typeof(DamageSnapshotRequiredAttribute)).Any());
            foreach (var propInfo in propertiesToValidate)
            {
                var propValue = propInfo.GetValue(this);
                var att = propInfo.GetCustomAttribute<DamageSnapshotRequiredAttribute>();
                var associatedPropInfo = this.GetType().GetRuntimeProperty(att.AssociatedPropertyName);
                var associatedPropValue = associatedPropInfo.GetValue(this);
                if ((bool)associatedPropValue)
                {
                    var imgList = propValue as ObservableCollection<ImageCapture>;
                    if (imgList != null)
                    {
                        if (imgList.Count == 0)
                        {
                            if (!_errors.Any(x=>x.PropertyName ==propInfo.Name))
                            {
                                _errors.Add(new ValidationError { PropertyName = propInfo.Name, ErrorMessage = att.ErrorMessage }); 
                            }
                        }
                    }
                }
            }
            return _errors.Count == 0;
        }
    }
}
