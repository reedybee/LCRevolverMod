// system
using System.Collections;
// bepinex
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
// harmony
using HarmonyLib;
// unity
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
// reedybee
using LCRevolverMod.Scripts;
// TODO: FIX ALL THIS
namespace LCRevolverMod.Scripts
{
    public class Revolver : GrabbableObject
    {
        public bool isReloading;
        public int bulletsLoaded;
        public int bulletsMax = 6;
        private int ammoSlotToUse;
        private Coroutine revolverCoroutine;
        public PlayerControllerB previousPlayerHeldBy;

        public override void Start()
        {
            base.Start();
            grabbableToEnemies = false;
        }

        public override void Update()
        {
            base.Update();
            if (base.IsOwner)
            {
                if (this.bulletsLoaded <= 0 || this.isReloading || this.isPocketed)
                {
                    return;
                }
            }
        }

        public override void EquipItem()
        {
            base.EquipItem();
            previousPlayerHeldBy = this.playerHeldBy;
            previousPlayerHeldBy.equippedUsableItemQE = true;
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (this.isReloading)
            {
                return;
            }
            if (bulletsLoaded <= 0)
            {
                return;
            }
            if (!IsOwner)
            {
                return;
            }
            ShootGunAsync(true);   
        }

        public void ShootGunAsync(bool heldByPlayer)
        {
            isReloading = false;
        }

        private void StartReloadGun()
        {
            if (!ReloadedGun())
            {
                // no ammo
                return;
            }
            if (!IsOwner)
            {
                return;
            }
            if (revolverCoroutine != null)
            {
                StopCoroutine(revolverCoroutine);
            }
            revolverCoroutine = StartCoroutine(reloadAnimation());
        }

        private bool ReloadedGun()
        {
            int num = FindAmmoInInventory();
            if (num == -1)
            {
                // no ammo
                return false;
            }
            ammoSlotToUse = num;
            return true;
        }

        private int FindAmmoInInventory()
        {
            for (int i = 0; i < playerHeldBy.ItemSlots.Length; i++)
            {
                if (playerHeldBy.ItemSlots[i] == null)
                {
                    RevolverAmmo ammo = playerHeldBy.ItemSlots[i] as RevolverAmmo;
                    if(ammo != null)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private IEnumerator reloadAnimation()
        {
            isReloading = true;
            for (int i = bulletsMax; i > 0; i--)
            {
                yield return new WaitForSeconds(1.0f);
                bulletsLoaded++;
            }
        }

        public override void PocketItem()
        {
            base.PocketItem();
            StopUsingGun();
        }

        private void StopUsingGun()
        {
            previousPlayerHeldBy.equippedUsableItemQE = false;
        }
    }
}
