namespace CZ.Worker.EmailSender.TemplateDomain.ServiceBus;

public class QueueEmailContent<T>
{
    public T Data { get; set; }
}

public class QueueEmailContent : QueueEmailContent<dynamic> { }