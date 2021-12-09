// Copyright (c) 2016, Dijji, and released under Ms-PL.  This can be found in the root of this distribution. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XstReader
{
    class NamedProperties
    {
        public byte[] StreamGuid;
        public byte[] StreamEntry;
        public byte[] StreamString;
        private NAMEID[] Entries;

        private class PropSet
        {
            public string GuidName;
            public string Guid;
            public Dictionary<UInt32, string> PropDesc = null;
        }

        private static Dictionary<string, PropSet> GuidToPropertySet = new Dictionary<string, PropSet>
        {
            { "00062008-0000-0000-c000-000000000046",
              new PropSet
              {
                  GuidName = "PSETID_Common",
                  PropDesc = new Dictionary<uint, string>
                  {
                      {0x00008501,  "ReminderDelta" },
                      {0x00008502,  "ReminderTime" },
                      {0x00008503,  "ReminderSet" },
                      {0x00008506,  "Private" },
                      {0x0000850e,  "AgingDontAgeMe" },
                      {0x00008510,  "SideEffects" },
                      {0x00008514,  "SmartNoAttach" },
                      {0x00008516,  "CommonStart" },
                      {0x00008517,  "CommonEnd" },
                      {0x00008518,  "TaskMode" },
                      {0x00008520,  "VerbStream" },
                      {0x00008530,  "FlagRequest" },
                      {0x00008552,  "CurrentVersion" },
                      {0x00008554,  "CurrentVersionName" },
                      {0x00008560,  "ReminderSignalTime" },
                      {0x00008578,  "undocumented (8578)" },
                      {0x00008580,  "InternetAccountName" },
                      {0x00008581,  "InternetAccountStamp" },
                      {0x00008582,  "UseTnef" },
                      {0x00008586,  "ContactLinkName" },
                      {0x0000858f,  "undocumented (858f)" },
                      {0x00008596,  "undocumented (8596)" },
                      {0x000085bf,  "ValidFlagStringProof" },
                      {0x000085a0,  "ToDoOrdinalDate" },
                      {0x000085a1,  "ToDoSubOrdinal" },
                      {0x000085a4,  "ToDoTitle" },
                      {0x000085d7,  "undocumented (85d7)" },
                      {0x000085d8,  "undocumented (85d8)" },
                  }
              }
            },
            { "00062002-0000-0000-c000-000000000046",
              new PropSet
              {
                  GuidName = "PSETID_Appointment",
                  PropDesc = new Dictionary<uint, string>
                  {
                      {0x00008200,  "undocumented (8200)" },
                      {0x00008201,  "AppointmentSequence" },
                      {0x00008203,  "AppointmentLastSequence" },
                      {0x00008205,  "BusyStatus" },
                      {0x00008207,  "AppointmentAuxiliaryFlags" },
                      {0x00008208,  "Location" },
                      {0x0000820d,  "AppointmentStartWhole" },
                      {0x0000820e,  "AppointmentEndWhole" },
                      {0x00008213,  "AppointmentDuration" },
                      {0x00008214,  "AppointmentColor" },
                      {0x00008215,  "AppointmentSubType" },
                      {0x00008217,  "AppointmentStateFlags" },
                      {0x00008218,  "ResponseStatus" },
                      {0x00008223,  "Recurring" },
                      {0x00008224,  "IntendedBusyStatus" },
                      {0x00008229,  "FInvited" },
                      {0x00008231,  "RecurrenceType" },
                      {0x00008232,  "RecurrencePattern" },
                      {0x00008234,  "TimeZoneDescription" },
                      {0x00008235,  "ClipStart" },
                      {0x00008236,  "ClipEnd" },
                      {0x0000823a,  "AutoFillLocation" },
                      {0x00008241,  "ConferencingType" },
                      {0x00008242,  "Directory" },
                      {0x00008243,  "OrganizerAlias" },
                      {0x00008245,  "undocumented (8245)" },
                      {0x00008247,  "CollaborateDoc" },
                      {0x00008248,  "NetShowUrl" },
                      {0x00008249,  "OnlinePassword" },
                      {0x00008256,  "AppointmentProposedDuration" },
                      {0x00008257,  "AppointmentCounterProposal" },
                      {0x00008259,  "AppointmentProposalNumber" },
                      {0x0000825a,  "AppointmentNotAllowPropose" },
                      {0x0000825e,  "AppointmentTimeZoneDefinitionStartDisplay" },
                      {0x0000825f,  "AppointmentTimeZoneDefinitionEndDisplay" },
                  }
              }
            },
            { "00062003-0000-0000-c000-000000000046",
              new PropSet
              {
                  GuidName = "PSETID_Task",
                  PropDesc = new Dictionary<uint, string>
                  {
                      {0x00008101,  "TaskStatus" },
                      {0x00008102,  "PercentComplete" },
                      {0x00008103,  "TeamTask" },
                      {0x00008104,  "TaskStartDate" },
                      {0x00008105,  "TaskDueDate" },
                      {0x0000810f,  "TaskDateCompleted" },
                      {0x00008110,  "TaskActualEffort" },
                      {0x00008111,  "TaskEstimatedEffort" },
                      {0x00008112,  "TaskVersion" },
                      {0x00008113,  "TaskState" },
                      {0x0000811c,  "TaskComplete" },
                      {0x00008121,  "TaskAssigner" },
                      {0x00008123,  "TaskOrdinal" },
                      {0x00008124,  "TaskNoCompute" },
                      {0x00008126,  "TaskFRecurring" },
                      {0x00008127,  "TaskRole" },
                      {0x00008129,  "TaskOwnership" },
                      {0x0000812a,  "TaskAcceptanceState" },
                      {0x0000812c,  "TaskFFixOffline" },
                  }
              }
            },
            { "00062004-0000-0000-c000-000000000046",
              new PropSet
              {
                  GuidName = "PSETID_Address",
                  PropDesc = new Dictionary<uint, string>
                  {
                      {0x00008005,  "FileUnder" },
                      {0x00008006,  "FileUnderId" },
                      {0x0000800e,  "undocumented (800e)" },
                      {0x00008010,  "Department" },
                      {0x00008015,  "HasPicture" },
                      {0x0000801a,  "HomeAddress" },
                      {0x0000801b,  "WorkAddress" },
                      {0x0000801c,  "OtherAddress" },
                      {0x00008023,  "ContactCharacterSet" },
                      {0x00008025,  "AutoLog" },
                      {0x00008026,  "FileUnderList" },
                      {0x00008027,  "undocumented (8027)" },
                      {0x00008028,  "AddressBookProviderEmailList" },
                      {0x00008029,  "AddressBookProviderArrayType" },
                      {0x00008045,  "WorkAddressStreet" },
                      {0x00008046,  "WorkAddressCity" },
                      {0x00008047,  "WorkAddressState" },
                      {0x00008048,  "WorkAddressPostalCode" },
                      {0x00008049,  "WorkAddressCountry" },
                      {0x0000804a,  "WorkAddressPostOfficeBox" },
                      {0x0000804c,  "DistributionListChecksum" },
                      {0x00008053,  "DistributionListName" },
                      {0x00008054,  "DistributionListOneOffMembers" },
                      {0x00008055,  "DistributionListMembers" },
                      {0x00008062,  "InstantMessagingAddress" },
                      {0x00008063,  "undocumented (8063)" },
                      {0x00008080,  "Email1DisplayName" },
                      {0x00008082,  "Email1AddressType" },
                      {0x00008083,  "Email1EmailAddress" },
                      {0x00008084,  "Email1OriginalDisplayName" },
                      {0x00008085,  "Email1OriginalEntryId" },
                      {0x00008090,  "Email2DisplayName" },
                      {0x00008092,  "Email2AddressType" },
                      {0x00008093,  "Email2EmailAddress" },
                      {0x00008094,  "Email2OriginalDisplayName" },
                      {0x00008095,  "Email2OriginalEntryId" },
                      {0x000080a0,  "Email3DisplayName" },
                      {0x000080a2,  "Email3AddressType" },
                      {0x000080a3,  "Email3EmailAddress" },
                      {0x000080a4,  "Email3OriginalDisplayName" },
                      {0x000080a5,  "Email3OriginalEntryId" },
                      {0x000080b2,  "Fax1AddressType" },
                      {0x000080b3,  "Fax1EmailAddress" },
                      {0x000080b4,  "Fax1OriginalDisplayName" },
                      {0x000080c2,  "Fax2AddressType" },
                      {0x000080c3,  "Fax2EmailAddress" },
                      {0x000080c4,  "Fax2OriginalDisplayName" },
                      {0x000080c5,  "Fax2OriginalEntryId" },
                      {0x000080d2,  "Fax3AddressType" },
                      {0x000080d3,  "Fax3EmailAddress" },
                      {0x000080d4,  "Fax3OriginalDisplayName" },
                      {0x000080dd,  "AddressCountryCode" },
                      {0x000080e0,  "IsContactLinked" },
                      {0x000080e6,  "ContactLinkGlobalAddressListLinkState" },
                      {0x000080eb,  "undocumented (80eb)" },
                  }
              }
            },
            { "6ed8da90-450b-101b-98da-00aa003f1305",
              new PropSet
              {
                  GuidName = "PSETID_Meeting",
                  PropDesc = new Dictionary<uint, string>
                  {
                      {0x00000002,  "Where" },
                      {0x00000003,  "GlobalObjectId" },
                      {0x00000005,  "IsRecurring" },
                      {0x0000000a,  "IsException" },
                      {0x0000001a,  "OwnerCriticalChange" },
                      {0x00000023,  "CleanGlobalObjectId" },
                      {0x00000024,  "AppointmentMessageClass" },
                  }
              }
            },
            { "00020386-0000-0000-c000-000000000046", new PropSet {GuidName = "PS_INTERNET_HEADERS" } },
            { "23239608-685d-4732-9c55-4c95cb4e8e33", new PropSet {GuidName = "PSETID_XmlExtractedEntities" } },
            { "41f28f13-83f4-4114-a584-eedb5a6b0bff", new PropSet {GuidName = "PSETID_Messaging" } },
        };

        private static PropSet[] IndexedPropertySets = new PropSet[]
        {
            new PropSet {GuidName = "PS_MAPI", Guid = "00020328-0000-0000-c000-000000000046" },
            new PropSet
            {
                GuidName = "PS_PUBLIC_STRINGS",
                Guid = "00020328-0000-0000-c000-000000000046",
                PropDesc = new Dictionary<uint, string>
                {
                    {0x00002328,  "Categories" },
                }
            }
        };

        public void LookupNPID(UInt16 npid, Property p)
        {
            if (Entries == null)
                Initialise();

            PropSet ps = null;

            try
            { 
                var entry = Entries.First(e => e.wPropIdx == npid - 0x8000);

                if (entry.wGuid == 1 || entry.wGuid == 2)
                {
                    ps = IndexedPropertySets[entry.wGuid - 1];
                    p.Guid = ps.Guid;
                    p.GuidName = ps.GuidName;
                }
                else if(entry.wGuid >= 3)
                {
                    var bytes = new byte[16];
                    Array.Copy(StreamGuid, (entry.wGuid - 3) * 16, bytes, 0, 16);
                    p.Guid = (new Guid(bytes)).ToString();
                   
                    if (GuidToPropertySet.TryGetValue(p.Guid, out ps))
                    {
                        p.GuidName = ps.GuidName;
                    }
                }

                if (entry.N)
                {
                    var len = Map.MapType<UInt32>(StreamString, (int)entry.dwPropertyID);
                    p.Name = Encoding.Unicode.GetString(StreamString, (int)entry.dwPropertyID + 4, (int)len);
                }
                else
                {
                    string name;
                    p.Lid = entry.dwPropertyID;
                    if (ps != null && ps.PropDesc != null && ps.PropDesc.TryGetValue(entry.dwPropertyID, out name))
                        p.Name = name;
                }
            }
            catch (InvalidOperationException)
            {
                // not found
                //name = "Named property not found";
            }
        }

        private void Initialise()
        {
            Entries = Map.MapArray<NAMEID>(StreamEntry, 0, StreamEntry.Length / Marshal.SizeOf(typeof(NAMEID)));
        }
    }
}
