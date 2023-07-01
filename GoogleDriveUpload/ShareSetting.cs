using Newtonsoft.Json;

namespace GoogleDrive
{
    [JsonObject]
    public record ShareSetting
    {
        public readonly string Token_ID;
        public readonly string Token_PW;
        public readonly string Project_Name;
        public readonly string ApiToken_SavePath;

        public bool CheckParam() =>
            !string.IsNullOrEmpty(Token_ID) &&
            !string.IsNullOrEmpty(Token_PW) &&
            !string.IsNullOrEmpty(Project_Name) &&
            !string.IsNullOrEmpty(ApiToken_SavePath);
    }
}