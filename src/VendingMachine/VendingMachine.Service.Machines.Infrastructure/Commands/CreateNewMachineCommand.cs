﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Infrastructure.Commands
{
    public class CreateNewMachineCommand: IRequest<int>
    {
        public string MachineName { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public bool? Status { get; set; }
    }
}
