using System;

namespace EventBus.Messages.Events
{
    public class IntergrationBaseEvent
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public IntergrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        public IntergrationBaseEvent(Guid id, DateTime creationDate)
        {
            Id = id;
            CreationDate = creationDate;
        }
    }
}
