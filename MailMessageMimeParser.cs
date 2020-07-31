using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;

namespace XstReader
{
    static class MailMessageMimeParser
    {

        //parse mime message into a given message object adds alll attachments and inserts inline content to message body
        public static void parseMessage(Message m, String mimeText)
        {
            Dictionary<string, string>  headers = getHeaders(new StringReader(mimeText));
            string Boundary = Regex.Match(headers["content-type"], @"boundary=""(.*?)""", RegexOptions.IgnoreCase).Groups[1].Value;
            string[] messageParts = getMimeParts(Boundary, mimeText);

            foreach(string part in messageParts)
            {
                Dictionary<string, string> partHeaders = getHeaders(new StringReader(part));
                //message body
                if (partHeaders.Keys.Contains("content-type") && partHeaders["content-type"].Trim().Contains("text/html;"))
                {
                    m.BodyHtml = DecodeQuotedPrintable(partHeaders["mimeBody"]);
                    m.NativeBody = BodyType.HTML;
                }
                //real attachments
                else if (partHeaders.Keys.Contains("content-disposition") && partHeaders["content-disposition"].Trim().Contains("attachment;"))
                {
                    string filename = Regex.Match(partHeaders["content-disposition"], @"filename=""(.*?)""", RegexOptions.IgnoreCase).Groups[1].Value;
                    //add base64 content to an attachement on the message.
                    Attachment a = new Attachment();
                    a.LongFileName = filename;
                    a.AttachMethod = AttachMethods.afByValue;
                    a.AttachmentBytes = Convert.FromBase64String(partHeaders["mimeBody"]);
                    a.Size = a.AttachmentBytes.Length;
                    m.Attachments.Add(a);
                }
                //inline images
                else if (partHeaders.Keys.Contains("content-id"))
                {
                    Attachment a = new Attachment();
                    string contentid = Regex.Match(partHeaders["content-id"], @"<(.*)>", RegexOptions.IgnoreCase).Groups[1].Value;
                    string name = Regex.Match(partHeaders["content-type"], @".*name=""(.*)""", RegexOptions.IgnoreCase).Groups[1].Value;
                    a.AttachMethod = AttachMethods.afByValue;
                    a.ContentId = contentid;
                    a.LongFileName = name;
                    a.Flags = AttachFlags.attRenderedInBody;
                    a.AttachmentBytes = Convert.FromBase64String(partHeaders["mimeBody"]);
                    a.Size = a.AttachmentBytes.Length;
                    m.Attachments.Add(a);
                }
            }
        }

        //decrpts mime message bytes with a valid cert in the user cert store
        // returns the decrypted message as a string
        public static string decryptMessage(byte[] encryptedMessageBytes)
        {
            //get cert store and collection of valid certs
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

            //decrypt bytes with EnvelopedCms
            EnvelopedCms ec = new EnvelopedCms();
            ec.Decode(encryptedMessageBytes);
            ec.Decrypt(fcollection);
            byte[] decryptedData = ec.ContentInfo.Content;

            return System.Text.Encoding.ASCII.GetString(decryptedData);
        }

        //Signed messages are base64 endcoded and broken up with \r\n 
        //This extracts the base64 content from signed message that has been wrapped in an encrypted message and decodes it
        // returns the decoded message string
        public static string DecodeSignedMessage(string s)
        {
            //parse out base64 encoded content in "signed-data"
            string base64Message = s.Split(new string[] { "filename=smime.p7m" }, StringSplitOptions.None)[1];
            string data = base64Message.Replace("\r\n", "");

            // parse out signing data from content
            SignedCms sc = new SignedCms();
            sc.Decode(Convert.FromBase64String(data));

            return System.Text.Encoding.ASCII.GetString(sc.ContentInfo.Content);
        }

        //parse out mime headers from a mime section
        //returns a dictionary with the header type as the key and its value as the value
        private static Dictionary<string, string> getHeaders(StringReader mimeText)
        {
            Dictionary<string, string> Headers = new Dictionary<string, string>();

            string line = string.Empty;
            string lastHeader = string.Empty;
            while ((!string.IsNullOrEmpty(line = mimeText.ReadLine()) && (line.Trim().Length != 0)))
            {

                //If the line starts with a whitespace it is a continuation of the previous line
                if (Regex.IsMatch(line, @"^\s"))
                {
                    Headers[lastHeader] = Headers[lastHeader] + " " + line.TrimStart('\t', ' ');
                }
                else
                {
                    string headerkey = line.Substring(0, line.IndexOf(':')).ToLower();
                    string value = line.Substring(line.IndexOf(':') + 1).TrimStart(' ');
                    if (value.Length > 0)
                        Headers[headerkey] = line.Substring(line.IndexOf(':') + 1).TrimStart(' ');
                    lastHeader = headerkey;
                }
            }

            string mimeBody = "";
            while ((line = mimeText.ReadLine()) != null)
            {
                mimeBody += line +"\r\n";
            }
            Headers["mimeBody"] = mimeBody;
            return Headers;
        }

        // splits a mime message into its individual parts
        // returns a string[] with the parts
        private static string[] getMimeParts(string initialBoundary, string mimetext)
        {
            String partRegex = @"\r\n------=_NextPart_.*\r\n";
            string[] test = Regex.Split(mimetext, partRegex);

            return test;
        }

        //decodes quoted printable text into UTF-8
        // returns the decoded text
        private static string DecodeQuotedPrintable(string input)
        {
            Regex regex = new Regex(@"(\=[0-9A-F][0-9A-F])+|=\r\n", RegexOptions.IgnoreCase);
            string value = regex.Replace(input, new MatchEvaluator(HexDecoderEvaluator));
            return value;
        }

        //converts hex endcoded values to UTF-8
        //returns the UTF-8 representation of the hex encoded value
        private static string HexDecoderEvaluator(Match m)
        {
            if (m.Groups[1].Success)
            {
                byte[] bytes = new byte[m.Value.Length / 3];

                for (int i = 0; i < bytes.Length; i++)
                {
                    string hex = m.Value.Substring(i * 3 + 1, 2);
                    int iHex = Convert.ToInt32(hex, 16);
                    bytes[i] = Convert.ToByte(iHex);
                }
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            return "";
        }
    }
}
