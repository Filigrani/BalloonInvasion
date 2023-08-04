using BalloonInvasion.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BalloonInvasion.Weapon;

namespace BalloonInvasion
{
    internal class Shooter : Component
    {
        public Dictionary<WeaponType, WeaponInfo> WeaponsInfos = new Dictionary<WeaponType, WeaponInfo>();
        public bool Ready = false;
        public List<Weapon> Weapons = new List<Weapon>();
        public int CurrentWeaponIndex = 0;
        public Weapon CurrentWeapon = null;
        public void Init()
        {
            if (Ready)
            {
                return;
            }

            WeaponInfo Gun = new WeaponInfo();
            Gun.Type = WeaponType.None;
            WeaponsInfos.Add(Gun.Type, Gun);

            WeaponInfo Gun1 = new WeaponInfo();
            Gun1.Type = WeaponType.Pistol;
            Gun1.FireRate = 0.1f;
            Gun1.ReloadTime = 1.32f;
            Gun1.Damage = 5;
            Gun1.ClipSize = 12;
            Gun1.ReserveMax = 120;
            Gun1.Auto = false;
            Gun1.MultiHit = false;
            Gun1.Knockback = 0.42f;
            Gun1.CursorType = "CrosshairPartly";
            Gun1.Recoil = 5;
            WeaponsInfos.Add(Gun1.Type, Gun1);

            WeaponInfo Gun2 = new WeaponInfo();
            Gun2.Type = WeaponType.Shotgun;
            Gun2.FireRate = 0.3f;
            Gun2.ReloadTime = 0.4f;
            Gun2.ReloadTimePerAmmo = 0.55f;
            Gun2.Damage = 25;
            Gun2.ClipSize = 5;
            Gun2.ReserveMax = 50;
            Gun2.Auto = false;
            Gun2.MultiHit = true;
            Gun2.Knockback = 1.67f;
            Gun2.CursorType = "CrosshairBig";
            Gun2.Recoil = 17;
            WeaponsInfos.Add(Gun2.Type, Gun2);

            WeaponInfo Gun3 = new WeaponInfo();
            Gun3.Type = WeaponType.Machingun;
            Gun3.FireRate = 0.07f;
            Gun3.ReloadTime = 1.3f;
            Gun3.Auto = true;
            Gun3.Damage = 7;
            Gun3.ClipSize = 30;
            Gun3.ReserveMax = 300;
            Gun3.MultiHit = false;
            Gun3.Knockback = 0.2f;
            Gun3.CursorType = "CrosshairHorizontal";
            Gun3.Recoil = 10;
            WeaponsInfos.Add(Gun3.Type, Gun3);

            WeaponInfo Gun4 = new WeaponInfo();
            Gun4.Type = WeaponType.SniperRifle;
            Gun4.FireRate = 0.1f;
            Gun4.ReloadTime = 0.85f;
            Gun4.Auto = false;
            Gun4.AutoReload = true;
            Gun4.Damage = 20;
            Gun4.ClipSize = 1;
            Gun4.ReserveMax = 30;
            Gun4.MultiHit = false;
            Gun4.Knockback = 3.1f;
            Gun4.CursorType = "Crosshair";
            Gun4.Recoil = 18;
            WeaponsInfos.Add(Gun4.Type, Gun4);

            WeaponInfo Gun5 = new WeaponInfo();
            Gun5.Type = WeaponType.Nailgun;
            Gun5.FireRate = 0.19f;
            Gun5.ReloadTime = 0;
            Gun5.Auto = true;
            Gun5.AutoReload = false;
            Gun5.Damage = 3;
            Gun5.ClipSize = 300;
            Gun5.ReserveMax = 0;
            Gun5.MultiHit = false;
            Gun5.Knockback = 0.31f;
            Gun5.CursorType = "CrosshairPartly";
            Gun5.Recoil = 1;
            WeaponsInfos.Add(Gun5.Type, Gun5);

            Ready = true;
        }

        public void SelectWeapon(GameTime gameTime, int Index)
        {
            if (Weapons.Count == 0)
            {
                return;
            }
            
            if(Index > Weapons.Count-1)
            {
                Index = 0;
            }

            if(CurrentWeapon != null)
            {
                CurrentWeapon.OnUnEquip(gameTime);
            }
            
            CurrentWeaponIndex = Index;
            CurrentWeapon = Weapons[CurrentWeaponIndex];
            CurrentWeapon.OnEquip(gameTime);
            if (GameManager.Game != null)
            {
                if(GameManager.CursorComp != null)
                {
                    GameManager.CursorComp.ChangeCursor(CurrentWeapon.Info.CursorType);
                }
                if (GameManager.MyGhost != null)
                {
                    GameManager.MyGhost.SwitchWeapon();
                }
            }
        }

        public void NextWeapon(GameTime gameTime)
        {
            if (Weapons.Count == 0)
            {
                return;
            }
            int Next = CurrentWeaponIndex + 1;
            if (Next > Weapons.Count - 1)
            {
                Next = 0;
            }
            SelectWeapon(gameTime, Next);
        }
        public void PreviousWeapon(GameTime gameTime)
        {
            if (Weapons.Count == 0)
            {
                return;
            }
            int Previous = CurrentWeaponIndex - 1;
            if (Previous == -1)
            {
                Previous = Weapons.Count-1;
            }
            SelectWeapon(gameTime, Previous);
        }

        public void GiveWeapon(WeaponType WeaponType)
        {
            WeaponInfo wpInfo = WeaponsInfos[WeaponType];
            Weapon wp = new Weapon(wpInfo);
            Weapons.Add(wp);
        }


        public Shooter()
        {
            Init();
            GiveWeapon(WeaponType.Pistol);
            GiveWeapon(WeaponType.Shotgun);
            GiveWeapon(WeaponType.Machingun);
            GiveWeapon(WeaponType.SniperRifle);
            GiveWeapon(WeaponType.Nailgun);
            SelectWeapon(new GameTime(), 0);
        }

        public bool Shoot(GameTime gameTime)
        {
            if(CurrentWeapon.Info.Type != WeaponType.None)
            {
                bool DidShot = CurrentWeapon.Shot(gameTime);
                if (DidShot)
                {
                    GameManager.MyGhost.DoShot();
                }
                return DidShot;
            }
            return false;
        }
        public void Reload(GameTime gameTime)
        {
            if (CurrentWeapon.Info.Type != WeaponType.None)
            {
                bool DidReload = CurrentWeapon.Reload(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Weapon wp in Weapons)
            {
                wp.Update(gameTime);
            }
        }
    }
}
