using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KitsTajm.Helpers;
using KitsTajmMobile.Helpers;
using Newtonsoft.Json;

namespace KitsTajmMobile.Service
{
    public class KitsTajmService : IKitsTajmService
    {
        private static readonly Uri _webServiceServer = new Uri("https://kitstajm.kits.se");
        private static readonly Uri _webServiceAppRoot = new Uri(KitsTajmService._webServiceServer, "tajm/");
        private static readonly Uri _webServiceApiRoot = new Uri(KitsTajmService._webServiceAppRoot, "api/");
        private static readonly Uri _loginUri = new Uri(KitsTajmService._webServiceApiRoot, "login");
        private static readonly Uri _projectsUri = new Uri(KitsTajmService._webServiceApiRoot, "projects");
        private static readonly Uri _timeRecordsUriRoot = new Uri(KitsTajmService._webServiceApiRoot, "timerecords/");
        private static readonly Uri _timeRecordsUri = new Uri(KitsTajmService._webServiceApiRoot, "timerecords");

        private readonly HttpClient _client;
        private readonly JsonSerializer _serializer;

        private readonly AsyncLazy<IEnumerable<ProjectResponse>> _projects; 

        public KitsTajmService()
        {
            var cookies = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies
            };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Referrer = KitsTajmService._webServiceAppRoot;

            this._client = client;
            this._serializer = new JsonSerializer();

            this._projects = new AsyncLazy<IEnumerable<ProjectResponse>>((Func<Task<IEnumerable<ProjectResponse>>>)DoGetProjects);
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var data = new LoginRequest
            {
                Password = password,
                UserName = username
            };

            using (var content = GetJsonStringContent(data))
            {
                using (var response = await this._client.PostAsync(KitsTajmService._loginUri, content))
                {
                    var result = await ToResponse<LoginResponse>(response);

                    return result;
                }
            }
        }

        public async Task<IEnumerable<ProjectResponse>> GetProjects()
        {
            var projects = await this._projects;

            return projects;
        }

        private async Task<IEnumerable<ProjectResponse>> DoGetProjects()
        {
            using (var response = await this._client.GetAsync(KitsTajmService._projectsUri))
            {
                var projects = await ToResponse<IEnumerable<ProjectResponse>>(response);

                return projects;
            }
        }

        public async Task<IEnumerable<GetTimeRecordsResponse>> GetTimeRecords(DateTime from, DateTime to)
        {
            var uri = new Uri(
                KitsTajmService._timeRecordsUriRoot,
                $"{from:yyyy-MM-dd}/{to:yyyy-MM-dd}");

            using (var response = await this._client.GetAsync(uri))
            {
                var result = await ToResponse<IEnumerable<GetTimeRecordsResponse>>(response);

                return result;
            }
        }

        public async Task<PostNewTimeRecordResponse> PostNewTimeRecord(
            DateTime date, 
            int time,
            ProjectResponse project,
            ProjectResponse.Activity activity)
        {
            var data = new PostNewTimeRecordsRequest
            {
                ActivityId = activity.Id,
                ProjectId = project.Id,
                Date = date,
                RecordId = null,
                Time = time
            };

            using (var content = GetJsonStringContent(data))
            {
                using (var response = await this._client.PostAsync(KitsTajmService._timeRecordsUri, content))
                {
                    var result = await ToResponse<PostNewTimeRecordResponse>(response);

                    return result;
                }
            }
        }

        public async Task<PutTimeRecordResponse> PutTimeRecord(
            DateTime date,
            int time,
            ProjectResponse project,
            ProjectResponse.Activity activity,
            int recordId)
        {
            var data = new PutTimeRecordsRequest
            {
                ActivityId = activity.Id,
                ProjectId = project.Id,
                Date = date,
                RecordId = recordId,
                Time = time
            };

            using (var content = GetJsonStringContent(data))
            {
                var uri = new Uri(KitsTajmService._timeRecordsUriRoot, recordId.ToString());

                using (var response = await this._client.PutAsync(uri, content))
                {
                    var result = await ToResponse<PutTimeRecordResponse>(response);

                    return result;
                }
            }
        }

        public async Task<DeleteTimeRecordResponse> DeleteTimeRecord(DateTime date, int recordId)
        {
            var uri = new Uri(KitsTajmService._timeRecordsUriRoot, recordId.ToString());

            using (var response = await this._client.DeleteAsync(uri))
            {
                var result = await ToResponse<DeleteTimeRecordResponse>(response);

                return result;
            }
        }

        private async Task<T> ToResponse<T>(HttpResponseMessage response)
        {
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                using (var streamreader = new StreamReader(stream))
                {
                    using (var jsonreader = new JsonTextReader(streamreader))
                    {
                        var responseobject = this._serializer.Deserialize<T>(jsonreader);

                        return responseobject;
                    }
                }

            }
        }

        private static StringContent GetJsonStringContent<T>(T data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data))
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            };

            return content;
        }
    }
}
