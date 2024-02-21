using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMVVM.Models
{
    public class Name : ObservableObject
    {
        private string _fullName = string.Empty;
        private string _first = string.Empty;
        private string _middle = string.Empty;
        private string _last = string.Empty;
        private NameSuffix _suffix;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (value != _fullName)
                {
                    _fullName = value;
                    ParseName(FullName);
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }
        public string First
        {
            get { return _first; }
            set
            {
                if (value != _first)
                {
                    _first = value;
                    OnPropertyChanged(nameof(First));
                }
            }
        }
        public string Last
        {
            get { return _last; }
            set
            {
                if (value != _last)
                {
                    _last = value;
                    OnPropertyChanged(nameof(Last));
                }
            }
        }
        public string Middle
        {
            get { return _middle; }
            set
            {
                if (value != _middle)
                {
                    _middle = value;
                    OnPropertyChanged(nameof(Middle));
                }
            }
        }
        NameSuffix Suffix
        {
            get { return _suffix; }
            set
            {
                if (value != _suffix)
                {
                    _suffix = value;
                    OnPropertyChanged(nameof(Suffix));
                }
            }
        }


        /// <summary>
        /// Takes a fullname string and breaks it down into it's constituent parts, including suffix. Call from FullName set method.
        /// </summary>
        /// <param name="fullName">String representing full name of contact in form 'F M L S', 'F M L' or 'F L'.</param>
        private void ParseName(string fullName)
        {
            string[] nameparts = fullName.Split(' ');
            for (int i = 0; i < nameparts.Length; i++)
            {
                nameparts[i] = nameparts[i].Replace('.', ' ').Trim();
            }
            if (nameparts.Length > 1)
            {   //'FMLS'||'FML'||'FLS'||'FL'
                this.First = nameparts[0];
                if (nameparts.Length > 2)
                {   //'FMLS'||'FML'||'FLS'
                    this.Middle = nameparts[1];
                    if (nameparts.Length > 3)
                    {   //'FMLS'
                        this.Last = nameparts[2];
                        object? tempSuffix;
                        if (Enum.TryParse(typeof(NameSuffix), nameparts[3], out tempSuffix))
                        {
                            this.Suffix = (NameSuffix)tempSuffix;
                        }
                        else
                        {
                            this.Suffix = NameSuffix.None;
                        }
                    }
                    else //'FML'||'FLS'
                    {
                        if (Enum.TryParse(typeof(NameSuffix), nameparts[2], out _))
                        {
                            //'FLS'
                            this.Middle = string.Empty;
                            this.Last = nameparts[1];
                            this.Suffix = (NameSuffix)Enum.Parse(typeof(NameSuffix), nameparts[2]);
                        }
                        else
                        {   //'FML'
                            this.Last = nameparts[2];
                            this.Suffix = NameSuffix.None;
                        }
                    }
                }
                else //'FL'
                {
                    this.Middle = string.Empty;
                    this.Suffix = NameSuffix.None;
                    this.Last = nameparts[1];
                }
            }
            else
            {
                //name is only 1 word, or is empty.
                if (nameparts[0].Trim() != "")
                {
                    this.First = nameparts[0];
                    this.Middle = string.Empty;
                    this.Last = string.Empty;
                    this.Suffix = NameSuffix.None;
                }
                else
                {
                    this.First = string.Empty;
                    this.Middle = string.Empty;
                    this.Last = string.Empty;
                    this.Suffix = NameSuffix.None;
                }
            }
        }
        public override string ToString()
        {
            return $"{First} {(Middle != "" ? Middle + " " : "")}{Last} {(Suffix != NameSuffix.None ? Suffix : "")}".Trim();
        }
    }

    [DefaultValue(None)]
    public enum NameSuffix
    {
        None = 0,
        Sr = 1,
        Jr = 2,
        III = 4,
        IV = 8,
        V = 16,
        VI = 32,
        VII = 64,
        VIII = 128,
        IX = 256,
        X = 512,
        XI = 1024,
        XII = 2048,
        XIII = 4096,
        XIV = 8192,
        XV = 16384,
        XVI = 32768,
        XVII = 65536,
        XVIII = 131072,
        XIX = 262144,
        XX = 524288
    }
}
