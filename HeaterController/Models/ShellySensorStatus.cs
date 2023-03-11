using System.Text.Json.Serialization;

namespace HeaterControllerApplication.Models;

public class ShellySensorStatus
{
    public double GetTemperature()
    {
        if (ext_temperature == null || ext_temperature._0 == null)
        {
            throw new Exception("Invalid sensor status object");
        }

        return ext_temperature._0.tC;
    }

    public Wifi_Sta? wifi_sta { get; set; }
    public Cloud? cloud { get; set; }
    public Mqtt? mqtt { get; set; }
    public string? time { get; set; }
    public int unixtime { get; set; }
    public int serial { get; set; }
    public bool has_update { get; set; }
    public string? mac { get; set; }
    public int cfg_changed_cnt { get; set; }
    public Actions_Stats? actions_stats { get; set; }
    public Relay[]? relays { get; set; }
    public Meter[]? meters { get; set; }
    public Input[]? inputs { get; set; }
    public Ext_Sensors? ext_sensors { get; set; }
    public Ext_Temperature? ext_temperature { get; set; }
    public Ext_Humidity? ext_humidity { get; set; }
    public Update? update { get; set; }
    public int ram_total { get; set; }
    public int ram_free { get; set; }
    public int fs_size { get; set; }
    public int fs_free { get; set; }
    public int uptime { get; set; }
}

public class Wifi_Sta
{
    public bool connected { get; set; }
    public string? ssid { get; set; }
    public string? ip { get; set; }
    public int rssi { get; set; }
}

public class Cloud
{
    public bool enabled { get; set; }
    public bool connected { get; set; }
}

public class Mqtt
{
    public bool connected { get; set; }
}

public class Actions_Stats
{
    public int skipped { get; set; }
}

public class Ext_Sensors
{
    public string? temperature_unit { get; set; }
}

public class Ext_Temperature
{
    [JsonPropertyName("0")]
    public _0? _0 { get; set; }
}

public class _0
{
    public string? hwID { get; set; }
    public float tC { get; set; }
    public float tF { get; set; }
}

public class Ext_Humidity
{
}

public class Update
{
    public string? status { get; set; }
    public bool has_update { get; set; }
    public string? new_version { get; set; }
    public string? old_version { get; set; }
}

public class Relay
{
    public bool ison { get; set; }
    public bool has_timer { get; set; }
    public int timer_started { get; set; }
    public int timer_duration { get; set; }
    public int timer_remaining { get; set; }
    public string? source { get; set; }
}

public class Meter
{
    public float power { get; set; }
    public bool is_valid { get; set; }
}

public class Input
{
    public int input { get; set; }
    public string? _event { get; set; }
    public int event_cnt { get; set; }
}


