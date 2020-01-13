using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VendingMachine.Service.Shared.Read;

namespace VendingMachine.Service.Machines.Read.Models
{
    public class MachineItemReadModel : IReadEntity
    {
        public int Id { get; set; }
        public MapPointReadModel Position { get; set; }
        public decimal? Temperature { get; set; }
        // Start or Stop or Not Set
        public bool? Status { get; set; }
        public decimal MoneyFromBirth { get; set; }
        public decimal MoneyMonth { get; set; }
        public decimal MoneyYear { get; set; }
        public DateTimeOffset? LatestLoadedProducts { get; set; }
        public DateTimeOffset? LatestCleaningMachine { get; set; }
        public DateTimeOffset? LatestMoneyCollection { get; set; }
        public decimal CoinsInMachine { get; set; }
        public decimal CoinsCurrentSupply { get; set; }
        public ICollection<ActiveProductReadModel> ActiveProducts { get; set; }

        public void FromDapper(MachineItemDapper d, MapPointReadModel position)
        {
            Id = d.Id;
            Temperature = d.Temperature;
            Status = d.Status;
            MoneyFromBirth = d.MoneyFromBirth;
            MoneyMonth = d.MoneyMonth;
            MoneyYear = d.MoneyYear;
            LatestLoadedProducts = d.LatestLoadedProducts;
            LatestCleaningMachine = d.LatestCleaningMachine;
            LatestMoneyCollection = d.LatestMoneyCollection;
            CoinsInMachine = d.CoinsInMachine;
            CoinsCurrentSupply = d.CoinsCurrentSupply;
            Position = position;
            ActiveProducts = new Collection<ActiveProductReadModel>();
        }
    }

    public class ActiveProductReadModel : ProductReadModel
    {
        public DateTimeOffset ActivationDate { get; set; }
    }


    public class MapPointReadModel : IReadEntity
    {
        public MapPointReadModel() { }
        public MapPointReadModel(double x, double y)
        {
            X = (decimal)X;
            Y = (decimal)y;
        }
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }



    public class MachineItemDapper
    {
        public int Id { get; set; }
        public SqlGeography Position { get; set; }
        public decimal? Temperature { get; set; }
        // Start or Stop or Not Set
        public bool? Status { get; set; }
        public decimal MoneyFromBirth { get; }
        public decimal MoneyMonth { get; }
        public decimal MoneyYear { get; }
        public DateTimeOffset? LatestLoadedProducts { get; private set; }
        public DateTimeOffset? LatestCleaningMachine { get; private set; }
        public DateTimeOffset? LatestMoneyCollection { get; private set; }
        public decimal CoinsInMachine { get; private set; }
        public decimal CoinsCurrentSupply { get; private set; }
    }
}
