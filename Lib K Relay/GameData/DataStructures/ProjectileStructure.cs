using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lib_K_Relay.GameData.DataStructures
{
    public class ProjectileStructure : IDataStructure<byte>
    {
        public int Damage;
        public float Speed;
        public int Size;
        public float Lifetime;
        public int MaxDamage;
        public int MinDamage;
        public float Magnitude;
        public float Amplitude;
        public float Frequency;
        public bool Wavy;
        public bool Parametric;
        public bool Boomerang;
        public bool ArmorPiercing;
        public bool MultiHit;
        public bool PassesCover;
        public Dictionary<string, float> StatusEffects;

        public byte Id { get; private set; }

        public string Name { get; private set; }

        public ProjectileStructure(XElement projectile)
        {
            Id = (byte)projectile.AttrDefault((XName)"id", "0").ParseInt();
            Damage = projectile.ElemDefault((XName)nameof(Damage), "0").ParseInt();
            Speed = projectile.ElemDefault((XName)nameof(Speed), "0").ParseFloat() / 10000f;
            Size = projectile.ElemDefault((XName)nameof(Size), "0").ParseInt();
            Lifetime = projectile.ElemDefault((XName)"LifetimeMS", "0").ParseFloat();
            MaxDamage = projectile.ElemDefault((XName)nameof(MaxDamage), "0").ParseInt();
            MinDamage = projectile.ElemDefault((XName)nameof(MinDamage), "0").ParseInt();
            Magnitude = projectile.ElemDefault((XName)nameof(Magnitude), "0").ParseFloat();
            Amplitude = projectile.ElemDefault((XName)nameof(Amplitude), "0").ParseFloat();
            Frequency = projectile.ElemDefault((XName)nameof(Frequency), "0").ParseFloat();
            Wavy = projectile.HasElement((XName)nameof(Wavy));
            Parametric = projectile.HasElement((XName)nameof(Parametric));
            Boomerang = projectile.HasElement((XName)nameof(Boomerang));
            ArmorPiercing = projectile.HasElement((XName)nameof(ArmorPiercing));
            MultiHit = projectile.HasElement((XName)nameof(MultiHit));
            PassesCover = projectile.HasElement((XName)nameof(PassesCover));
            var effects = new Dictionary<string, float>();
            projectile.Elements((XName)"ConditionEffect").ForEach<XElement>((Action<XElement>)(effect =>
                effects[effect.Value] = effect.AttrDefault((XName)"duration", "0").ParseFloat()));
            StatusEffects = effects;
            Name = projectile.ElemDefault((XName)"ObjectId", "");
        }

        public override string ToString()
        {
            return string.Format("Projectile: {0} (0x{1:X})", (object)Name, (object)Id);
        }
    }
}