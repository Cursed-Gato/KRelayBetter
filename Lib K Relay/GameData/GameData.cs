using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lib_K_Relay.GameData.DataStructures;
using Lib_K_Relay.Properties;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.GameData
{
    /// <summary>
    ///     Represents a mapping of short identifiers to data structures for a given data type
    /// </summary>
    /// <typeparam name="TIdType">The type of the short identifier (e.g. byte, ushort, string)</typeparam>
    /// <typeparam name="TDataType">The type of the data structure (e.g. PacketStructure, EnemyStructure, ServerStructure)</typeparam>
    public class GameDataMap<TIdType, TDataType> where TDataType : IDataStructure<TIdType>
    {
        public GameDataMap(Dictionary<TIdType, TDataType> map)
        {
            Map = map;
        }

        /// <summary>
        ///     Map of short id -> data structure
        /// </summary>
        public Dictionary<TIdType, TDataType> Map { get; }

        /// <summary>
        ///     Selects a data structure from this map by short identifier
        /// </summary>
        /// <param name="id">The short identifier</param>
        /// <returns>The data structure</returns>
        /// <example>GameData.Packets.ByID(255) -> Packet: UNKNOWN (255)</example>
        /// <example>GameData.Servers.ByID("USW") -> Server: USWest/USW</example>
        public TDataType ById(TIdType id)
        {
            return Map[id];
        }

        /// <summary>
        ///     Selects a data structure from this map by full identifier (strings only)
        /// </summary>
        /// <param name="name">The string identifier</param>
        /// <returns>The data structure</returns>
        /// <example>GameData.Packets.ByName("UNKNOWN") -> Packet: UNKNOWN(255)</example>
        /// <example>GameData.Servers.ByName("USWest") -> Server: USWest/USW</example>
        public TDataType ByName(string name)
        {
            return Map.First(e => e.Value.Name == name).Value;
        }

        /// <summary>
        ///     Selects the first value from this map for which the given function returns true.
        /// </summary>
        /// <param name="f">The expression to evaluate</param>
        /// <returns>The data structure</returns>
        /// <example>GameData.Packets.Match(p => p.Type == typeof(NewTickPacket)) -> NEWTICK (47)</example>
        public TDataType Match(Func<TDataType, bool> f)
        {
            return Map.First(e => f(e.Value)).Value;
        }
    }

    public static class GameData
    {
        /// <summary>
        ///     Maps item data ("type" attribute -> item structure)
        /// </summary>
        public static GameDataMap<ushort, ItemStructure> Items;

        /// <summary>
        ///     Maps tile data ("type" attribute -> tile structure)
        /// </summary>
        public static GameDataMap<ushort, TileStructure> Tiles;

        /// <summary>
        ///     Maps object data ("type" attribute -> object structure)
        /// </summary>
        public static GameDataMap<ushort, ObjectStructure> Objects;

        /// <summary>
        ///     Maps packet data (PacketID -> packet structure)
        /// </summary>
        public static GameDataMap<byte, PacketStructure> Packets;

        /// <summary>
        ///     Maps server data (Abbreviation -> server structure) (e.g. USW -> USWest)
        /// </summary>
        public static GameDataMap<string, ServerStructure> Servers;

        static GameData()
        {
            RawObjectsXml = Resources.Objects;
            RawPacketsXml = Resources.Packets;
            RawTilesXml = Resources.Tiles;
            RawServersXml = Resources.Servers;
        }

        public static string RawObjectsXml { get; }
        public static string RawPacketsXml { get; }
        public static string RawTilesXml { get; }
        public static string RawServersXml { get; }

        public static void Load()
        {
            Parallel.Invoke(
                () =>
                {
                    Items = new GameDataMap<ushort, ItemStructure>(ItemStructure.Load(XDocument.Parse(RawObjectsXml)));
                    PluginUtils.Log("GameData", "Mapped {0} items.", Items.Map.Count);
                },
                () =>
                {
                    Tiles = new GameDataMap<ushort, TileStructure>(TileStructure.Load(XDocument.Parse(RawTilesXml)));
                    PluginUtils.Log("GameData", "Mapped {0} tiles.", Tiles.Map.Count);
                },
                () =>
                {
                    Objects = new GameDataMap<ushort, ObjectStructure>(
                        ObjectStructure.Load(XDocument.Parse(RawObjectsXml)));
                    PluginUtils.Log("GameData", "Mapped {0} objects.", Objects.Map.Count);
                },
                () =>
                {
                    Packets = new GameDataMap<byte, PacketStructure>(
                        PacketStructure.Load(XDocument.Parse(RawPacketsXml)));
                    PluginUtils.Log("GameData", "Mapped {0} packets.", Packets.Map.Count);
                },
                () =>
                {
                    Servers = new GameDataMap<string, ServerStructure>(
                        ServerStructure.Load(XDocument.Parse(RawServersXml)));
                    PluginUtils.Log("GameData", "Mapped {0} servers.", Servers.Map.Count);
                });

            PluginUtils.Log("GameData", "Successfully loaded game data.");
        }
    }
}