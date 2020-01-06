using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Service.Machines.Data.Seeds
{
    public static partial class MigrationExtension
    {
        public static MigrationBuilder _2(this MigrationBuilder builder)
        {
            //builder.InsertData("",
            //    new string[] { "" });

            //builder.UpdateData("",
            //    "Id",
            //    1,
                //    new[] { "DataCreated", "DataUpdated", "Position", "Temperature", "Status", "MachineTypeId" },
            //    new object[] { DateTime.UtcNow, null, SeedBase.SetPosition(45.4351m, 10.9988m), "2", true, 2 })
            

            return builder;
        }
    }
}
