using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Documents;

namespace ContactsMVVM.Models
{
    class Contact : IContact, INotifyPropertyChanged
    {
        private Name _Name;
        private Name _Nickname;
        private IContact[] _Aliases;
        private Address[] _Addresses;
        private PhoneNumber[] _phoneNumbers;
        private Email[] _emailAddresses;
        private Organization[] _organizations;
        private Hyperlink[] _links;
        private DateOnly _birthday;
        private int _imageIndex;
        private BitmapImage[] _images;
        public Name Name { get { return _Name; } set { if (value != _Name) { _Name = value; OnPropertyChanged(nameof(Name)); } } }
        public Name Nickname { get { return _Nickname; } set { if (value != _Nickname) { _Nickname = value; OnPropertyChanged(nameof(Nickname)); } } }
        public IContact[] Aliases { get { return _Aliases; } set { if (value != _Aliases) { _Aliases = value; OnPropertyChanged(nameof(Aliases)); } } }
        public Address[] Addresses { get { return _Addresses; } set { if (value != _Addresses) { _Addresses = value; OnPropertyChanged(nameof(Addresses)); } } }
        public PhoneNumber[] PhoneNumbers { get { return _phoneNumbers; } set { if (_phoneNumbers != value) { _phoneNumbers = value; OnPropertyChanged(nameof(PhoneNumbers)); } } }
        public Email[] EmailAddresses { get { return _emailAddresses; } set { if (_emailAddresses != value) { _emailAddresses = value; OnPropertyChanged(nameof(EmailAddresses)); } } }
        public Organization[] Organizations { get { return _organizations; } set { if (_organizations != value) { _organizations = value; OnPropertyChanged(nameof(Organizations)); } } }
        public Hyperlink[] Links { get { return _links; } set { if (_links != value) { _links = value; OnPropertyChanged(nameof(Links)); } } }
        public DateOnly Birthday { get { return _birthday; } set { if (_birthday != value) { _birthday = value; OnPropertyChanged(nameof(Birthday)); } } }
        public int ImageIndex { get { return _imageIndex; } set { if (_imageIndex != value) { if (value >= 0 && value < Images.Length) { _imageIndex = value; OnPropertyChanged(nameof(ImageIndex)); } } } }
        public BitmapImage[] Images { get { return _images; } set { if (_images != value) { _images = value; OnPropertyChanged(nameof(Images)); } } }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public Contact()
        {
            this._Name = new();
            this._Nickname = new();
            this._Aliases = [];
            this._Addresses = [];
            this._phoneNumbers = [];
            this._emailAddresses = [];
            this._organizations = [];
            this._links = [];
            this._birthday = new();
            this._imageIndex = -1;
            this._images = [];
        }
    }
}
