﻿using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
   public class Document: ValidatableBindableBase
    {
        private string caseNumber;

        public string CaseNumber
        {
            get { return caseNumber; }
            set { SetProperty(ref caseNumber, value); }
        }

        private string documentType;

        public string DocumentType
        {
            get { return documentType; }
            set { SetProperty(ref documentType, value); }
        }

        private string serialNumber;

        public string SerialNumber
        {
            get { return serialNumber; }
            set { SetProperty(ref serialNumber, value); }
        }

        private string make;

        public string Make
        {
            get { return make; }
            set { SetProperty(ref make, value); }
        }

        private string model;

        public string Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        private string registrationNumber;

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { SetProperty(ref registrationNumber, value); }
        }

    }
}