using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Domain.Entities
{
    public class Desk
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int WorkSpaceItemId { get; set; }
        public Workspace? WorkspaceItem { get; set; }

    }
}
