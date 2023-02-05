using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPathFinding
{
    public class TimedEvent
    {
        public Action Action { get; set; }
        public DateTime ExecutionTime { get; set; }

        public TimedEvent(Action action, int delayInMs)
        {
            Action = action;
            ExecutionTime = DateTime.Now.AddMilliseconds(delayInMs);
        }
        public TimedEvent(Action action, DateTime executionTime)
        {
            Action = action;
            ExecutionTime = executionTime;
        }
    }
}
