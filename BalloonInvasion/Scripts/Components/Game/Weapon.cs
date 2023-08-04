using BalloonInvasion.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonInvasion
{
    public class Weapon
    {
        public WeaponInfo Info = new WeaponInfo();
        public TimeSpan LastShot = new TimeSpan();
        public TimeSpan NextShot = new TimeSpan();
        public TimeSpan ReloadFinish = new TimeSpan();
        public TimeSpan LastUpdate = new TimeSpan();
        public bool IsReloading = false;
        public int Clip = 0;
        public int Reserve = 0;
        public bool Equipped = false;
        public bool MyOne = false;
        public enum WeaponType
        {
            None,
            Pistol,
            Shotgun,
            Machingun,
            SniperRifle,
            Nailgun,
        }

        public class WeaponInfo
        {
            public float ReloadTime = 0;
            public float ReloadTimePerAmmo = 0;
            public float FireRate = 0;
            public float Damage = 0;
            public WeaponType Type = WeaponType.None;
            public int ClipSize = 0;
            public int ReserveMax = 0;
            public bool Auto = false;
            public bool MultiHit = false;
            public float Knockback = 1;
            public string CursorType = "Crosshair";
            public bool AutoReload = false;
            public int Recoil = 17;
        }

        public Weapon()
        {
            OnGiven();
        }
        public Weapon(WeaponInfo info)
        {
            Info = info;
            OnGiven();
        }

        public void OnGiven()
        {
            Clip = Info.ClipSize;
            Reserve = Info.ReserveMax;
        }

        public void OnEquip(GameTime gameTime)
        {
            if (Equipped)
            {
                return;
            }
            Equipped = true;

            if (!Info.AutoReload)
            {
                if (Clip == 0 && Reserve > 0)
                {
                    Reload(gameTime);
                }
            }
        }

        public void OnUnEquip(GameTime gameTime)
        {
            if (!Equipped)
            {
                return;
            }
            Equipped = false;

            if (!Info.AutoReload && IsReloading)
            {
                CancleReload();
            }
        }
        

        public bool CanShot()
        {
            if(Clip <= 0)
            {
                return false;
            }

            if (NextShot.TotalMilliseconds > LastUpdate.TotalMilliseconds)
            {
                return false;
            }
            return true;
        }

        public bool Shot(GameTime gameTime)
        {
            if(!CanShot())
            {
                return false;
            }

            if (IsReloading)
            {
                CancleReload();
            }

            LastShot = gameTime.TotalGameTime;
            Clip--;

            NextShot = gameTime.TotalGameTime + TimeSpan.FromSeconds(Info.FireRate);
            if(Clip == 0 && Reserve > 0) 
            {
                Reload(gameTime);
            }
            return true;
        }

        public void CancleReload()
        {
            if(IsReloading)
            {
                NextShot = new TimeSpan();
                ReloadFinish = new TimeSpan();
                IsReloading = false;
            }
        }

        public bool Reload(GameTime gameTime)
        {
            if(!IsReloading && Clip < Info.ClipSize && Reserve > 0)
            {
                int Insert = 0;
                if (Reserve < Info.ClipSize)
                {
                    Insert = Reserve;
                } else
                {
                    Insert = Info.ClipSize - Clip;
                }

                GameManager.MyGhost.DoReload(Insert);

                if(Info.ReloadTimePerAmmo == 0)
                {
                    if (Clip == 0)
                    {
                        NextShot = gameTime.TotalGameTime + TimeSpan.FromSeconds(Info.ReloadTime);
                    }
                    ReloadFinish = gameTime.TotalGameTime + TimeSpan.FromSeconds(Info.ReloadTime);
                } else
                {
                    MyOne = true;
                    if(Clip == 0)
                    {
                        NextShot = gameTime.TotalGameTime + TimeSpan.FromSeconds(Info.ReloadTime + Info.ReloadTimePerAmmo);
                    }
                    
                    ReloadFinish = gameTime.TotalGameTime + TimeSpan.FromSeconds(Info.ReloadTime + Info.ReloadTimePerAmmo);
                }
                IsReloading = true;
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            LastUpdate = gameTime.TotalGameTime;
            if (IsReloading)
            {
                if (ReloadFinish.TotalMilliseconds < LastUpdate.TotalMilliseconds)
                {
                    if (!MyOne)
                    {
                        if (Reserve >= Info.ClipSize)
                        {
                            int NeedAmmo = Info.ClipSize - Clip;
                            Reserve -= NeedAmmo;
                            Clip += NeedAmmo;
                        } else
                        {
                            Clip = Reserve;
                            Reserve = 0;
                        }
                        IsReloading = false;
                    } else
                    {
                        Clip++;
                        Reserve--;
                        if(Clip != Info.ClipSize)
                        {
                            ReloadFinish = gameTime.TotalGameTime + TimeSpan.FromSeconds(Info.ReloadTimePerAmmo);
                        } else
                        {
                            IsReloading = false;
                        }
                    }
                }
            }
        }
    }
}
