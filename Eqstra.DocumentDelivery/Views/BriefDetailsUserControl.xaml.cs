using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Eqstra.DocumentDelivery.Views
{
    public sealed partial class BriefDetailsUserControl : UserControl
    {
        public List<DocBrief> DocumentList { get; set; }
        
        public BriefDetailsUserControl()
        {
            this.InitializeComponent();
            this.DocumentList = new List<DocBrief>
            {
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
                new DocBrief{CaseNumber="E4323",DocumentType = "LicenseDisc"},
            };
        }
    }


    public class DocBrief
    {
        public string CaseNumber { get; set; }
        public string DocumentType { get; set; }
    }
}
