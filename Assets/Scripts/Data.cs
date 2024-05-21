using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Info 
{
    public static KeyCode WForward = KeyCode.W;
    public static KeyCode WBackward = KeyCode.S;
    public static KeyCode WLeft = KeyCode.A;
    public static KeyCode WRight = KeyCode.D;
    public static KeyCode Jump = KeyCode.Space;
    public static KeyCode Shoot = KeyCode.Mouse0;
    public static KeyCode Slot1 = KeyCode.Alpha1;
    public static KeyCode Slot2 = KeyCode.Alpha2;

}
public static class Weapon
{
    public static int PlasmarifleMaxAmmo = 30;
    public static int PlasmarifleMaxClip = 210;
    public static int ShotgunMaxClip = 60;
    public static int ShotgunMaxAmmo = 12;
    public static float ShotgunDelay = 0.9f;
    public static float PlasmarifleDelay = 0.02f;
    public static float PlasmarifleReloadDelay = 1.2f;
    public static float ShotgunReloadDelay = 0.5f;
    public static float ShotgunShootRange = 60f;
    public static float PlasmarifleShootRange = 100f;
    public static int PlasmarifleDamage = 10;
    public static int ShotgunDamage = 40;
    public static float ShotgunYSpread = 80f;
    public static float ShotgunXSpread = 120f;
    public static int ShotgunShootCount = 8;
}
public static class Settings
{
    public static bool FPS = false;
    public static bool BackGroundMusicPlaying = false;
    public static bool doomguy2 = false;
}
