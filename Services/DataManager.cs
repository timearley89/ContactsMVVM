using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Text.Json;
using System.Xml.Serialization;
using ContactsMVVM.Models;
using System.Data.Common;
using System.Data;
using System.Windows.Data;

namespace ContactsMVVM.Services
{
    public static class DataManager
    {
        /// <summary>
        /// Saves a list of contacts to an XML file.
        /// </summary>
        /// <param name="FileName">Fully qualified path of xml file, eg.'C:/Users/Public/Documents/Contacts.xml'</param>
        /// <param name="contacts">List of type Contact to save</param>
        /// <returns>Bool denoting whether save was successful or not.</returns>
        /// <exception cref="InvalidOperationException">Suppressed if serialization fails - usually a data structure problem.</exception>
        /// <exception cref="ArgumentNullException">Thrown if FileName is null or if contacts is null/empty.</exception>
        /// <exception cref="ArgumentException">Thrown if FileName does not contain a qualified file path.</exception>
        public static bool SaveContactListXml(string FileName, List<SerializableContact> contacts)
        {
            if (contacts != null)
            {
                if (contacts.Count >= 1)
                {
                    if (FileName != null && FileName != string.Empty)
                    {
                        if (Path.IsPathFullyQualified(FileName))
                        {
                            if (!Path.Exists(Path.GetDirectoryName(FileName)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(FileName)!);
                            }
                            FileStream fs = new FileStream(FileName, FileMode.Create);
                            XmlSerializer Xser = new XmlSerializer(typeof(List<SerializableContact>));
                            try
                            {
                                
                                Xser.Serialize(fs, contacts);
                            }
                            catch (InvalidOperationException ex)
                            {
                                //throw new InvalidOperationException("Error occured during serialization.", ex);
                                string msg = ex.Message;
                                return false;
                            }
                            fs.Dispose();
                            return true;
                        }
                        else
                        {
                            //argument exception - filename not qualified path
                            throw new ArgumentException("FileName is not a qualified path.");
                            //return false;
                        }
                    }
                    else
                    {
                        //argument exception - filename is empty
                        throw new ArgumentNullException(nameof(FileName), "FileName was null or empty.");
                        //return false;
                    }
                }
                else
                {
                    //argument exception - list is empty
                    throw new ArgumentNullException(nameof(contacts), "Contact list contained no data.");
                    //return false;
                }
            }
            else
            {
                //argument exception - list is null
                throw new ArgumentNullException(nameof(contacts), "Contact list was null.");
                //return false;
            }
        }
        /// <summary>
        /// Loads a List of type Contact from file.
        /// </summary>
        /// <param name="FileName">Fully qualified path to load data from. Must point to an xml file.</param>
        /// <param name="contacts">Out param for loaded and assembled contact list.</param>
        /// <returns>True if loading and assembly was successful, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if FileName is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if FileName is not a fully qualified, existing file path.</exception>
        public static bool LoadContactListXml(string FileName, out List<SerializableContact>? contacts)
        {
            if (FileName != null && FileName != string.Empty && Path.IsPathFullyQualified(FileName) && Path.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                XmlSerializer Xser = new(typeof(List<SerializableContact>));
                List<SerializableContact>? mycontactlist;
                try
                {
                    mycontactlist = (List<SerializableContact>?)Xser.Deserialize(fs);
                }
                catch (InvalidOperationException)
                {
                    contacts = null;
                    return false;
                }
                fs.Dispose();
                contacts = mycontactlist;
                return true;
            }
            else
            {
                if (FileName != null && FileName != string.Empty)
                {
                    throw new ArgumentException("FileName was not a qualified, existing file path.", nameof(FileName));
                }
                else
                {
                    throw new ArgumentNullException(nameof(FileName), "FileName cannot be null or empty.");
                }
            }
        }
    }
}
