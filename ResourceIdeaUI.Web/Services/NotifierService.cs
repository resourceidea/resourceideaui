using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceIdeaUI.Web.Services
{
    public class NotifierService
    {
        private readonly List<string> values = new List<string>();
        public IReadOnlyList<string> ValuesList => values;

        public NotifierService()
        {
        }

        public event Func<Task> Notify;

        public async Task AddToList(string value)
        {
            values.Add(value);
            if(Notify != null)
            {
                await Notify?.Invoke();
            }
        }
    }
}
