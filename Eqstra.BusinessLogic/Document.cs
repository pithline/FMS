﻿using Eqstra.BusinessLogic.Base;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic
{
    public class Document : ValidatableBindableBase
    {
        private long vehicleInsRecID;
        [PrimaryKey]
        public long VehicleInsRecID
        {
            get { return vehicleInsRecID; }
            set { SetProperty(ref vehicleInsRecID, value); }
        }
        private string caseNumber;
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
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

        private long caseCategoryRecID;

        public long CaseCategoryRecID
        {
            get { return caseCategoryRecID; }
            set { SetProperty(ref caseCategoryRecID, value); }
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

        private string docName;

        public string DocName
        {
            get { return docName; }
            set { SetProperty(ref docName, value); }
        }

        private bool isDelivered;
        public bool IsDelivered
        {
            get { return isDelivered; }
            set { SetProperty(ref isDelivered, value); }
        }

        private bool isCollected;
        public bool IsCollected
        {
            get { return isCollected; }
            set { SetProperty(ref isCollected, value); }
        }

        private bool shouldSave;

        public bool ShouldSave
        {
            get { return shouldSave; }
            set { SetProperty(ref shouldSave, value); }
        }

    }
}
