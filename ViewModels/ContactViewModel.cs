using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using ContactsMVVM.Models;
using ContactsMVVM.Views;
using System.Windows.Navigation;
using System.Diagnostics;
using ContactsMVVM.Services;
using System.Windows;

namespace ContactsMVVM.ViewModels
{
    class ContactViewModel : ObservableObject
    {
        /*this viewmodel is only for translating model data into view-bound properties, 
         * and relaying view commands to model.
         * 
         * As such, it should contain:
         * -Methods for getting data from model and setting view-bound properties
         * -Methods for receiving commands from view and raising model commands
         * Model --data--> ViewModel --data--> View
         * Model <--commands-- ViewModel <--commands-- View
         * 
         * Perhaps it should subscribe to any events raised by the model, and "handle" them,
         * then raise it's own PropertyChanged event to tell the view to update through it's binding
         * 
         * Height and weight aren't updating in newcontactform because we're calling the parameterless constructor for it's VM
         * and then calling the Contact constructor for it's VM. Maybe we should have properties in this viewmodel for holding the active
         * view model and view so that we only deal with one instance of each?
         */

        //---Private Fields---//
        private ObservableCollection<Contact> _contacts;
        private bool _btnEditEnabled;
        private Contact? _selectedContact;
        private string _fileSavePath;
        private string _fileLoadPath;
        private string _statusBarText;

        //---Public Properties---//
        public ObservableCollection<Contact> myContacts { get { return _contacts; }
            set { _contacts = value; OnPropertyChanged(nameof(myContacts)); } }
        public List<PhoneType> PhoneTypes { get; } = Enum.GetValues(typeof(PhoneType)).Cast<PhoneType>().ToList();
        public List<EmailType> EmailTypes { get; } = Enum.GetValues(typeof(EmailType)).Cast<EmailType>().ToList();
        public string ContactCount
        {
            get { return myContacts.Count != 1 ? $"{myContacts.Count} Contacts In Storage" : $"{myContacts.Count} Contact In Storage"; }
        }
        public bool BtnEditEnabled
        {
            get { return _btnEditEnabled; }
            set { if (_btnEditEnabled != value) { _btnEditEnabled = value; OnPropertyChanged(); } }
        }
        public Contact? SelectedContact
        { 
            get { return _selectedContact; }
            set 
            { 
                if (_selectedContact != value) 
                { 
                    _selectedContact = value; 
                    if (_selectedContact != null) { BtnEditEnabled = true; } else { BtnEditEnabled = false; } 
                    OnPropertyChanged(); 
                } 
            }
        }
        public string FileSavePath 
        { 
            get { return _fileSavePath; }
            set { if (_fileSavePath != value) { _fileSavePath = value; OnPropertyChanged(); } }
        }
        public string FileLoadPath 
        { 
            get { return _fileLoadPath; }
            set { if (_fileLoadPath != value) { _fileLoadPath = value; OnPropertyChanged(); } }
        }
        public string StatusBarText
        {
            get { return _statusBarText; }
            set { if (_statusBarText != value) { _statusBarText = value; OnPropertyChanged(); } }
        }


        //---Command Properties---//
        public DelegateCommand AddContactCommand { get; set; }
        public DelegateCommand EditContactCommand { get; set; }
        public DelegateCommand DeleteContactsCommand { get; set; }
        public DelegateCommand OnLinkCommand { get; set; }
        public DelegateCommand SaveContactsCommand { get; set; }
        public DelegateCommand LoadContactsCommand { get; set; }


        //---Constructors---//
        public ContactViewModel()
        {
            AddContactCommand = new DelegateCommand(AddNewContact, (x) => true);
            EditContactCommand = new DelegateCommand(EditContact, (x) => true);
            DeleteContactsCommand = new DelegateCommand(DeleteAllContacts, (x) => myContacts.Count > 0);
            OnLinkCommand = new DelegateCommand(OnLink, (x) => true);
            SaveContactsCommand = new DelegateCommand(SaveContacts, (x) => myContacts.Count >= 1);
            LoadContactsCommand = new DelegateCommand(LoadContacts, (x) => true);
            _contacts = new();
            _contacts.CollectionChanged += OnListChange;
            _btnEditEnabled = false;
            _fileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"/Earleytech/ContactsMVVM/Contacts.xml";
            _fileLoadPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"/Earleytech/ContactsMVVM/Contacts.xml";
            _statusBarText = "Initialization Complete!";
        }


        //---Event Handlers---//
        private void OnListChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ContactCount));    //catches the event that was registered in the constructor and raises a UI event.
            DeleteContactsCommand.RaiseCanExecuteChanged();
            SaveContactsCommand.RaiseCanExecuteChanged();
        }


        //---Command Delegates---//
        public void AddNewContact(object? param)
        {

            //---DataContext---//
            NewContactForm newContact = new();
            NewContactViewModel? newContactVM = new();
            newContact.DataContext = newContactVM;


            newContact.ShowDialog();
            if (newContactVM.MyDialogResult == true)
            {
                Contact myCont = new();
                myCont.Name.FullName = newContactVM.FullName;

                myCont.PhoneNumbers = new PhoneNumber[newContactVM.Phones.Count];
                myCont.EmailAddresses = new Email[newContactVM.Emails.Count];
                myCont.Addresses = new Address[newContactVM.Addresses.Count];

                for (int i = 0; i < newContactVM.Phones.Count; i++)
                {
                    myCont.PhoneNumbers[i] = new();
                    myCont.PhoneNumbers[i] = PhoneNumber.Parse(newContactVM.Phones[i].Item1);
                    myCont.PhoneNumbers[i].Type = newContactVM.Phones[i].Item2;
                }
                for (int i = 0; i < newContactVM.Emails.Count; i++)
                {
                    myCont.EmailAddresses[i] = new();
                    myCont.EmailAddresses[i] = Email.Parse(newContactVM.Emails[i].Item1) ?? new();
                    myCont.EmailAddresses[i].Type = newContactVM.Emails[i].Item2;
                }
                for (int i = 0; i < newContactVM.Addresses.Count; i++)
                {
                    myCont.Addresses[i] = new();
                    myCont.Addresses[i].Parse(newContactVM.Addresses[i].Item1);
                    myCont.Addresses[i].Type = newContactVM.Addresses[i].Item2;
                }

                myCont.Links = newContactVM.Links.ToArray<Hyperlink>();

                if (newContactVM.Birthday != null)
                {
                    myCont.Birthday = new(newContactVM.Birthday.Value.Year, newContactVM.Birthday.Value.Month, newContactVM.Birthday.Value.Day);
                }
                myCont.PhysicalInfo.EyeColor = newContactVM.SelectedEyeColor;
                myCont.PhysicalInfo.HairColor = newContactVM.SelectedHairColor;
                myCont.PhysicalInfo.Complexion = newContactVM.SelectedComplexion;
                myCont.PhysicalInfo.Gender = newContactVM.SelectedGender;
                myCont.PhysicalInfo.Ethnicity = newContactVM.SelectedEthnicity;
                myCont.PhysicalInfo.Height = newContactVM.Height;
                myCont.PhysicalInfo.Weight = newContactVM.Weight;
                myContacts.Add(myCont);
                OnPropertyChanged(nameof(ContactCount));
            }
            newContactVM = null;
            StatusBarText = "Contact Added!";
        }
        public void DeleteAllContacts(object? param)
        {
            myContacts.Clear();
            StatusBarText = "All Contacts Deleted!";
        }

        public void OnLink(object? button)
        {
            if (button != null && button is Button)
            {
                Button btn = (Button)button;
                Process.Start(new ProcessStartInfo(btn.Content.ToString() ?? "") { UseShellExecute = true });
            }
        }

        public void EditContact(object? contact)
        {
            if (contact is Contact)
            {
                Contact myContact = (Contact)contact;

                //---DataContext---//
                NewContactForm? editContact = new();
                NewContactViewModel? editContactVM = new(myContact);
                editContact.DataContext = editContactVM;


                editContact.ShowDialog();
                if (editContactVM != null)
                {
                    myContact.Name.FullName = editContactVM.FullName;
                    if (editContactVM.Birthday != null)
                    {
                        myContact.Birthday = new(editContactVM.Birthday.Value.Year,
                            editContactVM.Birthday.Value.Month,
                            editContactVM.Birthday.Value.Day);
                    }
                    List<Email> myEmails = new();
                    List<PhoneNumber> myPhones = new();
                    List<Address> myAddresses = new();
                    foreach (Tuple<string, EmailType> email in editContactVM.Emails)
                    {
                        if (email != null && email.Item1 != null)
                        {
                            Email? email1 = Email.Parse(email.Item1);
                            email1!.Type = email.Item2;
                            myEmails.Add(email1);
                        }
                    }
                    foreach (Tuple<string, PhoneType> phone in editContactVM.Phones)
                    {
                        if (phone != null && phone.Item1 != null)
                        {
                            PhoneNumber? phone1 = PhoneNumber.Parse(phone.Item1);
                            phone1!.Type = phone.Item2;
                            myPhones.Add(phone1);
                        }
                    }
                    foreach (Tuple<string, AddressType> address in editContactVM.Addresses)
                    {
                        if (address != null && address.Item1 != null)
                        {
                            Address? address1 = new();
                            address1.Parse(address.Item1);
                            address1.Type = address.Item2;
                            myAddresses.Add(address1);
                        }
                    }
                    myContact.PhoneNumbers = myPhones.ToArray();
                    myContact.EmailAddresses = myEmails.ToArray();
                    myContact.Addresses = myAddresses.ToArray();
                    myContact.Links = editContactVM.Links.ToArray();
                    myContact.PhysicalInfo.EyeColor = editContactVM.SelectedEyeColor;
                    myContact.PhysicalInfo.HairColor = editContactVM.SelectedHairColor;
                    myContact.PhysicalInfo.Complexion = editContactVM.SelectedComplexion;
                    myContact.PhysicalInfo.Gender = editContactVM.SelectedGender;
                    myContact.PhysicalInfo.Ethnicity = editContactVM.SelectedEthnicity;
                    myContact.PhysicalInfo.Height = editContactVM.Height;
                    myContact.PhysicalInfo.Weight = editContactVM.Weight;
                    contact = myContact;
                    StatusBarText = "Successfully Edited Contact!";
                }
            }
        }
        public void SaveContacts(object? param)
        {
            if (param is ObservableCollection<Contact>)
            {
                List<SerializableContact> contacts = new();
                foreach (Contact contact in (ObservableCollection<Contact>)param)
                {
                    contacts.Add(new(contact));
                }
                if (DataManager.SaveContactListXml(FileSavePath, contacts))
                {
                    //save successful
                    StatusBarText = "Save Successful!";
                }
                else
                {
                    //save error
                    StatusBarText = "An Error Occurred during Save.";
                }
            }
        }
        public void LoadContacts(object? param)
        {
            List<SerializableContact>? loadedContacts;
            if (DataManager.LoadContactListXml(FileLoadPath, out loadedContacts))
            {
                //load successful
                if (loadedContacts != null)
                {
                    List<Contact> mycontacts = new();
                    foreach (SerializableContact sercontact in loadedContacts)
                    {
                        mycontacts.Add(new(sercontact));
                    }
                    myContacts = new(mycontacts);
                    OnPropertyChanged(nameof(ContactCount));
                    DeleteContactsCommand.RaiseCanExecuteChanged();
                    SaveContactsCommand.RaiseCanExecuteChanged();
                }
                StatusBarText = "Load Successful!";
            }
            else
            {
                //load error
                StatusBarText = "An Error Occurred during Load.";
            }
        }
    }
}