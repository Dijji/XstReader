// Project site: https://github.com/iluvadev/XstReader
//
// Based on the great work of Dijji. 
// Original project: https://github.com/dijji/XstReader
//
// Issues: https://github.com/iluvadev/XstReader/issues
// License (Ms-PL): https://github.com/iluvadev/XstReader/blob/master/license.md
//
// Copyright (c) 2021, iluvadev, and released under Ms-PL License.


// This file was generated automatically on 2022-01-10 10:28 +01:00 from the documentation at https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/

using System;

namespace XstReader.ItemProperties
{
    /// <summary>
    /// Areas for the properties defined in 
    /// <see href="https://docs.microsoft.com/en-us/openspecs/exchange_server_protocols/ms-oxprops/../ms-oxcfold/c0f31b95-c07f-486c-98d9-535ed9705fbf">
    /// [MS-OXCFOLD]</see>
    /// </summary>
    public enum PropertyArea
    {

        /// <summary> Contact Properties </summary>
        [FriendlyName("Contact Properties")]
        ContactProperties,

        /// <summary> Common </summary>
        [FriendlyName("Common")]
        Common,

        /// <summary> Meetings </summary>
        [FriendlyName("Meetings")]
        Meetings,

        /// <summary> Conferencing </summary>
        [FriendlyName("Conferencing")]
        Conferencing,

        /// <summary> Calendar </summary>
        [FriendlyName("Calendar")]
        Calendar,

        /// <summary> General Message Properties </summary>
        [FriendlyName("General Message Properties")]
        GeneralMessageProperties,

        /// <summary> Conversation Actions </summary>
        [FriendlyName("Conversation Actions")]
        ConversationActions,

        /// <summary> Flagging </summary>
        [FriendlyName("Flagging")]
        Flagging,

        /// <summary> Tasks </summary>
        [FriendlyName("Tasks")]
        Tasks,

        /// <summary> Journal </summary>
        [FriendlyName("Journal")]
        Journal,

        /// <summary> Sticky Notes </summary>
        [FriendlyName("Sticky Notes")]
        StickyNotes,

        /// <summary> Site Mailbox </summary>
        [FriendlyName("Site Mailbox")]
        SiteMailbox,

        /// <summary> R S S </summary>
        [FriendlyName("R S S")]
        Rss,

        /// <summary> Meeting Response </summary>
        [FriendlyName("Meeting Response")]
        MeetingResponse,

        /// <summary> Reminders </summary>
        [FriendlyName("Reminders")]
        Reminders,

        /// <summary> Run-time configuration </summary>
        [FriendlyName("Run-time configuration")]
        RunTimeConfiguration,

        /// <summary> Sharing </summary>
        [FriendlyName("Sharing")]
        Sharing,

        /// <summary> Spam </summary>
        [FriendlyName("Spam")]
        Spam,

        /// <summary> Access Control Properties </summary>
        [FriendlyName("Access Control Properties")]
        AccessControlProperties,

        /// <summary> Address Book </summary>
        [FriendlyName("Address Book")]
        AddressBook,

        /// <summary> Outlook Application </summary>
        [FriendlyName("Outlook Application")]
        OutlookApplication,

        /// <summary> Provider Defined Non Transmittable </summary>
        [FriendlyName("Provider Defined Non Transmittable")]
        ProviderDefinedNonTransmittable,

        /// <summary> Address Properties </summary>
        [FriendlyName("Address Properties")]
        AddressProperties,

        /// <summary> Archive </summary>
        [FriendlyName("Archive")]
        Archive,

        /// <summary> Sync </summary>
        [FriendlyName("Sync")]
        Sync,

        /// <summary> Message Attachment Properties </summary>
        [FriendlyName("Message Attachment Properties")]
        MessageAttachmentProperties,

        /// <summary> General Report Properties </summary>
        [FriendlyName("General Report Properties")]
        GeneralReportProperties,

        /// <summary> Email </summary>
        [FriendlyName("Email")]
        Email,

        /// <summary> Secure Messaging Properties </summary>
        [FriendlyName("Secure Messaging Properties")]
        SecureMessagingProperties,

        /// <summary> Exchange </summary>
        [FriendlyName("Exchange")]
        Exchange,

        /// <summary> MIME Properties </summary>
        [FriendlyName("MIME Properties")]
        MimeProperties,

        /// <summary> Unified Messaging </summary>
        [FriendlyName("Unified Messaging")]
        UnifiedMessaging,

        /// <summary> History Properties </summary>
        [FriendlyName("History Properties")]
        HistoryProperties,

        /// <summary> Server-side Rules Properties </summary>
        [FriendlyName("Server-side Rules Properties")]
        ServerSideRulesProperties,

        /// <summary> Message Time Properties </summary>
        [FriendlyName("Message Time Properties")]
        MessageTimeProperties,

        /// <summary> Exchange Profile Configuration </summary>
        [FriendlyName("Exchange Profile Configuration")]
        ExchangeProfileConfiguration,

        /// <summary> I C S </summary>
        [FriendlyName("I C S")]
        Ics,

        /// <summary> Container Properties </summary>
        [FriendlyName("Container Properties")]
        ContainerProperties,

        /// <summary> Folder Properties </summary>
        [FriendlyName("Folder Properties")]
        FolderProperties,

        /// <summary> Conversations </summary>
        [FriendlyName("Conversations")]
        Conversations,

        /// <summary> ID Properties </summary>
        [FriendlyName("ID Properties")]
        IdProperties,

        /// <summary> Mapi Container </summary>
        [FriendlyName("Mapi Container")]
        MapiContainer,

        /// <summary> Mapi Envelope </summary>
        [FriendlyName("Mapi Envelope")]
        MapiEnvelope,

        /// <summary> Mapi Status </summary>
        [FriendlyName("Mapi Status")]
        MapiStatus,

        /// <summary> Message Class Defined Transmittable </summary>
        [FriendlyName("Message Class Defined Transmittable")]
        MessageClassDefinedTransmittable,

        /// <summary> Mapi Non Transmittable </summary>
        [FriendlyName("Mapi Non Transmittable")]
        MapiNonTransmittable,

        /// <summary> Server </summary>
        [FriendlyName("Server")]
        Server,

        /// <summary> Exchange Folder </summary>
        [FriendlyName("Exchange Folder")]
        ExchangeFolder,

        /// <summary> Mapi Mail User </summary>
        [FriendlyName("Mapi Mail User")]
        MapiMailUser,

        /// <summary> Mapi Common </summary>
        [FriendlyName("Mapi Common")]
        MapiCommon,

        /// <summary> Message Properties </summary>
        [FriendlyName("Message Properties")]
        MessageProperties,

        /// <summary> Mapi Address Book </summary>
        [FriendlyName("Mapi Address Book")]
        MapiAddressBook,

        /// <summary> MapiEnvelope Property set </summary>
        [FriendlyName("MapiEnvelope Property set")]
        MapiEnvelopePropertySet,

        /// <summary> Message Class Defined Non Transmittable </summary>
        [FriendlyName("Message Class Defined Non Transmittable")]
        MessageClassDefinedNonTransmittable,

        /// <summary> Calendar Document </summary>
        [FriendlyName("Calendar Document")]
        CalendarDocument,

        /// <summary> Rules </summary>
        [FriendlyName("Rules")]
        Rules,

        /// <summary> Miscellaneous Properties </summary>
        [FriendlyName("Miscellaneous Properties")]
        MiscellaneousProperties,

        /// <summary> Exchange Administrative </summary>
        [FriendlyName("Exchange Administrative")]
        ExchangeAdministrative,

        /// <summary> Ren Message Folder </summary>
        [FriendlyName("Ren Message Folder")]
        RenMessageFolder,

        /// <summary> Free/Busy Properties </summary>
        [FriendlyName("Free/Busy Properties")]
        FreeBusyProperties,

        /// <summary> Message Attachment Properties Property set </summary>
        [FriendlyName("Message Attachment Properties Property set")]
        MessageAttachmentPropertiesPropertySet,

        /// <summary> Exchange Message Read Only </summary>
        [FriendlyName("Exchange Message Read Only")]
        ExchangeMessageReadOnly,

        /// <summary> Transport Envelope </summary>
        [FriendlyName("Transport Envelope")]
        TransportEnvelope,

        /// <summary> Calendar Property set </summary>
        [FriendlyName("Calendar Property set")]
        CalendarPropertySet,

        /// <summary> Conflict Note </summary>
        [FriendlyName("Conflict Note")]
        ConflictNote,

        /// <summary> MAPI Display Tables </summary>
        [FriendlyName("MAPI Display Tables")]
        MapiDisplayTables,

        /// <summary> Table Properties </summary>
        [FriendlyName("Table Properties")]
        TableProperties,

        /// <summary> Message Store Properties </summary>
        [FriendlyName("Message Store Properties")]
        MessageStoreProperties,

        /// <summary> Common Property set </summary>
        [FriendlyName("Common Property set")]
        CommonPropertySet,

        /// <summary> Best Body </summary>
        [FriendlyName("Best Body")]
        BestBody,

        /// <summary> Offline Address Book Properties </summary>
        [FriendlyName("Offline Address Book Properties")]
        OfflineAddressBookProperties,

        /// <summary> Mail </summary>
        [FriendlyName("Mail")]
        Mail,

        /// <summary> Appointment </summary>
        [FriendlyName("Appointment")]
        Appointment,

        /// <summary> Exchange Non Transmittable Reserved </summary>
        [FriendlyName("Exchange Non Transmittable Reserved")]
        ExchangeNonTransmittableReserved,

        /// <summary> MapiNonTransmittable Property set </summary>
        [FriendlyName("MapiNonTransmittable Property set")]
        MapiNonTransmittablePropertySet,

        /// <summary> Transport Recipient </summary>
        [FriendlyName("Transport Recipient")]
        TransportRecipient,

        /// <summary> Mapi Recipient </summary>
        [FriendlyName("Mapi Recipient")]
        MapiRecipient,

        /// <summary> Mapi Attachment </summary>
        [FriendlyName("Mapi Attachment")]
        MapiAttachment,

        /// <summary> Mapi Message </summary>
        [FriendlyName("Mapi Message")]
        MapiMessage,

        /// <summary> Configuration </summary>
        [FriendlyName("Configuration")]
        Configuration,

        /// <summary> Exchange Message Store </summary>
        [FriendlyName("Exchange Message Store")]
        ExchangeMessageStore,

        /// <summary> Search </summary>
        [FriendlyName("Search")]
        Search,

        /// <summary> AB Container </summary>
        [FriendlyName("AB Container")]
        AbContainer,

        /// <summary> Logon Properties </summary>
        [FriendlyName("Logon Properties")]
        LogonProperties,

        /// <summary> Mapi Message Store </summary>
        [FriendlyName("Mapi Message Store")]
        MapiMessageStore,

    }
}
