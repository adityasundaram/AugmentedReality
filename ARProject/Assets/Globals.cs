using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Global{

    public struct CharacterObject
    {
        public GameObject charObject;
        public PlayerController charController;
        public Actions charActions;
        public GameObject health;

        public List<string> WeaponList;

        public bool WeaponAllowed;
        public int currentWeaponIndex;
        public String weaponName;
    }



    public struct Cannons
    {
        public static int cannonSelected = 0;
        public static int[] cannonAngles = new int[]{-135, -220, 45, -90};
    }
}