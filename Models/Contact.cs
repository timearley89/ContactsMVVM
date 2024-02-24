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
using System.Xml.Serialization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.Serialization;

namespace ContactsMVVM.Models
{
    public class Contact : ObservableObject, IContact
    {
        private Name _Name;
        private Name _Nickname;
        private SerializableContact[] _Aliases;
        private Address[] _Addresses;
        private PhoneNumber[] _phoneNumbers;
        private Email[] _emailAddresses;
        private Organization[] _organizations;
        private Hyperlink[] _links;
        private DateOnly _birthday;
        private int _imageIndex;
        private BitmapImage[] _images;
        [OptionalField(VersionAdded=2)]
        private PhysicalInfo _physicalInfo;
        public Name Name { get { return _Name; } set { if (value != _Name) { _Name = value; OnPropertyChanged(); } } }
        public Name Nickname { get { return _Nickname; } set { if (value != _Nickname) { _Nickname = value; OnPropertyChanged(); } } }
        public SerializableContact[] Aliases { get { return _Aliases; } set { if (value != _Aliases) { _Aliases = value; OnPropertyChanged(); } } }
        public Address[] Addresses { get { return _Addresses; } set { if (value != _Addresses) { _Addresses = value; OnPropertyChanged(); } } }
        public PhoneNumber[] PhoneNumbers { get { return _phoneNumbers; } set { if (_phoneNumbers != value) { _phoneNumbers = value; OnPropertyChanged(); } } }
        public Email[] EmailAddresses { get { return _emailAddresses; } set { if (_emailAddresses != value) { _emailAddresses = value; OnPropertyChanged(); } } }
        public Organization[] Organizations { get { return _organizations; } set { if (_organizations != value) { _organizations = value; OnPropertyChanged(); } } }
        [XmlIgnore]
        public Hyperlink[] Links { get { return _links; } set { if (_links != value) { _links = value; OnPropertyChanged(); } } }
        [XmlIgnore]
        public DateOnly Birthday { get { return _birthday; } set { if (_birthday != value) { _birthday = value; OnPropertyChanged(); OnPropertyChanged(nameof(LongBirthdayString)); } } }
        public string LongBirthdayString { get { return Birthday.ToLongDateString(); } }
        public int Age { get { return (int)Math.Floor((DateTime.Now - new DateTime(Birthday, new())).TotalDays / 365); } }
        public int ImageIndex { get { return _imageIndex; } set { if (_imageIndex != value) { if (value >= 0 && value < Images.Length) { _imageIndex = value; OnPropertyChanged(); } } } }
        [XmlIgnore]
        public BitmapImage[] Images { get { return _images; } set { if (_images != value) { _images = value; OnPropertyChanged(); } } }
        public PhysicalInfo PhysicalInfo { get { return _physicalInfo; } set { if (_physicalInfo != value) { _physicalInfo = value; OnPropertyChanged(); } } }


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
            this._physicalInfo = new();
        }
        public Contact(SerializableContact serContact)
        {
            this._Name = serContact.Name;
            this._Nickname = serContact.Nickname;
            this._Aliases = serContact.Aliases;
            this._Addresses = serContact.Addresses;
            this._phoneNumbers = serContact.PhoneNumbers;
            this._emailAddresses = serContact.EmailAddresses;
            this._organizations = serContact.Organizations;
            this._birthday = DateOnly.Parse(serContact.Birthday);
            this._imageIndex = serContact.ImageIndex;
            List<BitmapImage> myimages = new();
            foreach (byte[] imgdata in serContact.Images)
            {
                using (MemoryStream ms = new(imgdata))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                    myimages.Add(image);
                }
            }
            this._images = myimages.ToArray();
            List<Hyperlink> myLinks = new();
            foreach (string link in serContact.Links)
            {
                Run run = new(link);
                myLinks.Add(new(run) { NavigateUri=new(link)});
            }
            this._links = myLinks.ToArray<Hyperlink>();
            this._physicalInfo = serContact._physicalInfo;
            
        }
    }

}
