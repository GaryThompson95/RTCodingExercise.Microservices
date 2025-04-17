using Catalog.Domain;
using MassTransit;
using System;

namespace WebMVC.UnitTests.Fakes
{
    internal class ResponseFake : Response<ConsumerResponse>
    {
        public ResponseFake(ConsumerResponse message)
        {
            Message = message;
        }

        public ConsumerResponse Message {get; set;}

        public Guid? MessageId => throw new NotImplementedException();

        public Guid? RequestId => throw new NotImplementedException();

        public Guid? CorrelationId => throw new NotImplementedException();

        public Guid? ConversationId => throw new NotImplementedException();

        public Guid? InitiatorId => throw new NotImplementedException();

        public DateTime? ExpirationTime => throw new NotImplementedException();

        public Uri SourceAddress => throw new NotImplementedException();

        public Uri DestinationAddress => throw new NotImplementedException();

        public Uri ResponseAddress => throw new NotImplementedException();

        public Uri FaultAddress => throw new NotImplementedException();

        public DateTime? SentTime => throw new NotImplementedException();

        public Headers Headers => throw new NotImplementedException();

        public HostInfo Host => throw new NotImplementedException();

        object Response.Message => Message;
    }
}
