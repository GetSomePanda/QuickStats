using System.Collections.Generic;
using System;
using System.Reflection;
using System.Data;
using System.Linq;
using UnityEngine;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Logging;
using Oxide.Core.Plugins;
 
namespace Oxide.Plugins
{
    [Info("QuickStats", "Panda", 1.0)]
    class QuickStats : RustPlugin
    {
               
		string steamID;
        string retrnUid;
		string playerName;
		string QuickStatsInfo = "";
               
        int woodCollected;
        int woodFromCFG;
        int woodSum;
        string stringedWood;
               
        int stonesCollected;
        int stonesFromCFG;
        int stonesSum;
        string stringedStones;
               
        private void OnServerInitialized()
        {
			PrintWarning("QuickStats has been fully initialized!");
        }
               
        protected override void LoadDefaultConfig()
        {
			Puts("Config not found, Loading up a new one!!");
			SaveConfig();
		}
               
        void OnPlayerInit(BasePlayer player)
		{
			steamID = player.userID.ToString();
			retrnUid = Config.Get<string>(steamID);
			playerName = player.displayName;
			woodCollected = 0;
			stonesCollected = 0;
			
			if(retrnUid == null)
			{
				Config[steamID] = playerName;
				Config[steamID + " Wood"] = 0;
				Config[steamID + " Stones"] = 0;
				SaveConfig();
			}
						   
			if(retrnUid != null)
			{
				//Update the players name in the config!
				if(retrnUid != playerName)
				{
					Config[steamID] = playerName;
					SaveConfig();
				}
				
				woodFromCFG = Config.Get<int>(steamID + " Wood");
				stonesFromCFG = Config.Get<int>(steamID + " Stones");
			}
		}
               
		void OnPlayerDisconnected(BasePlayer player)
		{
			steamID = player.userID.ToString();
			Puts("Disconnectedddddd");
			Config[steamID + " Wood"] = woodSum;
			Config[steamID + " Stones"] = stonesSum;
			SaveConfig();
		}
				   
		private void OnDispenserGather(ResourceDispenser dispenser, BaseEntity entity, Item item)
		{
			if((item.info.displayName.english) == "Wood")
			{
				Puts("Its Wood");
				woodCollected = woodCollected + item.amount;
				woodFromCFG = Config.Get<int>(steamID + " Wood");
				woodSum = woodCollected + woodFromCFG;
				stringedWood = woodSum.ToString();
			}
						   
			if((item.info.displayName.english) == "Stones")
			{
				Puts("Its Stones");
				stonesCollected = stonesCollected + item.amount;
				stonesFromCFG = Config.Get<int>(steamID + " Stones");
				stonesSum = stonesCollected + stonesFromCFG;
				stringedStones = stonesSum.ToString();
			}
		}
			   
		[ChatCommand("quickstats")]
		private void StatsCommand(BasePlayer player, string command, string[] args)
		{
			steamID = player.userID.ToString();
			woodFromCFG = Config.Get<int>(steamID + " Wood");
			woodSum = woodCollected + woodFromCFG;
			stringedWood = woodSum.ToString();	
			stonesFromCFG = Config.Get<int>(steamID + " Stones");
			stonesSum = stonesCollected + stonesFromCFG;
			stringedStones = stonesSum.ToString();
			SendReply((BasePlayer)player, "Wood: " + stringedWood + " Stones: " + stringedStones);
		}
	}
}