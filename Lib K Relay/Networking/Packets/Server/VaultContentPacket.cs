using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class VaultContentPacket : Packet
    {
        public bool lastVaultUpdate;
        public int vaultChestObjectId;
        public int materialChestObjectID;
        public int giftChestObjectId;
        public int potionStorageObjectId;
        public int seasonalSpoilChestObjectId;
        public int[] vaultContents;
        public int[] materialContents;
        public int[] giftContents;
        public int[] potionContents;
        public int[] seasonalSpoilContent;
        public short vaultUpgradeCost;
        public short materialUpgradeCost;
        public short potionUpgradeCost;
        public short currentPotionMax;
        public short nextPotionMax;
        public string vaultChestEnchants;
        public string giftChestEnchants;
        public string spoilsChestEnchants;

        public override PacketType Type => PacketType.VAULTCONTENT;

        public override void Read(PacketReader r)
        {
            lastVaultUpdate = r.ReadBoolean();
            vaultChestObjectId = r.ReadCompressedInt();
            materialChestObjectID = r.ReadCompressedInt();
            giftChestObjectId = r.ReadCompressedInt();
            potionStorageObjectId = r.ReadCompressedInt();
            seasonalSpoilChestObjectId = r.ReadCompressedInt();

            vaultContents = new int[r.ReadCompressedInt()];
            for (var i = 0; i < vaultContents.Length; i++)
                vaultContents[i] = r.ReadCompressedInt();

            materialContents = new int[r.ReadCompressedInt()];
            for (var i = 0; i < materialContents.Length; i++)
                materialContents[i] = r.ReadCompressedInt();

            giftContents = new int[r.ReadCompressedInt()];
            for (var i = 0; i < giftContents.Length; i++)
                giftContents[i] = r.ReadCompressedInt();

            potionContents = new int[r.ReadCompressedInt()];
            for (var i = 0; i < potionContents.Length; i++)
                potionContents[i] = r.ReadCompressedInt();

            seasonalSpoilContent = new int[r.ReadCompressedInt()];
            for (var i = 0; i < seasonalSpoilContent.Length; i++)
                seasonalSpoilContent[i] = r.ReadCompressedInt();

            vaultUpgradeCost = r.ReadInt16();
            materialUpgradeCost = r.ReadInt16();
            potionUpgradeCost = r.ReadInt16();
            currentPotionMax = r.ReadInt16();
            nextPotionMax = r.ReadInt16();

            vaultChestEnchants = r.ReadString();
            giftChestEnchants = r.ReadString();
            spoilsChestEnchants = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(lastVaultUpdate);
            w.WriteCompressedInt(vaultChestObjectId);
            w.WriteCompressedInt(materialChestObjectID);
            w.WriteCompressedInt(giftChestObjectId);
            w.WriteCompressedInt(potionStorageObjectId);
            w.WriteCompressedInt(seasonalSpoilChestObjectId);

            w.WriteCompressedInt(vaultContents.Length);
            foreach (var item in vaultContents)
                w.WriteCompressedInt(item);

            w.WriteCompressedInt(materialContents.Length);
            foreach (var item in materialContents)
                w.WriteCompressedInt(item);

            w.WriteCompressedInt(giftContents.Length);
            foreach (var item in giftContents)
                w.WriteCompressedInt(item);

            w.WriteCompressedInt(potionContents.Length);
            foreach (var item in potionContents)
                w.WriteCompressedInt(item);

            w.WriteCompressedInt(seasonalSpoilContent.Length);
            foreach (var item in seasonalSpoilContent)
                w.WriteCompressedInt(item);

            w.Write(vaultUpgradeCost);
            w.Write(materialUpgradeCost);
            w.Write(potionUpgradeCost);
            w.Write(currentPotionMax);
            w.Write(nextPotionMax);

            w.WriteUtf32(vaultChestEnchants);
            w.WriteUtf32(giftChestEnchants);
            w.WriteUtf32(spoilsChestEnchants);
        }
    }
}