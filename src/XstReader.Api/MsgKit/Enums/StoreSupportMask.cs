//
// RecipientType.cs
//
// Author: RecipientRowDisplayType and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;


namespace XstReader.MsgKit.Enums
{
    internal class StoreSupportMaskConst
    {
        public const StoreSupportMask StoreSupportMask = Enums.StoreSupportMask.STORE_ATTACH_OK |
                                                         //StoreSupportMask.STORE_CATEGORIZE_OK |
                                                         Enums.StoreSupportMask.STORE_CREATE_OK |
                                                         //StoreSupportMask.STORE_ENTRYID_UNIQUE |
                                                         Enums.StoreSupportMask.STORE_MODIFY_OK |
                                                         //StoreSupportMask.STORE_MV_PROPS_OK |
                                                         //StoreSupportMask.STORE_OLE_OK |
                                                         //StoreSupportMask.STORE_RTF_OK |
                                                         Enums.StoreSupportMask.STORE_HTML_OK |
                                                         Enums.StoreSupportMask.STORE_UNICODE_OK;
    }

    /// <summary>
    ///     This property discloses the capabilities of a message store to client applications planning to send it a message.
    ///     The flags can facilitate decisions by a client or another store, such as whether to send PR_BODY (PidTagBody) or
    ///     only <see cref="PropertyTags.PR_RTF_COMPRESSED" /> (PidTagRtfCompressed). A client should never set 
    ///     <see cref="PropertyTags.PR_STORE_SUPPORT_MASK" /> an attempt returns MAPI_E_COMPUTED.
    /// </summary>
    [Flags]
    internal enum StoreSupportMask : uint
    {
        /// <summary>
        ///     The message store supports properties containing ANSI (8-bit) characters.
        /// </summary>
        STORE_ANSI_OK = 0x00020000,

        /// <summary>
        ///     The message store supports attachments (OLE or non-OLE) to messages.
        /// </summary>
        STORE_ATTACH_OK = 0x00000020,

        /// <summary>
        ///     The message store supports categorized views of tables.
        /// </summary>
        STORE_CATEGORIZE_OK = 0x00000400,

        /// <summary>
        ///     The message store supports creation of new messages.
        /// </summary>
        STORE_CREATE_OK = 0x00000010,

        /// <summary>
        ///     Entry identifiers for the objects in the message store are unique, that is, never reused during the life of the
        ///     store.
        /// </summary>
        STORE_ENTRYID_UNIQUE = 0x00000001,

        /// <summary>
        ///     The message store supports HTML messages, stored in the <see cref="PropertyTags.PR_HTML" /> (PidTagBodyHtml) 
        ///     property. Note that STORE_HTML_OK is not defined in versions of MAPIDEFS.H that are included with Microsoft Exchange 
        ///     2000 Server and earlier. If your development environment uses a MAPIDEFS.H file that does not include STORE_HTML_OK, 
        ///     use the value 0x00010000 instead.
        /// </summary>
        STORE_HTML_OK = 0x00010000,

        /// <summary>
        ///     In a wrapped PST store, indicates that when a new message arrives at the store, the store does rules and spam
        ///     filter processing on the message separately. The store calls IMAPISupport::Notify, setting fnevNewMail in the
        ///     NOTIFICATION structure that is passed as a parameter, and then passes the details of the new message to the
        ///     listening client. Subsequently, when the listening client receives the notification, it does not process rules on
        ///     the message.
        /// </summary>
        STORE_ITEMPROC = 0x00200000,

        /// <summary>
        ///     This flag is reserved and should not be used.
        /// </summary>
        STORE_LOCALSTORE = 0x00080000,

        /// <summary>
        ///     The message store supports modification of its existing messages.
        /// </summary>
        STORE_MODIFY_OK = 0x00000008,

        /// <summary>
        ///     The message store supports multivalued properties, guarantees the stability of value order in a multivalued
        ///     property throughout a save operation, and supports instantiation of multivalued properties in tables.
        /// </summary>
        STORE_MV_PROPS_OK = 0x00000200,

        /// <summary>
        ///     The message store supports notifications.
        /// </summary>
        STORE_NOTIFY_OK = 0x00000100,

        /// <summary>
        ///     The message store supports OLE attachments. The OLE data is accessible through an IStorage interface, such as that
        ///     available through the PR_ATTACH_DATA_OBJ (PidTagAttachDataObject) property
        /// </summary>
        STORE_OLE_OK = 0x00000040,

        /// <summary>
        ///     The folders in this store are public (multi-user), not private (possibly multi-instance but not multi-user).
        /// </summary>
        STORE_PUBLIC_FOLDERS = 0x00004000,

        /// <summary>
        ///     The MAPI Protocol Handler will not crawl the store, and the store is responsible to push any changes through
        ///     notifications to the indexer to have messages indexed.
        /// </summary>
        STORE_PUSHER_OK = 0x00800000,

        /// <summary>
        ///     All interfaces for the message store have a read-only access level.
        /// </summary>
        STORE_READONLY = 0x00000002,

        /// <summary>
        ///     The message store supports restrictions.
        /// </summary>
        STORE_RESTRICTION_OK = 0x00001000,

        /// <summary>
        ///     The message store supports Rich Text Format (RTF) messages, usually compressed, and the store itself keeps 
        ///     <see cref="PropertyTags.PR_BODY_W" /> and <see cref="PropertyTags.PR_RTF_COMPRESSED" /> synchronized.
        /// </summary>
        STORE_RTF_OK = 0x00000800,

        /// <summary>
        ///     The message store supports search-results folders.
        /// </summary>
        STORE_SEARCH_OK = 0x00000004,

        /// <summary>
        ///     The message store supports sorting views of tables.
        /// </summary>
        STORE_SORT_OK = 0x00002000,

        /// <summary>
        ///     The message store supports marking a message for submission.
        /// </summary>
        STORE_SUBMIT_OK = 0x00000080,

        /// <summary>
        ///     The message store supports storage of RTF messages in uncompressed form. An uncompressed RTF stream is identified
        ///     by the value dwMagicUncompressedRTF in the stream header. The dwMagicUncompressedRTF value is defined in the
        ///     RTFLIB.H file
        /// </summary>
        STORE_UNCOMPRESSED_RTF = 0x00008000,

        /// <summary>
        ///     The message store supports properties containing Unicode characters.
        /// </summary>
        STORE_UNICODE_OK = 0x00040000
    }
}