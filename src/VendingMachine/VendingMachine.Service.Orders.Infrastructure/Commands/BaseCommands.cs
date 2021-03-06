﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Orders.Infrastructure.Commands
{
    public class OrderProductItemBaseCommand
    {
        public int ProductItemId { get; set; }
    }

    public class OrderProductItemCommand : OrderProductItemBaseCommand
    {
        public GrossPriceCommand Price { get; set; }
    }

    public class GrossPriceCommand
    {
        public decimal GrossPrice { get; set; }
        public int TaxPercentage { get; set; }
    }

    public class MachineStatusCommand
    {
        public int MachineId { get; set; }
        public decimal CoinsCurrentSupply { get; set; }
    }
}
