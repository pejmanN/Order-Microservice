using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Domain.EventOutbox
{
    public class DomainEventItem
    {
        public long Id { get; set; }
        public Guid EventId { get; set; }
        public string Body { get; set; }
        public string Type { get; set; }
        public bool IsSent { get; set; }
        public DateTime PublishDateTime { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public void ChangeToSentState()
        {
            this.IsSent = true;
            this.ModifiedDate = DateTime.Now;
        }

        public object CreateMessage()
        {
            return JsonConvert.DeserializeObject(Body, System.Type.GetType(Type));
        }
    }



}
