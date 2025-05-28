using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using xNetwork.Core;
using xNetwork;
using xNetwork.SDK;
using System.Diagnostics;
using oNetwork = xNetwork.oNetwork;
using GTANetworkMethods;

class RobberyNPC : Script
{

    public static TimerEx[] player_robbery_timer { get; set; } = new TimerEx[500];


    public static int MAX_ROBBERY_NPC = 0;
    public class RobberyNPCEnum : IEquatable<RobberyNPCEnum>
    {
        public int id { get; set; }

        public string name { get; set; }
        public string model { get; set; }
        public Vector3 position { get; set; }
        public Vector3 caixa_1 { get; set; }
        public Vector3 caixa_2 { get; set; }

        public float heading { get; set; }
        public int robbery_state { get; set; }
        public int money { get; set; }
        //public Client owned { get; set; }
        public int time_remaining { get; set; }
        public int lastfraction { get; set; }
        public DateTime time_vulnerable { get; set; }
        public int players_aiming { get; set; }

        public int cash_amount { get; set; }
        public bool activeintime { get; set; }
  


        public static void SetRandomActiveHeistBusiness(PlayerX player)
        {
            var rnd = new Random().Next(0, MAX_ROBBERY_NPC + 1);
            var rnd2 = new Random().Next(0, MAX_ROBBERY_NPC + 1);
            robbery_npc[rnd].activeintime = true;
        }
        public override int GetHashCode()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            RobberyNPCEnum objAsPart = obj as RobberyNPCEnum;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

        public bool Equals(RobberyNPCEnum other)
        {
            if (other == null) return false;
            return (this.id.Equals(other.id));
        }
    }
    public static List<RobberyNPCEnum> robbery_npc = new List<RobberyNPCEnum>();


    public static void CreateRobberyNPC(string name, string model, Vector3 position, float heading, Vector3 caixa_1, Vector3 caixa_2)
    {
        robbery_npc.Add(new RobberyNPCEnum { id = MAX_ROBBERY_NPC, name = name, model = model, position = position, heading = heading, robbery_state = 0, players_aiming = 0, caixa_1 = caixa_1, caixa_2 = caixa_2, activeintime = true});

        MAX_ROBBERY_NPC++;
    }

    public RobberyNPC()
    {
        CreateRobberyNPC("Магазин 24/7 #2", "mp_m_shopkeep_01", new Vector3(24.47675, -1347.312, 29.49702), 266.0985f, new Vector3(24.47675, -1347.312, 29.49702), new Vector3(24.47675, -1347.312, 29.49702));
        CreateRobberyNPC("Магазин 24/7 #15", "mp_m_shopkeep_01", new Vector3(-1221.522, -907.9908, 12.32635), 31f, new Vector3(-1221.522, -907.9908, 12.32635), new Vector3(-1221.522, -907.9908, 12.32635));
        CreateRobberyNPC("Магазин 24/7 #12", "mp_m_shopkeep_01", new Vector3(1698.427, 4922.392, 42.063), 328f, new Vector3(1698.427, 4922.392, 42.063), new Vector3(1698.427, 4922.392, 42.063));
        CreateRobberyNPC("Магазин 24/7 #1", "mp_m_shopkeep_01", new Vector3(-46.28725, -1757.315, 29.421), 61f, new Vector3(-46.28725, -1757.315, 29.421), new Vector3(-46.28725, -1757.315, 29.421));
        CreateRobberyNPC("Магазин 24/7 #8", "mp_m_shopkeep_01", new Vector3(1727.767, 6414.963, 35.03722), 217f, new Vector3(1727.767, 6414.963, 35.03722), new Vector3(1727.767, 6414.963, 35.03722));
        CreateRobberyNPC("Магазин 24/7 #9", "mp_m_shopkeep_01", new Vector3(2678.398, 3279.246, 55.24113), 328f, new Vector3(2678.398, 3279.246, 55.24113), new Vector3(2678.398, 3279.246, 55.24113));
        CreateRobberyNPC("Магазин 24/7 #7", "mp_m_shopkeep_01", new Vector3(-3038.612, 584.4379, 7.908929), 15f, new Vector3(-3038.612, 584.4379, 7.908929), new Vector3(-3038.612, 584.4379, 7.908929));
        CreateRobberyNPC("Магазин 24/7 #50", "mp_m_shopkeep_01", new Vector3(-1486.0565, -3777.71857, 40.16339), 135.95183f, new Vector3(-1486.0565, -3777.71857, 40.16339), new Vector3(-1486.0565, -3777.71857, 40.16339));
        //CreateRobberyNPC("Магазин 24/7 #15", "mp_m_shopkeep_01", new Vector3(1392.335, 3606.429, 34.98089), 191f, new Vector3(1392.335, 3606.429, 34.98089), new Vector3(1392.335, 3606.429, 34.98089));
        CreateRobberyNPC("Магазин 24/7 #16", "mp_m_shopkeep_01", new Vector3(-706.1262, -912.9778, 19.21559), 80f, new Vector3(-706.1262, -912.9778, 19.21559), new Vector3(-706.1262, -912.9778, 19.21559));
        CreateRobberyNPC("Магазин 24/7 #4", "mp_m_shopkeep_01", new Vector3(372.539, 326.2779, 103.5665), 248f, new Vector3(372.539, 326.2779, 103.5665), new Vector3(372.539, 326.2779, 103.5665));
        CreateRobberyNPC("Магазин 24/7 #3", "mp_m_shopkeep_01", new Vector3(1134.11, -981.9982, 46.41585), 275f, new Vector3(1134.11, -981.9982, 46.41585), new Vector3(1134.11, -981.9982, 46.41585));
        CreateRobberyNPC("Магазин 24/7 #5", "mp_m_shopkeep_01", new Vector3(1164.663, -322.4549, 69.20505), 96f, new Vector3(1164.663, -322.4549, 69.20505), new Vector3(1164.663, -322.4549, 69.20505));
        CreateRobberyNPC("Магазин 24/7 #14", "mp_m_shopkeep_01", new Vector3(1166.283, 2710.754, 38.15771), 173f, new Vector3(1166.283, 2710.754, 38.15771), new Vector3(1164.663, -322.4549, 69.20505));
        CreateRobberyNPC("Магазин 24/7 #11", "mp_m_shopkeep_01", new Vector3(1960.146, 3739.983, 32.34378), 292f, new Vector3(1960.146, 3739.983, 32.34378), new Vector3(1960.146, 3739.983, 32.34378));
        CreateRobberyNPC("Магазин 24/7 #6", "mp_m_shopkeep_01", new Vector3(-3242.17, 1000.008, 12.83071), 353f, new Vector3(-3242.17, 1000.008, 12.83071), new Vector3(-3242.17, 1000.008, 12.83071));
        CreateRobberyNPC("Магазин 24/7 #13", "mp_m_shopkeep_01", new Vector3(549.0481, 2671.355, 42.1564), 90f, new Vector3(549.0481, 2671.355, 42.1564), new Vector3(549.0481, 2671.355, 42.1564));
        //CreateRobberyNPC("Магазин 24/7 #15", "mp_m_shopkeep_01", new Vector3(-2966.496, 390.5234, 15.0433), 80f, new Vector3(-2966.496, 390.5234, 15.0433), new Vector3(-2966.496, 390.5234, 15.0433));
        CreateRobberyNPC("Магазин 24/7 #10", "mp_m_shopkeep_01", new Vector3(2557.383, 380.8376, 108.623), 0f, new Vector3(2557.383, 380.8376, 108.623), new Vector3(2557.383, 380.8376, 108.623));
        CreateRobberyNPC("Магазин 24/7 #18", "mp_m_shopkeep_01", new Vector3(-1485.5847, -378.04044, 40.163433), 40.163433f, new Vector3(-1485.5847, -378.04044, 40.163433), new Vector3(-1485.5847, -378.04044, 40.163433));
        CreateRobberyNPC("Магазин одежды #5", "csb_denise_friend", new Vector3(424.48166, -811.9351, 29.491123), -3.8326f, new Vector3(77.21005, -1387.452, 29.37612), new Vector3(77.21005, -1387.452, 29.37612));
        CreateRobberyNPC("Магазин одежды #6", "csb_denise_friend", new Vector3(77.21005, -1387.452, 29.37612), 172.7294f, new Vector3(77.21005, -1387.452, 29.37612), new Vector3(77.21005, -1387.452, 29.37612));
        CreateRobberyNPC("Магазин одежды #84", "csb_denise_friend", new Vector3(612.8331, 2763.936, 42.08815), 275f, new Vector3(612.8331, 2763.936, 42.08815), new Vector3(612.8331, 2763.936, 42.08815));
        CreateRobberyNPC("Магазин одежды #14", "csb_denise_friend", new Vector3(126.5512, -225.5727, 54.557), 63.66f, new Vector3(126.5512, -225.5727, 54.557), new Vector3(126.5512, -225.5727, 54.557));
        CreateRobberyNPC("Магазин одежды #15", "a_f_y_business_04", new Vector3(-165.0003, -302.8776, 39.73326), 248.5f, new Vector3(-165.0003, -302.8776, 39.73326), new Vector3(-165.0003, -302.8776, 39.73326));
        CreateRobberyNPC("Магазин одежды #16", "a_f_y_business_04", new Vector3(-708.8664, -151.8637, 37.41513), 114.6f, new Vector3(-708.8664, -151.8637, 37.41513), new Vector3(-708.8664, -151.8637, 37.41513));
        CreateRobberyNPC("Магазин одежды #112", "a_f_y_business_04", new Vector3(-3169.5605, 1042.2905, 20.863213), 59.880222f, new Vector3(-3169.5605, 1042.2905, 20.863213), new Vector3(-3169.5605, 1042.2905, 20.863213));
        CreateRobberyNPC("Магазин одежды #113", "a_f_y_business_04", new Vector3(-1193.4406, -766.2672, 17.31573), -140.2f, new Vector3(-1193.4406, -766.2672, 17.31573), new Vector3(-1193.4406, -766.2672, 17.31573));
        CreateRobberyNPC("Магазин одежды #114", "a_f_y_business_04", new Vector3(1202.1208, -2709.1174, 38.222595), 91.6f, new Vector3(1202.1208, -2709.1174, 38.222595), new Vector3(1202.1208, -2709.1174, 38.222595));
        CreateRobberyNPC("Магазин одежды #115", "a_f_y_business_04", new Vector3(0.11151973, 6509.3286, 31.87783), -49.5f, new Vector3(0.11151973, 6509.3286, 31.87783), new Vector3(0.11151973, 6509.3286, 31.87783));
        CreateRobberyNPC("Магазин одежды #116", "a_f_y_business_04", new Vector3(-817.019, -1072.2014, 11.3281), 120.1f, new Vector3(-817.019, -1072.2014, 11.3281), new Vector3(-817.019, -1072.2014, 11.3281));
        CreateRobberyNPC("Магазин одежды #117", "a_f_y_business_04", new Vector3(-1449.1342, -238.29189, 48.713423), 49f, new Vector3(-1449.1342, -238.29189, 48.713423), new Vector3(-1449.1342, -238.29189, 48.713423));
        CreateRobberyNPC("Аммуниция #10", "ig_old_man1a", new Vector3(841.3726, -1035.507, 28.19486), 341.3f, new Vector3(841.3726, -1035.507, 28.19486), new Vector3(841.3726, -1035.507, 28.19486));
        CreateRobberyNPC("Аммуниция #8", "ig_old_man1a", new Vector3(809.69183, -2159.0151, 29.618996), -0.8131886f, new Vector3(809.69183, -2159.0151, 29.618996), new Vector3(809.69183, -2159.0151, 29.618996));
        CreateRobberyNPC("Аммуниция #9", "ig_old_man1a", new Vector3(23.13156, -1105.3682, 29.797031), 150.88312f, new Vector3(23.13156, -1105.3682, 29.797031), new Vector3(23.13156, -1105.3682, 29.797031));
        CreateRobberyNPC("Аммуниция #47", "ig_old_man1a", new Vector3(-1118.505, 2700.2434, 18.554127), -131.67674f, new Vector3(-1118.505, 2700.2434, 18.554127), new Vector3(-1118.505, 2700.2434, 18.554127));
        CreateRobberyNPC("Аммуниция #48", "ig_old_man1a", new Vector3(-331.24167, 6085.918, 31.454763), -144.78027f, new Vector3(-331.24167, 6085.918, 31.454763), new Vector3(-331.24167, 6085.918, 31.454763));
        CreateRobberyNPC("Аммуниция #49", "ig_old_man1a", new Vector3(-3173.2637, 1089.192, 20.83873), -124.474655f, new Vector3(-3173.2637, 1089.192, 20.83873), new Vector3(-3173.2637, 1089.192, 20.83873));
        CreateRobberyNPC("Аммуниция #51", "ig_old_man1a", new Vector3(-661.7755, -933.6142, 21.829227), 176.94124f, new Vector3(841.3726, -1035.507, 28.19486), new Vector3(841.3726, -1035.507, 28.19486));
        CreateRobberyNPC("Аммуниция #52", "ig_old_man1a", new Vector3(1692.6107, 3761.3005, -131.63663), -131.63663f, new Vector3(1692.6107, 3761.3005, -131.63663), new Vector3(1692.6107, 3761.3005, -131.63663));
        CreateRobberyNPC("Аммуниция #78", "ig_old_man1a", new Vector3(253.59306, -51.038734, 69.94106), 71f, new Vector3(253.59306, -51.038734, 69.94106), new Vector3(253.59306, -51.038734, 69.94106));
        CreateRobberyNPC("Аммуниция #75", "ig_old_man1a", new Vector3(2567.0198, 292.5951, 108.73485), -8.479365f, new Vector3(2567.0198, 292.5951, 108.73485), new Vector3(2567.0198, 292.5951, 108.73485));
        CreateRobberyNPC("Аммуниция #64", "ig_old_man1a", new Vector3(-1304.2183, -395.1151, 36.695763), 66.9849f, new Vector3(-1304.2183, -395.1151, 36.695763), new Vector3(-1304.2183, -395.1151, 36.695763));
        CreateRobberyNPC("Парикмахерская #18", "s_f_m_fembarber", new Vector3(-30.99789, -152.02844, 57.07653), -14.69022f, new Vector3(-30.99789, -152.02844, 57.07653), new Vector3(-30.99789, -152.02844, 57.07653));
        CreateRobberyNPC("Парикмахерская #19", "s_f_m_fembarber", new Vector3(1211.8906, -470.93848, 73.64267), 73.64267f, new Vector3(1211.8906, -470.93848, 73.64267), new Vector3(1211.8906, -470.93848, 73.64267));
        CreateRobberyNPC("Парикмахерская #57", "s_f_m_fembarber", new Vector3(-277.7127, 6229.9893, 31.695518), 48.46691f, new Vector3(-277.7127, 6229.9893, 31.695518), new Vector3(-277.7127, 6229.9893, 31.695518));
        CreateRobberyNPC("Парикмахерская #58", "s_f_m_fembarber", new Vector3(1930.7815, 3728.6077, 32.84442), -145.88036f, new Vector3(1930.7815, 3728.6077, 32.84442), new Vector3(1930.7815, 3728.6077, 32.84442));
        CreateRobberyNPC("Тату салон #106", "u_m_y_tattoo_01", new Vector3(1325.0133, -1650.3137, 52.27516), 131.46558f, new Vector3(1325.0133, -1650.3137, 52.27516), new Vector3(1325.0133, -1650.3137, 52.27516));
        CreateRobberyNPC("Тату салон #66", "u_m_y_tattoo_01", new Vector3(-3170.89, 1072.9305, 20.829165), -23.290316f, new Vector3(-3170.89, 1072.9305, 20.829165), new Vector3(-3170.89, 1072.9305, 20.829165));
        CreateRobberyNPC("Тату салон #56", "u_m_y_tattoo_01", new Vector3(-292.37784, 6199.829, 31.487114), -136.6099f, new Vector3(-292.37784, 6199.829, 31.487114), new Vector3(-292.37784, 6199.829, 31.487114));
        CreateRobberyNPC("Тату салон #55", "u_m_y_tattoo_01", new Vector3(1862.6794, 3748.3179, 33.031895), 36.35458f, new Vector3(1862.6794, 3748.3179, 33.031895), new Vector3(1862.6794, 3748.3179, 33.031895));
        CreateRobberyNPC("Тату салон #54", "u_m_y_tattoo_01", new Vector3(319.85733, 181.26614, 103.5865), -110.04758f, new Vector3(319.85733, 181.26614, 103.5865), new Vector3(319.85733, 181.26614, 103.5865));

        OnRobberyTimer();
    }

    public static void OnPlayerConnect(PlayerX player)
    {
        player.ResetData("store_rob");
        player.SetData<bool>("status", true);
        player.SetSharedData("Player_Aiming_To", -1);
        int index = 0;
        foreach (var robbery in robbery_npc)
        {
            player.TriggerEvent("CreateRobberyNPC", "robbery_npc_" + index, robbery.model, robbery.position, robbery.heading, index);
            index++;
        }
    }


    public void OnRobberyTimer()
    {
        TimerEx.SetTimer(() =>
        {
            int index = 0;
            foreach (var robbery in robbery_npc)
            {

                if (robbery.robbery_state == 1)
                {
                    if (robbery.time_remaining > 0)
                    {
                        foreach (var player in PlayersManager.GetAllPlayers())
                        {
                            if (player.GetData<bool>("status") == true && xNetwork.xNetwork.IsInRangeOfPoint(player.Position, robbery.position, 30.0f) && player.GetSharedData<int>("Player_Aiming_To") == index)
                            {
                                robbery.time_remaining--;
                                oNetwork.EventToClient(player, "client::setTimerHud", robbery.time_remaining);
                            }
                            else 
                            {
                                oNetwork.EventToClient(player, "client::setTimerHud", 0);
                            }
                        }
                    }

                    if (robbery.time_remaining == 0)
                    {
                        robbery.robbery_state = 0;
                        robbery.time_remaining = 0;

                        robbery.time_vulnerable = DateTime.Now.AddMinutes(60);
                        UpdateRobberyState(index, 0);
                    }

                    int count = 0;
                    foreach (var player in PlayersManager.GetAllPlayers())
                    {
                        if (player.GetData<bool>("status") == true && xNetwork.xNetwork.IsInRangeOfPoint(player.Position, robbery.position, 30.0f) && player.GetSharedData<int>("Player_Aiming_To") == index)
                        {
                            count++;
                        }
                        if (robbery.time_remaining == 0 && player.GetData<bool>("status") == true && xNetwork.xNetwork.IsInRangeOfPoint(player.Position, robbery.position, 30.0f) && player.GetSharedData<int>("Player_Aiming_To") == index)
                        {
                            robbery.lastfraction = player.CharacterData.FractionID;
                            var rnd = new Random().Next(1500, 14000);
                            xNotify.Succ(player, $"Магазин ограблен на сумму: {rnd}$");
                            var item = new xItem(ElementType.MoneyHeist, rnd);
                            robbery.activeintime = false;
                            var obj = NAPI.Object.CreateObject(NAPI.Util.GetHashKey("bkr_prop_moneypack_01a"), robbery.position + new Vector3(0, 0, 1), new Vector3(0, 0, 0), 255, 0);
                            obj.SetSharedData("TYPE", "DROPPED");
                            obj.SetSharedData("PICKEDT", false);
                            obj.SetSharedData("HEISTMONEY", true);
                            obj.SetData("ITEM", item);
                            var id = new Random().Next(1500, 14000);
                            while (Items.ItemsDropped.Contains(id)) id = new Random().Next(5000, 13999);
                            obj.SetData("ID", id);

                            foreach (var target in PlayersManager.GetAllPlayers())
                            {
                                if (target.CharacterData.FractionID == 7 || target.CharacterData.FractionID == 9 || target.CharacterData.FractionID == 14)
                                {
                                    oNetwork.EventToClient(target, "createHeistBizMark", robbery_npc[index].position, robbery_npc[index].id);
                                    xNetwork.Fractions.Manager.fractionChat(target, $"Совершено ограбление в {robbery_npc[index].name}");
                                    NAPI.Task.Run(() =>
                                    {
                                        try
                                        {
                                            if (target != null)
                                            {
                                                oNetwork.EventToClient(target, "deleteHeistBizMark", robbery_npc[index].id);
                                            }
                                        }
                                        catch { }
                                    }, 300000);
                                }
                            }
                        }
                    }

                    if (count == 0)
                    {
                        UpdateRobberyState(index, 0);
                    }
                    else
                    {
                        UpdateRobberyState(index, 1);
                    }
                }
                else
                {
                    UpdateRobberyState(index, 0);
                }
                index++;
            }
        }, 1000, 0);
    }

    public static void UpdateRobberyState(int index, int state)
    {
        foreach (var player in PlayersManager.GetAllPlayers())
        {
            if (player.GetData<bool>("status") == true && xNetwork.xNetwork.IsInRangeOfPoint(player.Position, robbery_npc[index].position, 30.0f))
            {
                player.TriggerEvent("SetRobberyState", "robbery_npc_" + index, state);
            }
        }
    }

    [RemoteEvent("Players_Aiming_To")]
    public static void startRobbery(PlayerX player, int index)
    {
        if (index != -1)
        {
            if (xNetwork.xNetwork.IsInRangeOfPoint(player.Position, robbery_npc[index].position, 10.0f))
            {
                if (robbery_npc[index].robbery_state == 0)
                {
                    if (robbery_npc[index].activeintime == false)
                    {
                        if (player.CharacterData.FractionID != robbery_npc[index].lastfraction) { 
                            if (!player.HasData("temp_message"))
                            {
                                player.SetData("temp_message", false);
                            }
                            if (player.HasData("temp_message") && player.GetData<bool>("temp_message") == false)
                            {
                                player.SetData("temp_message", true);
                                xNotify.Error(player, $"Этот магазин уже был ограблен");
                            }

                            TimerEx.SetTimer(() =>
                            {
                                player.SetData("temp_message", false);
                            }, 10000, 1);
                            return;
                        }
                    }

                    if (!player.HasData("ActiveWeaponRobbey") || player.GetData<bool>("ActiveWeaponRobbey") == false)
                    {
                        return;
                    }

                    int can_pass = 0;

                    if (NAPI.Player.GetPlayerCurrentWeapon(player) != WeaponHash.Unarmed)
                    {
                        can_pass = 1;
                    }
                    else if (NAPI.Player.GetPlayerCurrentWeapon(player) != WeaponHash.Unarmed)
                    {
                        can_pass = 1;
                    }

                    if (can_pass == 0)
                    {
                        return;
                    }

                    if (player.CharacterData.FractionID == 0 || xNetwork.Fractions.Manager.FractionTypes[player.CharacterData.FractionID] == 2)
                    {
                        return;
                    }

                    int count = 0;
                    foreach (var target in PlayersManager.GetAllPlayers())
                    {
                        if (target.GetData<bool>("status") == true && xNetwork.xNetwork.IsInRangeOfPoint(player.Position, target.Position, 20.0f))
                        {
                            count++;
                        }
                    }

                    int faction_id = player.CharacterData.FractionID;

                    foreach (var target in PlayersManager.GetAllPlayers())
                    {
                        if (target.GetData<bool>("status") == true && xNetwork.xNetwork.IsInRangeOfPoint(target.Position, player.Position, 20.0f))
                        {
                            
                        }
                    }

                    robbery_npc[index].time_remaining = 60;
                    robbery_npc[index].cash_amount = 400;
                    robbery_npc[index].robbery_state = 1;
                    UpdateRobberyState(index, 1);
                }
            }
        }
        player.SetSharedData("Player_Aiming_To", index);
    }
}