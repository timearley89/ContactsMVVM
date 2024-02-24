using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsMVVM.Models
{
    public class PhysicalInfo : ObservableObject
    {
        private EyeColor _eyeColor;
        private Complexion _complexion;
        private Gender _gender;
        private HairColor _hairColor;
        private Ethnicity _ethnicity;
        private Height _height;
        private Weight _weight;
        public EyeColor EyeColor
        {
            get { return _eyeColor; }
            set { if (_eyeColor != value) { _eyeColor = value; OnPropertyChanged(); } }
        }
        public Complexion Complexion
        {
            get { return _complexion; }
            set { if (_complexion != value) { _complexion = value; OnPropertyChanged(); } }
        }
        public Gender Gender
        {
            get { return _gender; }
            set { if (_gender != value) { _gender = value; OnPropertyChanged(); } }
        }
        public HairColor HairColor
        {
            get { return _hairColor; }
            set { if (_hairColor != value) { _hairColor = value; OnPropertyChanged(); } }
        }
        public Ethnicity Ethnicity
        {
            get { return _ethnicity; }
            set { if (_ethnicity != value) { _ethnicity = value; OnPropertyChanged(); } }
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

        public PhysicalInfo()
        {
            _height = new();
            _weight = new();
        }
    }
    public enum EyeColor
    {
        NotSet = 0,
        Blue=1,
        Brown=2,
        Green=4,
        Hazel=8,
        Gray=16,
        Red=32,
        Amber=64
    }
    public enum Complexion
    {
        NotSet = 0,
        VeryFair=1,
        Fair=2,
        Medium=4,
        Olive=8,
        Brown=16,
        Black=32
    }
    public enum Gender
    {
        NotSet=0,
        Male=1,
        Female=2,
        Other=4,
        Decline=8
    }
    public enum HairColor
    {
        NotSet=0,
        Black=1,
        Brown=2,
        Red=4,
        Blonde=8
    }
    public enum Ethnicity
    {
        NotSet=0,
        Hispanic=1,
        AmericanIndian=2,
        Asian=3,
        Black=4,
        PacificIslander=5,
        White=6,
        Other=7
    }
    public class Height : ObservableObject
    {
        private double _centimeters;
        private double _inches;
        public double Centimeters 
        { 
            get { return _centimeters; }
            set 
            { 
                if (_centimeters != value) 
                { 
                    _centimeters = value;
                    OnPropertyChanged();
                    Inches = _centimeters / 2.54d;
                } 
            } 
        }
        public double Inches 
        { 
            get { return _inches; }
            set
            {
                if (_inches != value)
                {
                    _inches = value;
                    OnPropertyChanged();
                    Centimeters = _inches * 2.54d;
                }
            }
        }
        public Height()
        {
            _inches = 0;
            _centimeters = 0;
        }
        /// <summary>
        /// Returns height, in inches, as a string representation.
        /// </summary>
        /// <returns>Height in Centimeters</returns>
        public override string ToString()
        {
            return Inches.ToString();
        }
        public string ToString(HeightOptions options = HeightOptions.Inches)
        {
            switch (options)
            {
                case HeightOptions.Inches:
                    {
                        return Inches.ToString();
                    }
                case HeightOptions.Centimeters:
                    {
                        return Centimeters.ToString();
                    }
                case HeightOptions.FeetExplicit:
                    {
                        int feet = (int)Math.Floor(Inches / 12.0d);
                        int inches = (int)(Inches % 12.0d);
                        return feet.ToString() + '.' + inches.ToString();
                    }
                case HeightOptions.Feet:
                    {
                        return (Inches / 12).ToString();
                    }
                case HeightOptions.MetersExplicit:
                    {
                        int meters = (int)Math.Floor(Centimeters / 100d);
                        int cents = (int)(Centimeters % 100d);
                        return meters.ToString() + '.' + cents.ToString();
                    }
                case HeightOptions.Meters:
                    {
                        return (Centimeters / 100).ToString();
                    }
                case HeightOptions.FeetVerbose:
                    {
                        return $"{Math.Floor(Inches / 12)} Feet, {Inches%12} Inches";
                    }
                case HeightOptions.MetersVerbose:
                    {
                        return $"{Math.Floor(Centimeters/100)} Meters, {Centimeters%100} Centimeters";
                    }
                case HeightOptions.InchesVerbose:
                    {
                        return $"{Inches} Inches";
                    }
                case HeightOptions.CentimetersVerbose:
                    {
                        return $"{Centimeters} Centimeters";
                    }
            }
            throw new InvalidEnumArgumentException("Options", (int)options, typeof(HeightOptions));
        }
        [DefaultValue(Inches)]
        public enum HeightOptions
        {
            Inches=0,
            Centimeters=1,
            /// <summary>
            /// Returns Inches / 12.
            /// </summary>
            Feet=2,
            /// <summary>
            /// Returns 'Feet.Inches'.
            /// </summary>
            FeetExplicit=3,
            /// <summary>
            /// Returns Centimeters / 100.
            /// </summary>
            Meters=4,
            /// <summary>
            /// Returns 'Meters.Centimeters'.
            /// </summary>
            MetersExplicit=5,
            /// <summary>
            /// Returns 'X Feet, Y Inches'.
            /// </summary>
            FeetVerbose=6,
            /// <summary>
            /// Returns 'X Meters, Y Centimeters'.
            /// </summary>
            MetersVerbose=7,
            /// <summary>
            /// Returns 'X Inches'.
            /// </summary>
            InchesVerbose=8,
            /// <summary>
            /// Returns 'X Centimeters'.
            /// </summary>
            CentimetersVerbose=9
        }
    }
    public class Weight : ObservableObject
    {
        private double _kilograms;
        private double _lbs;
        public double Kilograms
        {
            get { return _kilograms; }
            set 
            { 
                if (_kilograms != value)
                {
                    _kilograms = value;
                    OnPropertyChanged();
                    Lbs = _kilograms * 2.205d;
                } 
            }
        }
        public double Lbs 
        { 
            get { return _lbs; }
            set
            {
                if (_lbs != value)
                {
                    _lbs = value;
                    OnPropertyChanged();
                    Kilograms = _lbs / 2.205d;
                }
            }
        }
        public Weight()
        {
            _lbs = 0;
            _kilograms = 0;
        }
        /// <summary>
        /// Returns weight, in lbs, as string representation.
        /// </summary>
        /// <returns>Weight.Lbs.ToString()</returns>
        public override string ToString()
        {
            return Lbs.ToString();
        }
        public string ToString(WeightOptions options = WeightOptions.Lbs)
        {
            switch (options)
            {
                case WeightOptions.Ounces:
                    {
                        return (Lbs * 16).ToString();
                    }
                case WeightOptions.Lbs:
                    {
                        return Lbs.ToString();
                    }
                case WeightOptions.Grams:
                    {
                        return (Kilograms * 1000).ToString();
                    }
                case WeightOptions.Kilograms:
                    {
                        return Kilograms.ToString();
                    }
                case WeightOptions.OuncesVerbose:
                    {
                        return $"{Lbs * 16} Ounces";
                    }
                case WeightOptions.LbsVerbose:
                    {
                        return $"{Lbs} Pounds";
                    }
                case WeightOptions.GramsVerbose:
                    {
                        return $"{Kilograms * 1000} Grams";
                    }
                case WeightOptions.KilogramsVerbose:
                    {
                        return $"{Kilograms} Kilograms";
                    }
            }
            throw new InvalidEnumArgumentException(nameof(options), (int)options, typeof(WeightOptions));
        }
        [DefaultValue(Lbs)]
        public enum WeightOptions
        {
            Ounces=1,
            Lbs=2,
            Grams=3,
            Kilograms=4,
            OuncesVerbose=5,
            LbsVerbose=6,
            GramsVerbose=7,
            KilogramsVerbose=8
        }
    }
}
