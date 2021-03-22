using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    public class EventBus : IEventBus
    {
        /// <summary>
        /// default 3500
        /// </summary>
        public string DaprPort { get; set; }
        public bool EventsOn { get; set; } //just in case we need to disable the events, no need to publish events during a simple debugging session.
        public string Broker { get; set; }

        private readonly HttpClient _httpClient; //we expect the client to provide this for us (every api will hand this to us)

        public EventBus(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        ///   publish data.
        /// </summary>
        /// <param name="event">object you want to publish</param>
        /// <param name="eventName">name of the event</param>
        /// <returns></returns>
        public async Task<bool> Publish(object @event, string eventName)
        {
            if (!EventsOn) return false;
            var jsonMessage = JsonConvert.SerializeObject(@event);
            string publisherUrl = $"http://localhost:{DaprPort}/v1.0/publish/{Broker}/{eventName}";
            var result = await _httpClient.PostAsync(publisherUrl,
                new StringContent(jsonMessage, Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }
    }
}
