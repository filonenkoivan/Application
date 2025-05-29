using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public int Capacity { get; set; }

        public int WorkSpaceItemId { get; set; }
        public Workspace? WorkspaceItem { get; set; }
    }
}
