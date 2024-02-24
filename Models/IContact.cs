using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ContactsMVVM.Models
{
    public interface IContact
    {
        Name Name { get; set; }
        Name Nickname { get; set; }
        SerializableContact[] Aliases { get; set; }
        Address[] Addresses { get; set; }
        PhoneNumber[] PhoneNumbers { get; set; }
        Email[] EmailAddresses { get; set; }
        Organization[] Organizations { get; set; }
        Hyperlink[] Links { get; set; }
        DateOnly Birthday { get; set; }
        int ImageIndex { get; set; }
        BitmapImage[] Images { get; set; }
    }
}
