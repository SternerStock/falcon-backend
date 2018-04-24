namespace Falcon.API.Models
{
    using System;
    using System.IO;
    using System.Linq;
    using Falcon.API.Helpers;
    using fNbt;

    public enum MinecraftDimension
    {
        nether = -1,
        overworld = 0,
        end = 1
    }

    public class MinecraftPlayerModel
    {
        public MinecraftPlayerModel(NbtFile file, DateTime lastSeen)
        {
            this.Dimension = ((MinecraftDimension)file.RootTag["Dimension"].IntValue).ToString();
            this.X = file.RootTag["Pos"][0].FloatValue;
            this.Y = file.RootTag["Pos"][1].FloatValue;
            this.Z = file.RootTag["Pos"][2].FloatValue;
            this.XpLevel = file.RootTag["XpLevel"].IntValue;

            this.SelectedItem = "nothing";
            this.LastSeen = lastSeen;

            try
            {
                var inventory = (NbtList)file.RootTag["Inventory"];
                var holding = inventory[file.RootTag["SelectedItemSlot"].IntValue];
                this.SelectedItem = holding == null ? null : holding["id"].StringValue;
            }
            catch (Exception)
            {
            }

            string filename = Path.GetFileNameWithoutExtension(file.FileName);

            this.UUID = new Guid(filename);
            try
            {
                this.Name = MinecraftCache.GetPlayerNameByUUID(this.UUID);

                var onlinePlayers = MinecraftCache.GetOnlinePlayers();
                this.Online = onlinePlayers.Contains(this.Name);
            }
            catch (System.Net.WebException)
            {
            }
        }

        public string Dimension { get; set; }

        public string Name { get; set; }

        public bool Online { get; set; }

        public string SelectedItem { get; set; }

        public Guid UUID { get; set; }

        public float X { get; set; }

        public int XpLevel { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public DateTime LastSeen { get; set; }
    }
}