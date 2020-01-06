using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VendingMachine.Service.Machines.Application.ViewModels
{
    public class AddCoinsViewModel
    {
        [Required]
        public decimal Coins { get; set; }

        [Required]
        public int MachineId { get; set; }
    }
}
