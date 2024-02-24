using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ContactsMVVM.Models
{
    public class SerializableContact : Contact
    {
        public new string[] Links { get; set; }
        public new byte[][] Images { get; set; }
        public new string Birthday { get; set; }
        public SerializableContact(Contact contact)
        {
            this.Name = contact.Name;
            this.Nickname = contact.Nickname;
            this.Aliases = contact.Aliases;
            this.Addresses = contact.Addresses;
            this.PhoneNumbers = contact.PhoneNumbers;
            this.EmailAddresses = contact.EmailAddresses;
            this.Organizations = contact.Organizations;
            List<string> mylinks = new();
            foreach (Hyperlink link in contact.Links)
            {
                mylinks.Add(link.NavigateUri.AbsoluteUri);
            }
            this.Links = mylinks.ToArray<string>();
            this.Birthday = contact.Birthday.ToString();
            this.ImageIndex = contact.ImageIndex;
            //this.Images = contact.Images;
            List<byte[]> byteimages = new();
            foreach (BitmapImage image in contact.Images)
            {
                byte[] imgData;
                JpegBitmapEncoder bmpEncoder = new();
                bmpEncoder.Frames.Add(BitmapFrame.Create(image));
                using (MemoryStream ms = new())
                {
                    bmpEncoder.Save(ms);
                    imgData = ms.ToArray();
                }
                byteimages.Add(imgData);
            }
            this.Images = byteimages.ToArray();
            this.PhysicalInfo = contact.PhysicalInfo;
        }
        private SerializableContact()
        {
            this.Links = new string[0];
            this.Images = new byte[0][];
            this.Birthday = "";
        }
    }
}
