using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dk.CctalkLib.Devices
{
    public enum CctalkCommands
    {
        SimplePoll = 254,
        AddressPoll = 253,
        AddressClash = 252,
        AddressChange = 251,
        AddressRandom = 250,
        RequestPollingPriority = 249,
        RequestStatus = 248,
        RequestManufacturerId = 246,
        RequestEquipmentCategoryId = 245,
        RequestProductCode = 244,
        RequestDatabaseVersion = 243,
        RequestSerialNumber = 242,
        RequestSoftwareVersion = 241,
        TestSolenoids = 240,
        TestOutputLines = 238,
        ReadInputLines = 237,
        ReadOptoStates = 236,
        LatchesOutputLines = 233,
        PerformSelfCheck = 232,
        ModifyInhibitStatus = 231,
        RequestInhibitStatus = 230,
        RequestBufferedCreditOrErrorCodes = 229,
        ModifyMasterInhibitStatus = 228,
        RequestMasterInhibitStatus = 227,
        ModifySorterPath = 210,
        RequestSorterPath = 209,
        CalculateRomChecksum = 197,
        RequestCreationDate = 196,
        RequestLastMdificationDate = 195,
        RequestBuildCode = 192,
        RequestCoinId = 184,
        RequestBaseYear = 170,
        RequestAddressMode = 169,

        ReadBufferedBillEvents = 159,
        ModifyBillOperatingMode = 153,

        Nak = 5,
        RequestCommsRevision = 4,
        ClearCommsStatusvariables = 3,
        RequestCommsStatusVariables = 2,
        ResetDevice = 1,

        None = 0,
    }
}
