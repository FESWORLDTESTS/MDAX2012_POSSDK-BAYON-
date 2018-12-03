/*
SAMPLE CODE NOTICE

THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED, 
OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.  
THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.  
NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
*/

using LSRetailPosis;
using System;
using System.Data;

namespace Microsoft.Dynamics.Retail.Pos.SalesOrder.WinFormsTouch.OrderDetailsPages
{
    sealed partial class CustomerInformationPage : OrderDetailsPage
    {
        public CustomerInformationPage()
        {
            InitializeComponent();
            viewAddressUserControl1.SetEditable(false);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            if (!this.DesignMode)
            {
                TranslateLabels();
            }
            base.OnLoad(e);

           
        }

        private void TranslateLabels()
        {
            this.Text                = ApplicationLocalizer.Language.Translate(56329); // "Customer information"
            labelContact.Text        = ApplicationLocalizer.Language.Translate(56308); // "CONTACT INFORMATION"
            labelName.Text           = ApplicationLocalizer.Language.Translate(56309); // "Name:"
            labelPhoneContact.Text   = ApplicationLocalizer.Language.Translate(56314); // "Phone number:"
            labelEmailId.Text        = ApplicationLocalizer.Language.Translate(51186); // "Email:"
            btnAdd.Text              = ApplicationLocalizer.Language.Translate(56316); // "Add"
            btnSearch.Text           = ApplicationLocalizer.Language.Translate(56317); // "Search"
        }

        protected override void OnViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Transaction":
                    bindingSource.ResetBindings(false);
                     break;
                default:
                    break;
            }
            if (ViewModel != null && ViewModel.Transaction != null && ViewModel.Transaction.Customer != null)
            {
                viewAddressUserControl1.SetProperties(ViewModel.Transaction.Customer, ViewModel.Transaction.Customer.PrimaryAddress);

            }
        }

        public override void SetViewModel<T>(T viewModel)
        {
            base.SetViewModel<T>(viewModel);
            if (ViewModel != null && ViewModel.Transaction != null && ViewModel.Transaction.Customer != null)
            {

                viewAddressUserControl1.SetProperties(ViewModel.Transaction.Customer, ViewModel.Transaction.Customer.PrimaryAddress);
                DAC odac = new DAC(SalesOrder.InternalApplication.Settings.Database.Connection);
                DataTable dt = odac.GetContactData(this.ViewModel.Transaction.TransactionId, this.ViewModel.Transaction.TerminalId, this.ViewModel.Transaction.StoreId);
                if (dt.Rows.Count > 0)
                {
                    textBoxEmailId.Text = dt.Rows[0]["EMAIL"].ToString();
                    textBoxName.Text = dt.Rows[0]["NAME"].ToString();
                    textBoxPhoneContact.Text = dt.Rows[0]["PHONE"].ToString();
                    
                }
            }
        }

        private CustomerInformationViewModel ViewModel
        {
            get { return GetViewModel<CustomerInformationViewModel>(); }
        }

        public override void OnClear()
        {
            if (this.ViewModel != null)
                this.ViewModel.ExecuteClearCustomerCommand();
        }

        private void OnBtnSearch_Click(object sender, System.EventArgs e)
        {
            if (this.ViewModel != null)
                this.ViewModel.ExecuteCustomerSearchCommand();
        }

        private void OnBtnAdd_Click(object sender, System.EventArgs e)
        {
            if (this.ViewModel != null)
                this.ViewModel.ExecuteCustomerAddCommand();
        }

        private void textBoxName_EditValueChanged(object sender, System.EventArgs e)
        {
            if (this.ViewModel != null)
                this.ViewModel.Transaction.Customer.Name = textBoxName.Text.Trim();
        }

        private void textBoxEmailId_EditValueChanged(object sender, System.EventArgs e)
        {
            if (this.ViewModel != null)
                this.ViewModel.Transaction.Customer.Email = textBoxEmailId.Text.Trim();
        }

        private void textBoxPhoneContact_EditValueChanged(object sender, System.EventArgs e)
        {
            if (this.ViewModel != null)
                this.ViewModel.Transaction.Customer.Telephone  = textBoxPhoneContact.Text.Trim();
        }
    }
}