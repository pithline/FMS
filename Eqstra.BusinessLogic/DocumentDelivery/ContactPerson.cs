using Eqstra.BusinessLogic.Base;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.BusinessLogic.DocumentDelivery
{
    public class ContactPerson : ValidatableBindableBase
    {
        private string caseNumber;
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
        }

        private long caseCategoryRecID;

        public long CaseCategoryRecID
        {
            get { return caseCategoryRecID; }
            set { SetProperty(ref caseCategoryRecID, value); }
        }


        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }
        private string surname;
        public string Surname
        {
            get { return surname; }
            set { SetProperty(ref surname, value); }
        }


        private string fullName;
        [Ignore]
        public string FullName
        {
            get { return (this.FirstName + String.Empty.PadLeft(2)+ this.Surname); }
            set { SetProperty(ref fullName, value); }
        }

        private string cellPhone;

        public string CellPhone
        {
            get { return cellPhone; }
            set { SetProperty(ref cellPhone, value); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string position;

        public string Position
        {
            get { return position; }


            set { SetProperty(ref position, value); }
        }

    }

    public class ContactPersonEvent : PubSubEvent<ContactPerson>
    {
    }
}
