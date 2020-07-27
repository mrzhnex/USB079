using Exiled.API.Features;
using UnityEngine;

namespace USB079
{
    class Uploader : MonoBehaviour
    {
        private Player uploader;
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private float progress_upload = 0f;

        public void Start()
        {
            uploader = Player.Get(gameObject);
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeIsUp)
            {
                timer = 0f;

                if (Vector3.Distance(gameObject.transform.position, Global.super_computer.transform.position) < Global.load_distance)
                {
                    progress_upload += timeIsUp;
                    uploader.ClearBroadcasts();
                    uploader.Broadcast(2, "<color=#228b22>Вы загружаете SCP079. Осталось секунд: " + (Global.time_to_load - progress_upload) + "</color>", Broadcast.BroadcastFlags.Normal);
                }
                else
                {
                    uploader.ClearBroadcasts();
                    uploader.Broadcast(10, "<color=#228b22>Вы прекратили загрузку SCP079</color>", Broadcast.BroadcastFlags.Normal);
                    Destroy(gameObject.GetComponent<Uploader>());
                }
                if (progress_upload > Global.time_to_load)
                {
                    uploader.ClearBroadcasts();
                    uploader.Broadcast(10, "<color=#42aaff>Вы загрузили SCP079</color>", Broadcast.BroadcastFlags.Normal);
                    Destroy(gameObject.GetComponent<UsbHolder>());
                    foreach (Player searchSCP079 in Player.List)
                    {
                        if (searchSCP079.Role == RoleType.Scp079)
                        {
                            searchSCP079.ReferenceHub.scp079PlayerScript.ForceLevel(4, true);
                            if (Global.IsFullRp)
                            {
                                searchSCP079.GameObject.GetComponent<ServerRoles>().BypassMode = true;
                            }
                            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase("ATTENTION . THE FOUNDATION HAS LOST CONTROL OVER THE FACILITY", 0.0f, 0.0f);
                        }
                    }
                    Destroy(gameObject.GetComponent<Uploader>());
                }
            }
        }
    }
}