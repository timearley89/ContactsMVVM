using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactsMVVM.Models
{
    public class PhoneNumber : ObservableObject
    {
        private int _countryCode;
        private int _areaCode;
        private int _group1;
        private int _group2;
        private PhoneType _type;

        public int CountryCode { get { return _countryCode; } set { if (_countryCode != value) { _countryCode = value; OnPropertyChanged(); } } }
        public int AreaCode { get { return _areaCode; } set { if (_areaCode != value) { _areaCode = value; OnPropertyChanged(); } } }
        public int Group1 { get { return _group1; } set { if (_group1 != value) { _group1 = value; OnPropertyChanged(); } } }
        public int Group2 { get { return _group2; } set { if (_group2 != value) { _group2 = value; OnPropertyChanged(); } } }
        public PhoneType Type { get { return _type; } set { if (_type != value) { _type = value; OnPropertyChanged(); } } }

        public override string ToString()
        {
            //'+1(999)123-4567'
            return $"{(CountryCode != 0 ? "+" + CountryCode : "")}({AreaCode:###}){Group1:###}-{Group2:####}";
        }
        public static PhoneNumber Parse(string input)
        {
            //8006661234
            //18006661234
            //1-800-666-1234
            //+1(800)666-1234
            //in a phone number we should have either 10 or 11 digits. Strip input of anything non-numerical, and go by count.
            //if count isn't 10 or 11, it's an invalid number. Throw an argumentexception.
            string strippedinput = "";
            PhoneNumber tempNumber = new();
            for (int i = 0; i < input.Length; i++)
            {
                Regex pattern = new Regex(@"[\d]");
                if (pattern.IsMatch(input[i].ToString())) { strippedinput += input[i].ToString(); }
            }
            if (strippedinput.Length >= 10 && strippedinput.Length <= 11)
            {
                if (strippedinput.Length == 10)
                {
                    //no country code. Can we get the one from the system's current culture?
                    tempNumber.CountryCode = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName == "enu" ? 1 : 0;
                    for (int i = 0; i < 10; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                {
                                    tempNumber.AreaCode = Int32.Parse(strippedinput.Substring(i, 3));
                                    i = 2;
                                    break;
                                }
                            case 3:
                                {
                                    tempNumber.Group1 = Int32.Parse(strippedinput.Substring(i, 3));
                                    i = 5;
                                    break;
                                }
                            case 6:
                                {
                                    tempNumber.Group2 = Int32.Parse(strippedinput.Substring(i, 4));
                                    i = 9;
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    //Country code included. First number in sequence.
                    for (int i = 0; i < 11; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                {
                                    tempNumber.CountryCode = Int32.Parse(strippedinput[i].ToString()); break;
                                }
                            case 1:
                                {
                                    tempNumber.AreaCode = Int32.Parse(strippedinput.Substring(i, 3));
                                    i = 3;
                                    break;
                                }
                            case 4:
                                {
                                    tempNumber.Group1 = Int32.Parse(strippedinput.Substring(i, 3));
                                    i = 6;
                                    break;
                                }
                            case 7:
                                {
                                    tempNumber.Group2 = Int32.Parse(strippedinput.Substring(i, 4));
                                    i = 10;
                                    break;
                                }
                        }
                    }
                }
                return tempNumber;
            }
            else { throw new ArgumentException("Invalid Phone Number - Unable to parse."); }
        }
    }

    [DefaultValue(Home)]
    public enum PhoneType
    {
        Home = 0,
        Cell = 1,
        Work = 2,
        School = 4,
        Organization = 8,
        Other = 16
    }
}
