using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using File = System.IO.File;

namespace GoogleDrive
{
    public class ShareFile
    {
        private static readonly string LogHeader = $"[{typeof(ShareFile)}] :: ";
        private static ShareSetting m_Setting = null;
        public static async Task Main(string[] args)
        {
            m_Setting = GetSetting();
            Log("Upload File");
            var _sb = new StringBuilder();
            string _appName;
            string _appFullPath;
            if (args == null || args.Length <= 0)
            {
                Log("Input \"{AppName} {App Full Path} {email} {email}...\"");

                using var _sr = new StreamReader(Console.OpenStandardInput());
                var _input = await _sr.ReadLineAsync();

                ThrowIfNullOrEmpty(_input);

                var _inputToArr = _input.Split();
                if (_inputToArr.Length <= 1)
                    throw new ArgumentException("The Param Is Less");
                    
                _appName = _inputToArr[0];
                _appFullPath = _inputToArr[1];
                for(var i = 2; i < _inputToArr.Length; i++)
                    _sb.Append(_inputToArr[i]);
            }
            else
            {
                _appName = args[0];
                _appFullPath = args[1];
                for (var i = 2; i < args.Length; i++)
                    _sb.Append(args[i]);
            }
            if (string.IsNullOrEmpty(_appName) || _sb.Length <= 0)
                throw new ArgumentException("The Param Error");

            Log($"App Name : {_appName}\n" +
                $"App Full Path {_appFullPath}\n" +
                $"Email = {_sb.ToString()}");
            var _path = await Get(_appName ,_appFullPath, _sb.ToString());
            Log($"The Path Is {_path}");
        }
        
        private static async Task<string> Get(string appName, string appFullPath, string emailAddress)
        {
            using var _s = await GetService();
            var _meta = new Google.Apis.Drive.v3.Data.File
            {
                Name = appName,
            };

            using var _fs = new FileStream(appFullPath, FileMode.Open);
            var _uploadRequest = _s.Files.Create(_meta, _fs, ShareHelper.MimeType.Default);
            _uploadRequest.Fields = "id";

            var _uploadResponse = await _uploadRequest.UploadAsync();

            if (_uploadResponse.Status != UploadStatus.Completed)
                throw _uploadResponse.Exception;

            var _id = _uploadRequest.ResponseBody?.Id;

            Log($"File ID Is {_id}");
            ThrowIfNullOrEmpty(_id);

            var _emailList = emailAddress.Split(',');

            foreach (var str in _emailList)
            {
                var _address = str.Trim();
                var _permission = new Permission
                {
                    EmailAddress = _address,
                    Role = ShareHelper.Role.Reader,
                    Type = ShareHelper.Type.User,
                    AllowFileDiscovery = null,
                };
                PermissionsResource.CreateRequest
                    _request = new PermissionsResource.CreateRequest(_s, _permission, _id);

                await _request.ExecuteAsync();
            }
            return string.Format(ShareHelper.HTTP_SHARE_ADDRESS, _id);
        }

        private static async Task<DriveService> GetService()
        {
            var _cs = new ClientSecrets
            {
                ClientId = m_Setting.Token_ID,
                ClientSecret = m_Setting.Token_PW,
            };
        
            var _tokenPath = m_Setting.ApiToken_SavePath;
            var _scope = ShareHelper.DRIVE_SCOPE;
        
            var _auth = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets: _cs,
                scopes: _scope,
                ShareHelper.Type.User,
                CancellationToken.None,
                new FileDataStore(_tokenPath, true));

            await _auth;
            
            if (_auth.IsCompleted)
            {
                var _service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = _auth.Result,
                    ApplicationName = m_Setting.Project_Name
                });
        
                return _service;
            }
            else
            {
                return null;
            }
        }

        private static void ThrowIfNullOrEmpty<T>(T obj) where T : class
        {
            switch (obj)
            {
                case string str when string.IsNullOrEmpty(str):
                    throw new ArgumentException($"[{typeof(ShareFile)}] {typeof(T).Name} Is Null Or Empty : {str}");
                case null:
                    throw new ArgumentException($"[{typeof(ShareFile)}] {typeof(T).Name} Is Null");
            }
        }

        private static ShareSetting GetSetting()
        {
            var _result = new ShareSetting();
            var _path = ShareHelper.CONFIG_PATH;
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, JsonConvert.SerializeObject(_result));
                throw new Exception("Can't Find Config Path");
            }

            var _settingToStr = File.ReadAllText(_path);
            _result = JsonConvert.DeserializeObject<ShareSetting>(_settingToStr);

            if (!_result.CheckParam())
            {
                throw new Exception("Can't Parse Setting");
            }
            return _result;
        }

        private static void Log(object msg) => Console.WriteLine($"{LogHeader} {msg}");
    }
}