using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Statistics.Infrastructure.Entities
{
    public class CallInfo
    {
        [Key]
        public int Id { get; private set; }
        public int Count { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime BeginDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }

        public CallInfo(int count, DateTime beginDateTime, DateTime endDateTime)
        {
            Count = count;
            CreatedAt = DateTime.UtcNow;
            BeginDateTime = beginDateTime;
            EndDateTime = endDateTime;
        }
        private CallInfo()
        {
            //ef core ctor
        }
    }
}
