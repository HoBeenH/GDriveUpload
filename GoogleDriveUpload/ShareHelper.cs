using System.IO;
using Google.Apis.Drive.v3;

namespace GoogleDrive
{
    public static class ShareHelper
    {
        // Share Config Path
        public static readonly string CONFIG_PATH = Path.Combine(Directory.GetCurrentDirectory(), "Config.json");
        
        // Google Drive Scope
        public static readonly string[] DRIVE_SCOPE = new[] { DriveService.Scope.Drive, DriveService.Scope.DriveMetadataReadonly, DriveService.Scope.DriveFile };
        
        // Share Link {0} == File ID
        public const string HTTP_SHARE_ADDRESS = "https://drive.google.com/file/d/{0}";

        public static class Role
        {
            public static readonly string Owner = "owner";
            public static readonly string Organizer = "organizer";
            public static readonly string FileOrganizer = "fileOrganizer";
            public static readonly string Writer = "writer";
            public static readonly string Commenter = "commenter";
            public static readonly string Reader = "reader";
        }
        
        public static class Type
        {
            public static readonly string User = "user";
            public static readonly string Group = "group";
            public static readonly string Domain = "domain";
            public static readonly string Anyone = "anyone";
        }
        
        public static class MimeType
        {
            public static readonly string XLS = "application/vnd.ms-excel";
            public static readonly string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public static readonly string XML = "text/xml";
            public static readonly string ODX = "application/vnd.oasis.opendocument.spreadsheet";
            public static readonly string CSV = "text/plain";
            public static readonly string Tmpl = "text/plain";
            public static readonly string PDF = "application/pdf";
            public static readonly string PHP = "application/x-httpd-php";
            public static readonly string JPG = "image/jpeg";
            public static readonly string PNG = "image/png";
            public static readonly string GIF = "image/gif";
            public static readonly string BMP = "image/bmp";
            public static readonly string TXT = "text/plain";
            public static readonly string DOC = "application/msword";
            public static readonly string JS = "text/js";
            public static readonly string SWF = "application/x-shockwave-flash";
            public static readonly string MP3 = "audio/mpeg";
            public static readonly string Zip = "application/zip";
            public static readonly string Rar = "application/rar";
            public static readonly string Tar = "application/tar";
            public static readonly string Arj = "application/arj";
            public static readonly string Cab = "application/cab";
            public static readonly string Html = "text/html";
            public static readonly string Htm = "text/html";
            public static readonly string Default = "application/octet-stream";
            public static readonly string Folder = "application/vnd.google-apps.folder";
        }
    }
}