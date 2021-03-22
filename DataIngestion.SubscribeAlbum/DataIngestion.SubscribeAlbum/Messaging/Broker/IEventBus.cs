using System.Threading.Tasks;

namespace DataIngestion.SubscribeAlbum.Broker
{
    public interface IEventBus
    {
        string DaprPort { get; set; }
        bool EventsOn { get; set; }
        string Broker { get; set; }

        /// <summary>
        /// publish a message to dapr runtime.
        /// </summary>
        /// <param name="event">the clr type defined in the Event project</param>
        /// <param name="eventname">the logical event name</param>
        /// <returns>IsSuccessStatusCode</returns>
        Task<bool> Publish(object @event, string eventName);
    }
}
