namespace Falcon.MtG
{
    public interface ISimpleLookup
    {
        int ID { get; set; }
        string Name { get; set; }
    }

    public partial class CardType : ISimpleLookup { };

    public partial class Supertype : ISimpleLookup { };

    public partial class Subtype : ISimpleLookup { };

    public partial class Layout : ISimpleLookup { };

    public partial class Rarity : ISimpleLookup { };

    public partial class Border : ISimpleLookup { };

    public partial class Frame : ISimpleLookup { };

    public partial class Watermark : ISimpleLookup { };

    public partial class Artist : ISimpleLookup { };

    public partial class SetType : ISimpleLookup { };

    public partial class Block : ISimpleLookup { };
}