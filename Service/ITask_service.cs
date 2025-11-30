using Luno_platform.Models;

namespace Luno_platform.Service
{
    public interface ITask_service : I_BaseService<Tasks>
    {
        List<Question> GetTaskbyid(int taskid);

        int CorrectTaskAndSave(int taskId, int studentId, Dictionary<int, string> answers);
    }
}
