using UnityEngine;
using Exiled.API.Features;

namespace USB079
{
    class Downloader : MonoBehaviour
    {
        private Player downloader;
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private float progress_download = 0.0f;

        public void Start()
        {
            downloader = Player.Get(gameObject);
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeIsUp)
            {
                timer = 0.0f;
                if (Vector3.Distance(gameObject.transform.position, Global.panel_079) < Global.download_distance)
                {
                    progress_download += timeIsUp;
                    downloader.ClearBroadcasts();
                    downloader.Broadcast(2, "<color=#228b22>Вы загружаете SCP079 на флешку. Осталось секунд: " + (Global.time_to_upload - progress_download) + "</color>", Broadcast.BroadcastFlags.Normal);
                }
                else
                {
                    downloader.ClearBroadcasts();
                    downloader.Broadcast(10, "<color=#228b22>Загрузка на флешку остановилась</color>", Broadcast.BroadcastFlags.Normal);
                    Destroy(gameObject.GetComponent<Downloader>());
                }

                if (progress_download > Global.time_to_upload)
                {
                    gameObject.AddComponent<UsbHolder>();
                    Global.panel_079 = Global.panel_null;
                    downloader.ClearBroadcasts();
                    downloader.Broadcast(10, "<color=#42aaff>Вы загрузили SCP079 на флешку</color>", Broadcast.BroadcastFlags.Normal);
                    Destroy(gameObject.GetComponent<Downloader>());
                }
            }            
        }
    }
}