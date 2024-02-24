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
    class NewContactViewModel : ObservableObject, INotifyCollectionChanged
    {
        private string _fullName;
        private string _phoneText;
        private string _emailText;
        private string _linkText;
        private string _addressText;
        private bool _nameErrorVisibility;
        private EmailType _selectedEmailType;
        private PhoneType _selectedPhoneType;
        private AddressType _selectedAddressType;
        private EyeColor _selectedEyeColor;
        private HairColor _selectedHairColor;
        private Complexion _selectedComplexion;
        private Gender _selectedGender;
        private Ethnicity _selectedEthnicity;
        private Height _height;
        private Weight _weight;

        public bool MyDialogResult { get; set; } = false;
        public ObservableCollection<PhoneType> PhoneTypes { get; set; } = new(Enum.GetValues(typeof(PhoneType)).Cast<PhoneType>());
        public ObservableCollection<EmailType> EmailTypes { get; set; } = new(Enum.GetValues(typeof(EmailType)).Cast<EmailType>());
        public ObservableCollection<EyeColor> EyeColors { get; set; } = new(Enum.GetValues(typeof(EyeColor)).Cast<EyeColor>());
        public ObservableCollection<HairColor> HairColors { get; set; } = new(Enum.GetValues(typeof(HairColor)).Cast<HairColor>());
        public ObservableCollection<Complexion> Complexions { get; set; } = new(Enum.GetValues(typeof(Complexion)).Cast<Complexion>());
        public ObservableCollection<Gender> Genders { get; set; } = new(Enum.GetValues(typeof(Gender)).Cast<Gender>());
        public ObservableCollection<Ethnicity> Ethnicities { get; set; } = new(Enum.GetValues(typeof(Ethnicity)).Cast<Ethnicity>());
        public ObservableCollection<AddressType> AddressTypes { get; set; } = new(Enum.GetValues(typeof(AddressType)).Cast<AddressType>());
        public string FullName { get { return _fullName; } set { if (_fullName != value) 
                { _fullName = value; OnPropertyChanged();
                    if (_fullName != "") { NameErrorVisibility = false; }
                } } }
        public ObservableCollection<Tuple<string, PhoneType>> Phones { get; set; } = new();
        public ObservableCollection<Tuple<string, EmailType>> Emails { get; set; } = new();
        public ObservableCollection<Hyperlink> Links { get; set; } = new();
        public ObservableCollection<Tuple<string, AddressType>> Addresses { get; set; } = new();
        public DateTime? _birthday;
        public DateTime? Birthday { get { return _birthday; } set { if (_birthday != value) { _birthday = value; OnPropertyChanged(); } } }
        public EmailType SelectedEmailType 
        { 
            get { return _selectedEmailType; }
            set { if (_selectedEmailType != value) { _selectedEmailType = value; OnPropertyChanged(); } }
        }
        public PhoneType SelectedPhoneType 
        { 
            get { return _selectedPhoneType; }
            set { if (_selectedPhoneType != value) { _selectedPhoneType = value; OnPropertyChanged(); } }
        }
        public AddressType SelectedAddressType
        {
            get { return _selectedAddressType; }
            set { if (_selectedAddressType != value) { _selectedAddressType = value; OnPropertyChanged(); } }
        }
        public bool NameErrorVisibility 
        { 
            get { return _nameErrorVisibility; }
            set { if (_nameErrorVisibility != value) { _nameErrorVisibility = value; OnPropertyChanged(); } }
        }
        public string PhoneText 
        { 
            get { return _phoneText; }
            set { if (_phoneText != value) { _phoneText = value; OnPropertyChanged(); } }
        }
        public string EmailText 
        { 
            get { return _emailText; }
            set { if (_emailText != value) { _emailText = value; OnPropertyChanged(); } }
        }
        public string LinkText 
        { 
            get { return _linkText; }
            set { if (_linkText != value) { _linkText = value; OnPropertyChanged(); } }
        }
        public string AddressText
        {
            get { return _addressText; }
            set { if (_addressText != value) { _addressText = value; OnPropertyChanged(); } }
        }
        public EyeColor SelectedEyeColor
        {
            get { return _selectedEyeColor; }
            set { if (_selectedEyeColor != value) { _selectedEyeColor = value; OnPropertyChanged(); } }
        }
        public HairColor SelectedHairColor
        {
            get { return _selectedHairColor; }
            set { if (_selectedHairColor != value) { _selectedHairColor = value; OnPropertyChanged(); } }
        }
        public Complexion SelectedComplexion
        {
            get { return _selectedComplexion; }
            set { if (_selectedComplexion != value) { _selectedComplexion = value; OnPropertyChanged(); } }
        }
        public Gender SelectedGender
        {
            get { return _selectedGender; }
            set { if (_selectedGender != value) { _selectedGender = value; OnPropertyChanged(); } }
        }
        public Ethnicity SelectedEthnicity
        {
            get { return _selectedEthnicity; }
            set { if (_selectedEthnicity != value) { _selectedEthnicity = value; OnPropertyChanged(); } }
        }
        public Height Height
        {
            get { return _height; }
            set { if (_height != value) { _height = value; OnPropertyChanged(); } }
        }
        public Weight Weight
        {
            get { return _weight; }
            set { if (_weight != value) { _weight = value; OnPropertyChanged(); } }
        }

        public DelegateCommand SubmitCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand AddEmailCommand { get; set; }
        public DelegateCommand AddPhoneCommand { get; set; }
        public DelegateCommand AddLinkCommand { get; set; }
        public DelegateCommand RemoveItemCommand { get; set; }
        public DelegateCommand AddAddressCommand { get; set; }


        public NewContactViewModel()
        {
            SubmitCommand = new(btnSubmit_Click, (x) => true);
            CancelCommand = new(CancelForm, (x) => true);
            AddEmailCommand = new(AddEmailToList, (x) => true);
            AddPhoneCommand = new(AddPhoneToList, (x) => true);
            AddLinkCommand = new(AddLinkToList, (x) => true);
            RemoveItemCommand = new(RemoveItem, (x) => true);
            AddAddressCommand = new(AddAddressToList, (x) => true);
            PhoneTypes.CollectionChanged += CollectionChanged;
            EmailTypes.CollectionChanged += CollectionChanged;
            EyeColors.CollectionChanged += CollectionChanged;
            HairColors.CollectionChanged += CollectionChanged;
            Complexions.CollectionChanged += CollectionChanged;
            Genders.CollectionChanged += CollectionChanged;
            Ethnicities.CollectionChanged += CollectionChanged;
            Phones.CollectionChanged += CollectionChanged;
            Emails.CollectionChanged += CollectionChanged;
            Links.CollectionChanged += CollectionChanged;
            Addresses.CollectionChanged += CollectionChanged;
            _selectedEmailType = EmailType.Personal;
            _selectedPhoneType = PhoneType.Home;
            _selectedAddressType = AddressType.Home;
            _fullName = "";
            _nameErrorVisibility = false;
            _phoneText = "";
            _emailText = "";
            _linkText = "";
            _addressText = "";
            _height = new();
            _weight = new();
        }
        public NewContactViewModel(Contact contact)
        {
            SubmitCommand = new(btnSubmit_Click, (x) => true);
            CancelCommand = new(CancelForm, (x) => true);
            AddEmailCommand = new(AddEmailToList, (x) => true);
            AddPhoneCommand = new(AddPhoneToList, (x) => true);
            AddLinkCommand = new(AddLinkToList, (x) => true);
            RemoveItemCommand = new(RemoveItem, (x) => true);
            AddAddressCommand = new(AddAddressToList, (x) => true);
            PhoneTypes.CollectionChanged += CollectionChanged;
            EmailTypes.CollectionChanged += CollectionChanged;
            EyeColors.CollectionChanged += CollectionChanged;
            HairColors.CollectionChanged += CollectionChanged;
            Complexions.CollectionChanged += CollectionChanged;
            Genders.CollectionChanged += CollectionChanged;
            Ethnicities.CollectionChanged += CollectionChanged;
            Phones.CollectionChanged += CollectionChanged;
            Emails.CollectionChanged += CollectionChanged;
            Links.CollectionChanged += CollectionChanged;
            Addresses.CollectionChanged += CollectionChanged;
            _selectedEmailType = EmailType.Personal;
            _selectedPhoneType = PhoneType.Home;
            _selectedAddressType = AddressType.Home;
            _selectedEyeColor = contact.PhysicalInfo.EyeColor;
            _selectedHairColor = contact.PhysicalInfo.HairColor;
            _selectedGender = contact.PhysicalInfo.Gender;
            _selectedComplexion = contact.PhysicalInfo.Complexion;
            _selectedEthnicity = contact.PhysicalInfo.Ethnicity;
            _weight = contact.PhysicalInfo.Weight;
            _height = contact.PhysicalInfo.Height;
            _weight.Lbs = contact.PhysicalInfo.Weight.Lbs;
            _height.Inches = contact.PhysicalInfo.Height.Inches;
            _nameErrorVisibility = false;
            _phoneText = "";
            _emailText = "";
            _linkText = "";
            _addressText = "";
            _fullName = contact.Name.FullName;
            Birthday = new(contact.Birthday, new());
            foreach (Email email in contact.EmailAddresses) { Emails.Add(new(email.ToString(), email.Type)); }
            foreach (PhoneNumber number in contact.PhoneNumbers) { Phones.Add(new(number.ToString(), number.Type)); }
            foreach (Hyperlink link in contact.Links) { Links.Add(link); }
            foreach (Address address in contact.Addresses) { Addresses.Add(new(address.ToString(), address.Type)); }
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

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
        public void AddAddressToList(object? param)
        {
            if (!Addresses.Contains(new(AddressText, SelectedAddressType)))
            {
                Addresses.Add(new(AddressText, SelectedAddressType));
            }
            AddressText = "";
            SelectedAddressType = AddressTypes[0];
            //txtAddress.Focus();
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
            else if (param is Tuple<string, AddressType>)
            {
                Addresses.Remove((Tuple<string, AddressType>)param);
            }
        }
    }
}
