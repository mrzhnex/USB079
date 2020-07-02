using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace USB079
{
    public class SetEvents
    {
        public void OnCallCommand(ConsoleCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            string command = ev.Command.Split(new char[]
            {
                ' '
            })[0].ToLower();
            if (command == "usb079download")
            {
                if (Global.panel_079 != Global.panel_null)
                {                    
                    if (ev.Player.GetRole() == RoleType.Spectator || (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp049))
                    {
                        ev.ReturnMessage = Global._isnotclass;
                        return;
                    }
                    if (!Global.usb079_is_fullrp)
                    {
                        if (ev.Player.GetTeam() == Team.MTF || ev.Player.GetRole() == RoleType.FacilityGuard || ev.Player.GetTeam() == Team.SCP)
                        {
                            ev.ReturnMessage = Global._isnotclass;
                            return;
                        }
                    }
                    GameObject gameobj = ev.Player.gameObject;
                    if (Vector3.Distance(gameobj.transform.position, Global.panel_079) <= Global.download_distance)
                    {
                        foreach (ReferenceHub _p in Player.GetHubs())
                        {
                            if (_p.gameObject.GetComponent<Downloader>() != null && _p.GetPlayerId() != ev.Player.GetPlayerId())
                            {
                                ev.ReturnMessage = Global._alreadyload;
                                return;
                            }
                        }
                        if (gameobj.GetComponent<Downloader>() == null)
                        {
                            gameobj.AddComponent<Downloader>();
                            ev.ReturnMessage = Global._successstart;
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = Global._alreadyload;
                            return;
                        }

                    }
                    else
                    {
                        ev.ReturnMessage = Global._isnotclass;
                        return;
                    }
                  
                }
                else
                {
                    ev.ReturnMessage = Global._usbisempty;
                    return;
                }
            }
            else if (command == "usb079upload")
            {
                if (ev.Player.gameObject.GetComponent<UsbHolder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                else
                {
                    if (Vector3.Distance(ev.Player.gameObject.transform.position, Global.super_computer.transform.position) < Global.load_distance)
                    {
                        if (ev.Player.gameObject.GetComponent<Uploader>() == null)
                        {
                            ev.Player.gameObject.AddComponent<Uploader>();
                            ev.ReturnMessage = Global._successstart;
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = Global._alreadyload;
                            return;
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = Global._istoolongforupload;
                        return;
                    }
                }
            }
            else if (command == "usb079pickup")
            {
                if (ev.Player.GetRole() == RoleType.Spectator || (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp049))
                {
                    ev.ReturnMessage = Global._isnotclass;
                    return;
                }
                if (!Global.usb079_is_fullrp)
                {
                    if (ev.Player.GetTeam() == Team.MTF || ev.Player.GetRole() == RoleType.FacilityGuard || ev.Player.GetTeam() == Team.SCP)
                    {
                        ev.ReturnMessage = Global._isnotclass;
                        return;
                    }
                }   
                if (ev.Player.gameObject.GetComponent<UsbHolder>() != null)
                {
                    ev.ReturnMessage = Global._isholder;
                    return;
                }

                GameObject gameobj = ev.Player.gameObject;
                if (Vector3.Distance(gameobj.transform.position, Global.death_usb) <= (Global.load_distance * 1.5f))
                {
                    if (gameobj.GetComponent<UsbHolder>() == null)
                    {
                        gameobj.AddComponent<UsbHolder>();
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#228b22>Вы подобрали флешку с SCP079</color>", true);
                        Global.death_usb = Global.panel_null;
                        return;
                    }
                    else
                    {
                        ev.ReturnMessage = Global._isholder;
                        return;
                    }
                }
                else
                {
                    ev.ReturnMessage = Global._istoolongforflash;
                    return;
                }

            }
            else if (command == "usb079drop")
            {
                if (ev.Player.gameObject.GetComponent<UsbHolder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                else
                {
                    Object.Destroy(ev.Player.gameObject.GetComponent<UsbHolder>());
                    Global.death_usb = ev.Player.GetPosition();
                    ev.ReturnMessage = Global._isdrop;
                    return;
                }
            }
            else if (command == "usb079enter")
            {
                if (Global.inside == Global.panel_null || Global.outside == Global.panel_null)
                {
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (Vector3.Distance(ev.Player.gameObject.transform.position, Global.outside) > Global.distance_to_enter_exit)
                {
                    ev.ReturnMessage = Global._istoolongforgate1;
                    return;
                }
                ev.Player.SetPosition(Global.inside);
                ev.ReturnMessage = Global._successenterexit;
                return;
            }
            else if (command == "usb079exit")
            {
                if (Global.inside == Global.panel_null || Global.outside == Global.panel_null)
                {
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (Vector3.Distance(ev.Player.gameObject.transform.position, Global.inside) > Global.distance_to_enter_exit)
                {
                    ev.ReturnMessage = Global._istoolongforgate2;
                    return;
                }
                ev.Player.SetPosition(Global.outside);
                ev.ReturnMessage = Global._successenterexit;
                return;
            }

        }

        public void OnPlayerSpawn(PlayerSpawnEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<UsbHolder>() != null)
            {
                Object.Destroy(ev.Player.gameObject.GetComponent<UsbHolder>());
                Global.death_usb = ev.Player.GetPosition();
            }
            if (ev.Role == RoleType.Scp079)
            {
                Global.panel_079 = ev.Spawnpoint - (Vector3.up * 1.6f);
                foreach (Door door in Map.Doors)
                {
                    if (door.DoorName.ToLower().Contains("079_second"))
                    {
                        Vector3 tempdoor = door.gameObject.transform.position - (Vector3.up * 1.5f);
                        Global.inside = Global.panel_079 + Vector3.Normalize(tempdoor - Global.panel_079) * Global.distance_to_set_vector_1;
                        Global.outside = Global.panel_079 + Vector3.Normalize(tempdoor - Global.panel_079) * Global.distance_to_set_vector_2;
                    }
                }
            }
        }

        public void OnPlayerDeath(ref PlayerDeathEvent ev)
        {
            if (ev.Player.gameObject.GetComponent<UsbHolder>() != null)
            {
                Object.Destroy(ev.Player.gameObject.GetComponent<UsbHolder>());
                Global.death_usb = ev.Player.GetPosition();
            }
        }

        public void OnRoundStart()
        {
            Global.can_use_commands = true;
            foreach (Door door in Map.Doors)
            {
                if (door.DoorName.ToLower().Contains("nuke_surface"))
                {
                    WorkStation[] workS = UnityEngine.Object.FindObjectsOfType<WorkStation>();
                    foreach (WorkStation work in workS)
                    {
                        if (Vector3.Distance(work.transform.position, door.gameObject.transform.position) <= 10.0f)
                        {
                            Global.super_computer = work;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
            Global.panel_079 = Global.panel_null;
            Global.death_usb = Global.panel_null;
            Global.inside = Global.panel_079;
            Global.outside = Global.panel_null;
            Global.usb079_is_fullrp = true;
        }
    }
}