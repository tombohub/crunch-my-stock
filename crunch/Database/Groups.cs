using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crunch.Database
{
    class Groups
    {
        public void Select()
        {
            using (var context = new Models.Context())
            {
                var groups = context.Groups.ToList();
                groups.ForEach(group => Console.WriteLine($" amama {group.Industry}"));
            }
        }
    }
}
