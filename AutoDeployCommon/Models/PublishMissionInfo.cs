namespace Benlai.Application.AutoPublish.Common.Models
{
    public class PublishMissionInfo
    {
        public int MissionId { get; set; }

        public string MissionStep { get; set; }

        public bool IsSkipLocalBackup { get; set; }
    }
}
