using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Models
{
	public class Note
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string CreatedAt { get; set; }
	}
}
