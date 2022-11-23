using System;

namespace DailyNotebookApp.Models
{
    public class DateRange
    {
		private DateTime start;
        private DateTime end;

        public DateTime Start
		{
			get { return start; }
			private set { start = value; }
		}

		public DateTime End
		{
			get { return end; }
			private set { end = value; }
		}

		public DateRange(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}

		public bool Contains(DateTime dateTime)
		{
			return (start <= dateTime) && (dateTime <= end);
		}

        public override string ToString()
        {
            return Start.ToShortDateString() + " - " + End.ToShortDateString();
        }
    }
}
