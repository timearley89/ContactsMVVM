using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMVVM.Models
{
    public class Organization : ObservableObject
    {
        private string _name;
        private Address _address;
        private PhoneNumber _phoneNumber;
        private Email _email;

        public string Name
        {
            get { return _name; }
            set { if (_name != value) { _name = value; OnPropertyChanged(nameof(Name)); } }
        }
        public Address Address
        {
            get { return _address; }
            set { if (_address != value) { _address = value; OnPropertyChanged(nameof(Address)); } }
        }
        public PhoneNumber PhoneNumber
        {
            get { return _phoneNumber; }
            set { if (_phoneNumber != value) { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); } }
        }
        public Email Email
        {
            get { return _email; }
            set { if (_email != value) { _email = value; OnPropertyChanged(nameof(Email)); } }
        }
        public Organization()
        {
            _name = string.Empty;
            _address = new();
            _phoneNumber = new();
            _email = new();
        }
    }
}
