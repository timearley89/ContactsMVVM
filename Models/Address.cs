using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMVVM.Models
{
    public class Address : ObservableObject
    {
        private int _StreetNumber;
        private string _StreetName;
        private string _City;
        private string _State;
        private string _ZipCode;
        private AddressType _Type;
        public int StreetNumber { get { return _StreetNumber; } set { if (_StreetNumber != value) { _StreetNumber = value; OnPropertyChanged(nameof(StreetNumber)); } } }
        public string StreetName { get { return _StreetName; } set { if (_StreetName != value) { _StreetName = value; OnPropertyChanged(nameof(StreetName)); } } }
        public string City { get { return _City; } set { if (_City != value) { _City = value; OnPropertyChanged(nameof(City)); } } }
        public string State { get { return _State; } set { if (_State != value) { _State = value; OnPropertyChanged(nameof(State)); } } }
        public string ZipCode { get { return _ZipCode; } set { if (_ZipCode != value) { _ZipCode = value; OnPropertyChanged(nameof(ZipCode)); } } }
        public AddressType Type { get { return _Type; } set { if (_Type != value) { _Type = value; OnPropertyChanged(nameof(Type)); } } }

        public Address()
        {
            _StreetNumber = 0;
            _StreetName = string.Empty;
            _City = string.Empty;
            _State = string.Empty;
            _ZipCode = string.Empty;
        }

        public void Parse(string Address)
        {
            Address = Address.Trim();
            //--StreetNumber--//
            int numberLength = 0;
            while (Address[numberLength] >= 48 && Address[numberLength] <= 57) { numberLength++; }
            StreetNumber = Int32.Parse(Address.Substring(0, numberLength));
            Address = Address.Remove(0, numberLength);
            Address = Address.Trim();   //StreetNumber removed, ready for zip code.

            //--ZipCode--//
            int zipStartIndex = 0;
            for (int i = Address.Length - 1; i >= 0; i--)
            {
                if ((Address[i] <= 48 && Address[i] >= 57) || (Address[i] != '-')) { zipStartIndex = i + 1; break; }
            }
            ZipCode = Address.Substring(zipStartIndex, Address.Length - zipStartIndex);
            Address = Address.Remove(zipStartIndex, Address.Length - zipStartIndex);
            Address = Address.Trim();   //ZipCode removed, ready for splitting up.

            //--Parts Parsing--//
            string[] addressParts = Address.Split(',');
            if (addressParts.Length == 3)
            {
                StreetName = addressParts[0].Trim().Remove(',');
                City = addressParts[1].Trim().Remove(',');
                State = addressParts[2].Trim().Remove(',').Length > 2 ? addressParts[2].Trim().Remove(',') : addressParts[2].Trim().Remove(',').ToUpper();
            }
            else { throw new ArgumentException("The entered address was in an unsupported format - Could not split StreetName, City and State.", nameof(Address)); }
        }
        public override string ToString()
        {
            return (StreetNumber == default || StreetName == default || City == default || State == default || ZipCode == default) ? "" : $"{StreetNumber} {StreetName}, {City}, {State} {ZipCode}";
        }
    }
    public enum AddressType
    {
        Home = 0,
        Work = 1,
        School = 2,
        PO_Box = 4
    }
    
}
