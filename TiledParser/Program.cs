﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using TiledParser.Models;

namespace TiledParser
{
    public class Program
    {
        private const string Magic = "MMORL";
        private const int Version = 1;

        public static void Main(string[] args)
        {
            string input = "C:/Users/Matt/source/repos/MMORL/Tiled/overworld.json";
            string output = "C:/Users/Matt/source/repos/MMORL/MMORL.Server/Data/export.dat";

            if (args.Length >= 1)
            {
                input = args[0];
            }

            if (args.Length >= 2)
            {
                output = args[1];
            }

            Stopwatch watch = Stopwatch.StartNew();

            Map map = ParseMap(input);
            ExportMap(map, output);

            watch.Stop();
            Console.WriteLine($"Completed in: {watch.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        private static Map ParseMap(string path)
        {
            Console.WriteLine($"Reading file: {path}");
            string jsonString = File.ReadAllText(path);
            JObject root = JObject.Parse(jsonString);
            return root.ToObject<Map>();
        }

        private static void ExportMap(Map map, string output)
        {
            Console.WriteLine("Exporting map");

            using (FileStream stream = new FileStream(output, FileMode.Create))
            using (GZipStream gZip = new GZipStream(stream, CompressionLevel.Optimal))
            using (BinaryWriter writer = new BinaryWriter(gZip, Encoding.UTF8))
            {
                WriteFormat(writer);
                WriteTileDefinitions(map, writer);
                WriteWarps(map, writer);
                WriteChunkData(map, writer);
            }
        }

        private static void WriteFormat(BinaryWriter writer)
        {
            writer.Write(Magic);
            writer.Write(Version);
        }

        private static void WriteTileDefinitions(Map map, BinaryWriter writer)
        {
            Tileset terrainTileset = map.GetTilesetByName("terrain");

            List<Tile> tiles = terrainTileset.Tiles;
            List<Tile> tilesToRegister = new List<Tile>();

            foreach (Tile tile in tiles)
            {
                tilesToRegister.Add(tile);
            }

            Layer terrainLayer = map.GetLayerByName("tiles");
            ushort terrainFirstId = terrainTileset.FirstGid;

            foreach (Chunk chunk in terrainLayer.Chunks)
            {
                int chunkSize = chunk.Width;

                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        int index = x + y * chunkSize;

                        ushort tileId = (ushort)chunk.Data[index];
                        if (tileId != 0)
                        {
                            tileId -= terrainFirstId;
                        }

                        if (!tilesToRegister.Any(t => t.Id == tileId))
                        {
                            Tile tile = new Tile
                            {
                                Id = tileId,
                                Properties = new List<CustomProperty>
                                {
                                    new CustomProperty { Name = "isSolid", Value = "false" },
                                    new CustomProperty { Name = "isTransparent", Value = "true" }
                                }
                            };
                            tilesToRegister.Add(tile);
                        }
                    }
                }
            }

            writer.Write((ushort)tilesToRegister.Count);

            foreach (Tile tile in tilesToRegister)
            {
                writer.Write(tile.Id);

                bool isSolid = false;

                if (tile.HasProperty("isSolid"))
                {
                    isSolid = tile.GetProperty("isSolid");
                }

                bool isTransparent = !isSolid;

                if (tile.HasProperty("isTransparent"))
                {
                    isTransparent = tile.GetProperty("isTransparent");
                }

                writer.Write(isSolid);
                writer.Write(isTransparent);
            }
        }

        private static void WriteWarps(Map map, BinaryWriter writer)
        {
            Layer warpsLayer = map.GetLayerByName("warps");

            List<Warp> warps = new List<Warp>();

            foreach (LayerObject layerObject in warpsLayer.Objects)
            {

                if (layerObject.HasProperty("warp"))
                {
                    CustomProperty target = layerObject.GetProperty("warp");
                    LayerObject targetObject = warpsLayer.GetObjectById(int.Parse(target.Value));

                    Warp warp = new Warp
                    {
                        Id = layerObject.Id,
                        StartX = (short)layerObject.X,
                        StartY = (short)layerObject.Y,
                        EndX = (short)targetObject.X,
                        EndY = (short)targetObject.Y,
                    };
                    warps.Add(warp);
                }
            }

            writer.Write((ushort)warps.Count);
            foreach (Warp warp in warps)
            {
                writer.Write(warp.Id);
                writer.Write(warp.StartX);
                writer.Write(warp.StartY);
                writer.Write(warp.EndX);
                writer.Write(warp.EndY);
            }
        }

        private static void WriteChunkData(Map map, BinaryWriter writer)
        {
            Tileset terrainTileset = map.GetTilesetByName("terrain");
            Tileset colorTileset = map.GetTilesetByName("swatch");

            ushort terrainFirstId = terrainTileset.FirstGid;
            byte colorFirstId = (byte)colorTileset.FirstGid;

            Layer terrainLayer = map.GetLayerByName("tiles");
            Layer colorLayer = map.GetLayerByName("colors");

            writer.Write((ushort)terrainLayer.Chunks.Count);

            foreach (Chunk chunk in terrainLayer.Chunks)
            {
                if (chunk.Width != chunk.Height)
                {
                    throw new InvalidOperationException($"Chunk size is not square: {chunk.Width}x{chunk.Height}");
                }

                byte chunkSize = chunk.Width;

                short chunkX = (short)(chunk.X / chunkSize);
                short chunkY = (short)(chunk.Y / chunkSize);

                Chunk colorChunk = colorLayer.GetChunkAt(chunkX, chunkY);

                writer.Write(chunkX);
                writer.Write(chunkY);
                writer.Write(chunkSize);

                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        int index = x + y * chunkSize;

                        ushort tileId = (ushort)chunk.Data[index];
                        if (tileId != 0)
                        {
                            tileId -= terrainFirstId;
                        }

                        byte colorId = (byte)colorChunk.Data[index];

                        if (colorId != 0)
                        {
                            colorId -= colorFirstId;
                        }

                        writer.Write(tileId);
                        writer.Write(colorId);
                    }
                }
            }

            Console.WriteLine($"Saved {terrainLayer.Chunks.Count} chunk{(terrainLayer.Chunks.Count != 1 ? "s" : "")}");
        }

    }
}
