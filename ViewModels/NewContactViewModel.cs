using ContactsMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using ContactsMVVM.Views;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Controls;

namespace ContactsMVVM.ViewModels
{
    class NewContactViewModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
        private string _fullName;
        private string _phoneText;
        private string _emailText;
        private string _linkText;
        private bool _nameErrorVisibility;
        private EmailType _selectedEmailType;
        private PhoneType _selectedPhoneType;
        public bool MyDialogResult { get; set; } = false;
        public ObservableCollection<PhoneType> PhoneTypes { get; set; } = new(Enum.GetValues(typeof(PhoneType)).Cast<PhoneType>());
        public ObservableCollection<EmailType> EmailTypes { get; set; } = new(Enum.GetValues(typeof(EmailType)).Cast<EmailType>());
        public string FullName { get { return _fullName; } set { if (_fullName != value) 
                { _fullName = value; OnPropertyChanged(nameof(FullName));
                    if (_fullName != "") { NameErrorVisibility = false; }
                } } }
        public ObservableCollection<Tuple<string, PhoneType>> Phones { get; set; } = new();
        public ObservableCollection<Tuple<string, EmailType>> Emails { get; set; } = new();
        public ObservableCollection<Hyperlink> Links { get; set; } = new();
        public DateTime? _birthday;
        public DateTime? Birthday { get { return _birthday; } set { if (_birthday != value) { _birthday = value; OnPropertyChanged(nameof(Birthday)); } } }
        public EmailType SelectedEmailType 
        { 
            get { return _selectedEmailType; }
            set { if (_selectedEmailType != value) { _selectedEmailType = value; OnPropertyChanged(nameof(SelectedEmailType)); } }
        }
        public PhoneType SelectedPhoneType 
        { 
            get { return _selectedPhoneType; }
            set { if (_selectedPhoneType != value) { _selectedPhoneType = value; OnPropertyChanged(nameof(SelectedPhoneType)); } }
        }
        public bool NameErrorVisibility 
        { 
            get { return _nameErrorVisibility; }
            set { if (_nameErrorVisibility != value) { _nameErrorVisibility = value; OnPropertyChanged(nameof(NameErrorVisibility)); } }
        }
        public string PhoneText 
        { 
            get { return _phoneText; }
            set { if (_phoneText != value) { _phoneText = value; OnPropertyChanged(PhoneText); } }
        }
        public string EmailText 
        { 
            get { return _emailText; }
            set { if (_emailText != value) { _emailText = value; OnPropertyChanged(EmailText); } }
        }
        public string LinkText 
        { 
            get { return _linkText; }
            set { if (_linkText != value) { _linkText = value; OnPropertyChanged(LinkText); } }
        }

        public DelegateCommand SubmitCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand AddEmailCommand { get; set; }
        public DelegateCommand AddPhoneCommand { get; set; }
        public DelegateCommand AddLinkCommand { get; set; }
        public DelegateCommand RemoveItemCommand { get; set; }


        public NewContactViewModel()
        {
            SubmitCommand = new(btnSubmit_Click, (x) => true);
            CancelCommand = new(CancelForm, (x) => true);
            AddEmailCommand = new(AddEmailToList, (x) => true);
            AddPhoneCommand = new(AddPhoneToList, (x) => true);
            AddLinkCommand = new(AddLinkToList, (x) => true);
            RemoveItemCommand = new(RemoveItem, (x) => true);
            PhoneTypes.CollectionChanged += CollectionChanged;
            EmailTypes.CollectionChanged += CollectionChanged;
            Phones.CollectionChanged += CollectionChanged;
            Emails.CollectionChanged += CollectionChanged;
            Links.CollectionChanged += CollectionChanged;
            SelectedEmailType = EmailType.Personal;
            SelectedPhoneType = PhoneType.Home;
            _fullName = "";
            _nameErrorVisibility = false;
            _phoneText = "";
            _emailText = "";
            _linkText = "";
        }
        public NewContactViewModel(Contact contact)
        {
            SubmitCommand = new(btnSubmit_Click, (x) => true);
            CancelCommand = new(CancelForm, (x) => true);
            AddEmailCommand = new(AddEmailToList, (x) => true);
            AddPhoneCommand = new(AddPhoneToList, (x) => true);
            AddLinkCommand = new(AddLinkToList, (x) => true);
            RemoveItemCommand = new(RemoveItem, (x) => true);
            PhoneTypes.CollectionChanged += CollectionChanged;
            EmailTypes.CollectionChanged += CollectionChanged;
            Phones.CollectionChanged += CollectionChanged;
            Emails.CollectionChanged += CollectionChanged;
            Links.CollectionChanged += CollectionChanged;
            SelectedEmailType = EmailType.Personal;
            SelectedPhoneType = PhoneType.Home;
            _nameErrorVisibility = false;
            _phoneText = "";
            _emailText = "";
            _linkText = "";
            _fullName = contact.Name.FullName;
            Birthday = new(contact.Birthday, new());
            List<Tuple<string, EmailType>> myEmails = new();
            List<Tuple<string, PhoneType>> myPhones = new();
            List<Hyperlink> myLinks = new();
            foreach (Email email in contact.EmailAddresses) { myEmails.Add(new(email.ToString(), email.Type)); }
            foreach (PhoneNumber number in contact.PhoneNumbers) { myPhones.Add(new(number.ToString(), number.Type)); }
            foreach (Hyperlink link in contact.Links) { myLinks.Add(link); }
            Emails = new(myEmails);
            Phones = new(myPhones);
            Links = new(myLinks);
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void btnSubmit_Click(object? param)
        {
            if (FullName.Trim() != string.Empty)
            {
                MyDialogResult = true;
                if (param is ContactsMVVM.Views.NewContactForm)
                {
                    ((NewContactForm)param).Close();
                }
                //.Close();
            }
            else { NameErrorVisibility = true; }

        }
        public void CancelForm(object? param)
        {
            MyDialogResult = false;
            if (param is ContactsMVVM.Views.NewContactForm)
            {
                ((NewContactForm)param).Close();
            }
            //.Close();
        }

        public void AddEmailToList(object? param)
        {
            if (!Emails.Contains(new(EmailText, SelectedEmailType)))
            {
                Emails.Add(new(EmailText, SelectedEmailType));
            }
            EmailText = "";
            SelectedEmailType = EmailTypes[0];
            //txtEmail.Focus();
            if (param is TextBox) { ((TextBox)param).Focus(); }
        }
        public void AddPhoneToList(object? param)
        {
            if (!Phones.Contains(new(PhoneText, SelectedPhoneType)))
            {
                Phones.Add(new(PhoneText, SelectedPhoneType));
            }
            PhoneText = "";
            SelectedPhoneType = PhoneTypes[0];
            //txtPhone.Focus();
            if (param is TextBox) { ((TextBox)param).Focus(); }
        }
        public void AddLinkToList(object? param)
        {

            string linktext = LinkText;
            if (!linktext.Contains("https://")) { linktext = linktext.Insert(0, "https://"); }
            Run runme = new Run(linktext);
            Hyperlink link = new Hyperlink(runme) { NavigateUri = new Uri(linktext) };
            Links.Add(link);
            LinkText = "";
            //txtLinks.Focus();
            if (param is TextBox) { ((TextBox)param).Focus(); }
        }
        public void RemoveItem(object? param)
        {
            if (param is Tuple<string, EmailType>)
            {
                Emails.Remove((Tuple<string, EmailType>)param);
            }
            else if (param is Tuple<string, PhoneType>)
            {
                Phones.Remove((Tuple<string, PhoneType>)param);
            }
            else if (param is Hyperlink)
            {
                Links.Remove((Hyperlink)param);
            }
        }
    }
}
