using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstructSpriteString
{
    public static string Spaceship(
        string scene_name,
        bool RepairWindshield_completed,
        bool RepairSpaceship_completed,
        bool InstallNewHyperdriveCore_completed
    ){
        //type:
        // interior/exterior

        //window:
        // nw - new window
        // bw - broken window
        // pw - patched window
        // fw - fixed window

        //hyperdrive (interior only):
        // nhd - no hyperdrive
        // hd - hyperdrive

        //state:
        // broken/clean

        string type, window, hyperdrive, state;
        type = scene_name == "Level 0 - spaceship" ? "interior" : "exterior";
        window = RepairWindshield_completed ? "fw" : "pw";
        hyperdrive = InstallNewHyperdriveCore_completed ? "hd" : "nhd";
        state = RepairSpaceship_completed ? "clean" : "broken";

        string last_id_part = type == "exterior" ? "" : "_"+hyperdrive;

        string sprite_identifier = type + "_" + window + "_" + state + last_id_part;

        return "Sprites/Spaceship/"+sprite_identifier;
    }
}
