﻿using Co_Working.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Co_Working.Domain.Entities
{
    public class Room : IBookable
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public int Quantity { get; set; }

        public int WorkSpaceItemId { get; set; }
        public Workspace? WorkspaceItem { get; set; }
    }
}
