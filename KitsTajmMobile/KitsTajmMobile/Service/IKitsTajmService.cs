using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KitsTajmMobile.Service
{
    public interface IKitsTajmService
    {
        Task<LoginResponse> Login(string username, string password);
        Task<IEnumerable<ProjectResponse>> GetProjects();
        Task<IEnumerable<GetTimeRecordsResponse>> GetTimeRecords(DateTime from, DateTime to);
        Task<PostNewTimeRecordResponse> PostNewTimeRecord(
            DateTime date,
            int time,
            ProjectResponse project,
            ProjectResponse.Activity activity);
        Task<PutTimeRecordResponse> PutTimeRecord(
            DateTime date,
            int time,
            ProjectResponse project,
            ProjectResponse.Activity activity,
            int recordId);
        Task<DeleteTimeRecordResponse> DeleteTimeRecord(DateTime date, int recordId);
    }
}
