namespace EmailSummarizer.Console;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

public class GoogleDriveUploader
{
	private readonly string[] _scopes = { DriveService.Scope.DriveFile };
	private readonly string _applicationName = "EmailSummaryUploader";
	private readonly string _credentialsPath = "credentials.json";
	private readonly string _tokenPath = "token.json";

	public string UploadFile(string filePath)
	{
		UserCredential credential;

		using (var stream = new FileStream(_credentialsPath, FileMode.Open, FileAccess.Read))
		{
			credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				GoogleClientSecrets.FromStream(stream).Secrets,
				_scopes,
				"user",
				CancellationToken.None,
				new FileDataStore(_tokenPath, true)).Result;
		}

		var service = new DriveService(new BaseClientService.Initializer()
		{
			HttpClientInitializer = credential,
			ApplicationName = _applicationName,
		});

		var fileMetadata = new Google.Apis.Drive.v3.Data.File()
		{
			Name = Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(filePath)
		};

		FilesResource.CreateMediaUpload request;
		using (var stream = new FileStream(filePath, FileMode.Open))
		{
			request = service.Files.Create(fileMetadata, stream, "text/plain");
			request.Upload();
		}

		return request.ResponseBody.Id;
	}
}