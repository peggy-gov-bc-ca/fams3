﻿namespace SearchApi.Web
{
    public class Keys
    {
        internal const string SEARCHAPI_SECTION_SETTING_KEY = "SearchApi";
        internal const string REDIS_SECTION_SETTING_KEY = "Redis";
        internal const string DATA_PARTNER_JSON_PATH = "$.DataPartners";

    }
    public class EventName
    {
        internal const string Accepted = "Accepted";
        internal const string Rejected = "Rejected";
        internal const string Failed = "Failed";
        internal const string Completed = "Completed";
        internal const string Finalized = "Finalized";
    }
}