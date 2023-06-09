﻿// <auto-generated/>

#nullable enable

namespace HeaterControllerApplication.Models;

// JsonSerializer.Deserialize<ShellyRelayStatus> will have null entries for
// reference fields in ShellyRelayStatus that are not present in the json
// For example ext_sensors can be null
public class ShellyRelayStatus
{
    public RelayStatus GetRelayStatus()
    {
        if (relays == null)
        {
            throw new Exception("Invalid response from the relay");
        }

        return relays[0].ison ? RelayStatus.On : RelayStatus.Off;
    }

    public Wifi_Sta? wifi_sta { get; set; }
    public Cloud? cloud { get; set; }
    public Mqtt? mqtt { get; set; }
    public string? time { get; set; }
    public int? unixtime { get; set; }
    public int? serial { get; set; }
    public bool? has_update { get; set; }
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
    public int? ram_total { get; set; }
    public int? ram_free { get; set; }
    public int? fs_size { get; set; }
    public int? fs_free { get; set; }
    public int? uptime { get; set; }
}
