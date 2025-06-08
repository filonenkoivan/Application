using Co_Working.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Domain.Interfaces
{
    public interface IBookable
    {
        int Id { get; set; }
        int Quantity { get; set; }
        int WorkSpaceItemId { get; set; }
        Workspace? WorkspaceItem { get; set; }
    }
}
