using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.Passenger;
using Eqstra.VehicleInspection.UILogic.VIService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqstra.VehicleInspection.UILogic.ServiceAdapter
{
    public class ServiceAdapter
    {
        public ObservableCollection<MzkAccessoryContract> CreateAccessoriesForService()
        {
            ObservableCollection<MzkAccessoryContract> mzkAccessoryContractOC = new ObservableCollection<MzkAccessoryContract>();
            var pAccessories = SqliteHelper.Storage.LoadTableAsync<PAccessories>();
            foreach (var accessories in pAccessories.Result)
            {
                MzkAccessoryContract mzkAccessoryContract = new MzkAccessoryContract();
                mzkAccessoryContract.parmRecID = 120; // randome data
                mzkAccessoryContract.parmTableId = 120;//randome data
                mzkAccessoryContract.parmVehicleInsRecID = 120;// randome data

                if (accessories.HasAircon)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.Aircon;
                    mzkAccessoryContract.parmComments = accessories.AirconComment;
                    if (accessories.IsAirconDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();

                }
                if (accessories.HasAlarm)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.Alarm;
                    mzkAccessoryContract.parmComments = accessories.AlarmComment;
                    if (accessories.IsAlarmDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();
                }
                if (accessories.HasCDShuffle)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.CDShuttle;
                    mzkAccessoryContract.parmComments = accessories.CDShuffleComment;
                    if (accessories.IsCDShuffleDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();

                }
                if (accessories.HasKey)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.Key;
                    mzkAccessoryContract.parmComments = accessories.KeyComment;
                    if (accessories.IsKeyDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();
                }
                if (accessories.HasNavigation)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.Navigation;
                    mzkAccessoryContract.parmComments = accessories.NavigationComment;
                    if (accessories.IsNavigationDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();
                }
                if (accessories.HasRadio)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.Radio;
                    mzkAccessoryContract.parmComments = accessories.RadioComment;
                    if (accessories.IsRadioDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();
                }
                if (accessories.HasSpareKeys)
                {
                    mzkAccessoryContract.parmAccessoryType = MzkPassengerAccessories.SpareKey;
                    mzkAccessoryContract.parmComments = accessories.SpareKeysComment;
                    if (accessories.IsSpareTyreDmg)
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.Yes;
                    }
                    else
                    {
                        mzkAccessoryContract.parmIsDamaged = NoYes.No;
                    }
                    mzkAccessoryContractOC.Add(mzkAccessoryContract);
                    mzkAccessoryContract = new MzkAccessoryContract();
                }
            }

            return mzkAccessoryContractOC;
        }
    }
}
