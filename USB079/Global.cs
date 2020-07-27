using UnityEngine;

namespace USB079
{
    public class Global
    {
        public static bool IsFullRp = false;
        public static WorkStation super_computer;
        public static float download_distance = 3.0f;
        public static float load_distance = 2.0f;
        public static Vector3 panel_079;
        public static float time_to_pickup = 10f;

        public static float time_to_upload = 60f;
        public static float time_to_load = 30f;
        public static Vector3 panel_null = new Vector3(0f, -10000f, 0f);
        public static Vector3 death_usb;
    
        public static string _isnotclass = "Отказано. Вы не можете взаимодействовать с флешкой";
        public static string _alreadyload = "Отказано. USB флешку уже закачивают";
        public static string _usbisempty = "Отказано. Данных на закачку нет";
        public static string _successstart = "Загрузка успешно началась";
        public static string _isnotholder = "Отказано. У вас отсутствует флешка";
        public static string _isholder = "У вас есть USB флешка";
        public static string _istoolongforflash = "Вы порыскали вокруг, но флешку не нашли";
        public static string _isdrop = "Вы бросили флешку";
        public static string _istoolongforupload = "Рядом нет супер-компьютера";

        public static Vector3 outside;
        public static Vector3 inside;
        public static float distance_to_enter_exit = 2.0f;

        public static string _somethingwrong = "Калитка закрыта неведомыми технологиями...";
        public static string _istoolongforgate1 = "Вы слишком далеко от калитки, чтобы зайти внутрь";
        public static string _istoolongforgate2 = "Вы слишком далеко от калитки, чтобы выйти наружу";
        public static string _successenterexit = "Вы прошли через калитку";

        public static float distance_to_set_vector_1 = 3f;
        public static float distance_to_set_vector_2 = 5f;
        internal static bool can_use_commands;
    }
}
