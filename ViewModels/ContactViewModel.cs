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
         */

        //---Private Fields---//
        private ObservableCollection<Contact> _contacts;
        private bool _btnEditEnabled;
        private Contact? _selectedContact;

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
            set { if (_btnEditEnabled != value) { _btnEditEnabled = value; OnPropertyChanged(nameof(BtnEditEnabled)); } }
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
                    OnPropertyChanged(nameof(SelectedContact)); 
                } 
            }
        }


        //---Command Properties---//
        public DelegateCommand AddContactCommand { get; set; }
        public DelegateCommand EditContactCommand { get; set; }
        public DelegateCommand DeleteContactsCommand { get; set; }
        public DelegateCommand OnLinkCommand { get; set; }


        //---Constructors---//
        public ContactViewModel()
        {
            AddContactCommand = new DelegateCommand(AddNewContact, (x) => true);
            EditContactCommand = new DelegateCommand(EditContact, (x) => true);
            DeleteContactsCommand = new DelegateCommand(DeleteAllContacts, (x) => myContacts.Count > 0);
            OnLinkCommand = new DelegateCommand(OnLink, (x) => true);
            _contacts = new();
            _contacts.CollectionChanged += OnListChange;
            _btnEditEnabled = false;
        }


        //---Event Handlers---//
        private void OnListChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ContactCount));    //catches the event that was registered in the constructor and raises a UI event.
            DeleteContactsCommand.RaiseCanExecuteChanged();
        }


        //---Command Delegates---//
        public void AddNewContact(object? param)
        {
            NewContactForm newContact = new();
            NewContactViewModel? newContactVM = (NewContactViewModel)newContact.DataContext;
            newContact.ShowDialog();
            if (newContactVM.MyDialogResult == true)
            {
                Contact myCont = new();
                myCont.Name.FullName = newContactVM.FullName;

                List<Tuple<string, PhoneType>> phonenumbers = newContactVM.Phones.ToList();
                List<Tuple<string, EmailType>> emails = newContactVM.Emails.ToList();

                myCont.PhoneNumbers = new PhoneNumber[phonenumbers.Count];
                myCont.EmailAddresses = new Email[emails.Count];

                for (int i = 0; i < phonenumbers.Count; i++)
                {
                    myCont.PhoneNumbers[i] = PhoneNumber.Parse(phonenumbers[i].Item1);
                    myCont.PhoneNumbers[i].Type = phonenumbers[i].Item2;
                }
                for (int i = 0; i < emails.Count; i++)
                {
                    myCont.EmailAddresses[i] = Email.Parse(emails[i].Item1) ?? new();
                    myCont.EmailAddresses[i].Type = emails[i].Item2;
                }

                myCont.Links = newContactVM.Links.ToArray<Hyperlink>();

                if (newContactVM.Birthday != null)
                {
                    myCont.Birthday = new(newContactVM.Birthday.Value.Year, newContactVM.Birthday.Value.Month, newContactVM.Birthday.Value.Day);
                }

                myContacts.Add(myCont);

            }
            newContactVM = null;
        }
        public void DeleteAllContacts(object? param)
        {
            myContacts.Clear();
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
                    myContact.PhoneNumbers = myPhones.ToArray();
                    myContact.EmailAddresses = myEmails.ToArray();
                    myContact.Links = editContactVM.Links.ToArray();
                    contact = myContact;
                }
            }
        }

    }
}