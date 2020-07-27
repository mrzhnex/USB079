using Exiled.API.Features;
using Exiled.Events.EventArgs;
using UnityEngine;

namespace USB079
{
    public class SetEvents
    {
        internal void OnSpawning(SpawningEventArgs ev)
        {
            if (ev.RoleType == RoleType.Scp079)
            {
                Global.panel_079 = ev.Position - (Vector3.up * 1.6f);
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

        internal void OnRoundStarted()
        {
            Global.can_use_commands = true;
            foreach (Door door in Map.Doors)
            {
                if (door.DoorName.ToLower().Contains("nuke_surface"))
                {
                    WorkStation[] workS = Object.FindObjectsOfType<WorkStation>();
                    foreach (WorkStation work in workS)
                    {
                        if (Vector3.Distance(work.transform.position, door.gameObject.transform.position) < 10.0f)
                        {
                            Global.super_computer = work;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        internal void OnRoleChanging(ChangingRoleEventArgs ev)
        {
            if (ev.Player.GameObject.GetComponent<UsbHolder>() != null)
            {
                Object.Destroy(ev.Player.GameObject.GetComponent<UsbHolder>());
                Global.death_usb = ev.Player.Position;
            }
        }

        internal void OnSendingConsoleCommand(SendingConsoleCommandEventArgs ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            ev.Allow = false;
            if (ev.Name.ToLower() == "usb079download")
            {
                if (Global.panel_079 != Global.panel_null)
                {
                    if (ev.Player.Role == RoleType.Spectator || (ev.Player.Team == Team.SCP && ev.Player.Role != RoleType.Scp049))
                    {
                        ev.ReturnMessage = Global._isnotclass;
                        return;
                    }
                    if (!Global.IsFullRp)
                    {
                        if (ev.Player.Team == Team.MTF || ev.Player.Role == RoleType.FacilityGuard || ev.Player.Team == Team.SCP)
                        {
                            ev.ReturnMessage = Global._isnotclass;
                            return;
                        }
                    }
                    GameObject gameobj = ev.Player.GameObject;
                    if (Vector3.Distance(gameobj.transform.position, Global.panel_079) <= Global.download_distance)
                    {
                        foreach (Player _p in Player.List)
                        {
                            if (_p.GameObject.GetComponent<Downloader>() != null && _p.Id != ev.Player.Id)
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
            else if (ev.Name.ToLower() == "usb079upload")
            {
                if (ev.Player.GameObject.GetComponent<UsbHolder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                else
                {
                    if (Vector3.Distance(ev.Player.GameObject.transform.position, Global.super_computer.transform.position) < Global.load_distance)
                    {
                        if (ev.Player.GameObject.GetComponent<Uploader>() == null)
                        {
                            ev.Player.GameObject.AddComponent<Uploader>();
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
            else if (ev.Name.ToLower() == "usb079pickup")
            {
                if (ev.Player.Role == RoleType.Spectator || (ev.Player.Team == Team.SCP && ev.Player.Role != RoleType.Scp049))
                {
                    ev.ReturnMessage = Global._isnotclass;
                    return;
                }
                if (!Global.IsFullRp)
                {
                    if (ev.Player.Team == Team.MTF || ev.Player.Role == RoleType.FacilityGuard || ev.Player.Team == Team.SCP)
                    {
                        ev.ReturnMessage = Global._isnotclass;
                        return;
                    }
                }
                if (ev.Player.GameObject.GetComponent<UsbHolder>() != null)
                {
                    ev.ReturnMessage = Global._isholder;
                    return;
                }

                GameObject gameobj = ev.Player.GameObject;
                if (Vector3.Distance(gameobj.transform.position, Global.death_usb) <= (Global.load_distance * 1.5f))
                {
                    if (gameobj.GetComponent<UsbHolder>() == null)
                    {
                        gameobj.AddComponent<UsbHolder>();
                        ev.Player.ClearBroadcasts();
                        ev.Player.Broadcast(10, "<color=#228b22>Вы подобрали флешку с SCP079</color>", Broadcast.BroadcastFlags.Normal);
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
            else if (ev.Name.ToLower() == "usb079drop")
            {
                if (ev.Player.GameObject.GetComponent<UsbHolder>() == null)
                {
                    ev.ReturnMessage = Global._isnotholder;
                    return;
                }
                else
                {
                    Object.Destroy(ev.Player.GameObject.GetComponent<UsbHolder>());
                    Global.death_usb = ev.Player.Position;
                    ev.ReturnMessage = Global._isdrop;
                    return;
                }
            }
            else if (ev.Name.ToLower() == "usb079enter")
            {
                if (Global.inside == Global.panel_null || Global.outside == Global.panel_null)
                {
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (Vector3.Distance(ev.Player.GameObject.transform.position, Global.outside) > Global.distance_to_enter_exit)
                {
                    ev.ReturnMessage = Global._istoolongforgate1;
                    return;
                }
                ev.Player.Position = Global.inside;
                ev.ReturnMessage = Global._successenterexit;
                return;
            }
            else if (ev.Name.ToLower() == "usb079exit")
            {
                if (Global.inside == Global.panel_null || Global.outside == Global.panel_null)
                {
                    ev.ReturnMessage = Global._somethingwrong;
                    return;
                }
                if (Vector3.Distance(ev.Player.GameObject.transform.position, Global.inside) > Global.distance_to_enter_exit)
                {
                    ev.ReturnMessage = Global._istoolongforgate2;
                    return;
                }
                ev.Player.Position = Global.outside;
                ev.ReturnMessage = Global._successenterexit;
                return;
            }
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
            Global.panel_079 = Global.panel_null;
            Global.death_usb = Global.panel_null;
            Global.inside = Global.panel_079;
            Global.outside = Global.panel_null;
        }
    }
}