using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactsMVVM.Models
{
    public class Email : ObservableObject
    {
        private string _username;
        private string _domain;
        private EmailType _type;
        public string Username { get { return _username; } set { if (_username != value) { _username = ValidateUsername(value) ?? ""; OnPropertyChanged(); } } }
        public string Domain { get { return _domain; } set { if (_domain != value) { _domain = ValidateDomain(value) ?? ""; OnPropertyChanged(); } } }
        public EmailType Type { get { return _type; } set { if (_type != value) { _type = value; OnPropertyChanged(); } } }

        public static string? ValidateUsername(string username)
        {
            //no commas, periods can not start or end username, and cannot be consecutive
            if (username.Contains(",") || username.Contains("..") || username.StartsWith(".") || username.EndsWith(".")) { return null; }
            return username;
        }
        public static string? ValidateDomain(string domain)
        {
            //'.' separated list of labels, each label being 'A-Z','a-z', or '-' as long as not first or last char.
            Regex pattern = new Regex(@"\A[^-][A-Za-z0-9-]+[^-]\z");
            if (domain.Split('.').All(x => pattern.IsMatch(x)) && domain.Split('.').Length >= 2) { return domain; } else { return null; }
        }
        public Email()
        {
            this._username = "None";
            this._domain = "empty.com";
        }
        public override string ToString()
        {
            return Username + "@" + Domain;
        }
        public static Email? Parse(string input)
        {
            //if email isn't valid, return null email
            string[] parts = input.Split('@');
            if (parts.Length != 2) { return null; }
            if (ValidateUsername(parts[0]) == null || ValidateDomain(parts[1]) == null) { return null; }
            Email outEmail = new Email() { _username = parts[0], _domain = parts[1] };
            return outEmail;
        }
    }
    [DefaultValue(Personal)]
    public enum EmailType
    {
        Personal = 0,
        School = 1,
        Work = 2,
        Other = 4
    }
}
