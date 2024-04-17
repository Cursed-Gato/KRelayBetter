using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Lib_K_Relay.GameData.DataStructures
{
    public class ObjectStructure : IDataStructure<ushort>
    {
        public string ObjectClass;
        public ushort MaxHP;
        public float XPMult;
        public bool Static;
        public bool OccupySquare;
        public bool EnemyOccupySquare;
        public bool FullOccupy;
        public bool BlocksSight;
        public bool ProtectFromGroundDamage;
        public bool ProtectFromSink;
        public bool Enemy;
        public bool Player;
        public bool Pet;
        public bool DrawOnGround;
        public ushort Size;
        public ushort ShadowSize;
        public ushort Defense;
        public bool Flying;
        public bool God;
        public bool Cube;
        public bool Quest;
        public bool Item;
        public bool Usable;
        public bool Soulbound;
        public ushort MpCost;
        public ProjectileStructure[] Projectiles;
        public bool Invulnerable;
        public bool Invincible;

        public ObjectStructure(XElement obj)
        {
            Id = (ushort)obj.AttrDefault("type", "0x0").ParseHex();

            // if this errors you need to add a new entry to the krObject.Class enum
            ObjectClass = obj.ElemDefault("Class", "GameObject");

            MaxHP = (ushort)obj.ElemDefault("MaxHitPoints", "0").ParseHex();
            XPMult = obj.ElemDefault("XpMult", "0").ParseFloat();

            Static = obj.HasElement("Static");
            OccupySquare = obj.HasElement("OccupySquare");
            EnemyOccupySquare = obj.HasElement("EnemyOccupySquare");
            FullOccupy = obj.HasElement("FullOccupy");
            BlocksSight = obj.HasElement("BlocksSight");
            ProtectFromGroundDamage = obj.HasElement("ProtectFromGroundDamage");
            ProtectFromSink = obj.HasElement("ProtectFromSink");
            Enemy = obj.HasElement("Enemy");
            Player = obj.HasElement("Player");
            Pet = obj.HasElement("Pet");
            DrawOnGround = obj.HasElement("DrawOnGround");
            Cube = obj.HasElement("Cube");
            Quest = obj.HasElement("Quest");
            Item = obj.HasElement("Item");
            Usable = obj.HasElement("Usabled");
            Soulbound = obj.HasElement("Soulbound");
            MpCost = (ushort)obj.ElemDefault("MpCost", "0").ParseInt();
            Invulnerable = obj.HasElement("Invulnerable");
            Invincible = obj.HasElement("Invincible");

            Size = (ushort)obj.ElemDefault("Size", "0").ParseInt();
            ShadowSize = (ushort)obj.ElemDefault("ShadowSize", "0").ParseInt();
            Defense = (ushort)obj.ElemDefault("Defense", "0").ParseInt();
            Flying = obj.HasElement("Flying");
            God = obj.HasElement("God");

            var projs = new List<ProjectileStructure>();
            obj.Elements((XName)"Projectile")
                 .ForEach<XElement>((Action<XElement>)(projectile => projs.Add(new ProjectileStructure(projectile))));
            Projectiles = projs.ToArray();

            Name = obj.AttrDefault("id", "");
        }

        /// <summary>
        ///     The numerical identifier for this object
        /// </summary>
        public ushort Id { get; }

        /// <summary>
        ///     The text identifier for this object
        /// </summary>
        public string Name { get; }

        internal static Dictionary<ushort, ObjectStructure> Load(XDocument doc)
        {
            var map = new Dictionary<ushort, ObjectStructure>();

            doc.Element("Objects")
                .Elements("Object")
                .ForEach(obj =>
                {
                    var o = new ObjectStructure(obj);
                    map[o.Id] = o;
                });

            return map;
        }

        public override string ToString()
        {
            return $"Object: {Name} (0x{Id:X})";
        }
    }
}