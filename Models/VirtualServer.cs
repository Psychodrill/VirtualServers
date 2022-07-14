﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtualServers.Models
{
    public class VirtualServer
    {
        public int? VirtualServerId { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? RemoveDateTime { get; set; }

        public bool SelectedForRemove { get; set; }

        public void Refresh()
        {
            this.RemoveDateTime = DateTime.Now;
        }
    }
}
