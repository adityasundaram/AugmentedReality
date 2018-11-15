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
    }
}