using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

//Card card = new("The Restaurant")
//{
//    new Category("Appetizers")
//    {
//        new Item("Dungeon Crab Cocktail", "Classic cocktail sauce", 27M),
//        new Item("Almond Crusted Scallops", "Almonds, Parmesan, chive beurre blanc", 19M)
//    },
//    new Category("Dinner")
//    {
//        new Item("Grilled King Salmon", "Lemon chive beurre blanc", 49M)
//    }
//};

Category appetizers = new("Appetizers");
appetizers.Items.Add(new Item("Dungeon Crab Cocktail", "Classic cocktail sauce", 27M));
appetizers.Items.Add(new Item("Almond Crusted Scallops", "Almonds, Parmesan, chive beurre blanc", 19M));

Category dinner = new("Dinner");
dinner.Items.Add(new Item("Grilled King Salmon", "Lemon chive beurre blanc", 49M));

Card card = new("The Restaurant");
card.Categories.Add(appetizers);
card.Categories.Add(dinner);

string json = SerializeJson(card);
// string json = File.ReadAllText("thecard.json");
DeserializeJson(json);


string SerializeJson(Card card)
{
    Console.WriteLine(nameof(SerializeJson));

    //JsonSerializerOptions options = new()
    //{
    //    WriteIndented = true,
    //    //Converters =
    //    //{
    //    //    new System.Text.Json.Serialization.JsonConverter<object>
    //    //}
    //};

    string json = JsonSerializer.Serialize(card);
    Console.WriteLine(json);
    Console.WriteLine();
    return json;
}

static void DeserializeJson(string json)
{
    Console.WriteLine(nameof(DeserializeJson));

    Card? card = JsonSerializer.Deserialize<Card>(json);
    if (card is null)
    {
        Console.WriteLine("no card deserialized");
        return;
    }
    Console.WriteLine($"{card.Title}");
    foreach (var category in card.Categories)
    {
        Console.WriteLine($"\t{category.Title}");
        foreach (var item in category.Items)
        {
            Console.WriteLine($"\t\t{item.Title}");
        }
    }
    Console.WriteLine();
}

// NotSupportedException with IEnumerable<T> 
/*
 *  System.NotSupportedException: The collection type 'Card' is abstract, an interface, or is read only, and could not be instantiated and populated. Path: $ | LineNumber: 0 | BytePositionInLine: 1.
 ---> System.NotSupportedException: The collection type 'Card' is abstract, an interface, or is read only, and could not be instantiated and populated.
   --- End of inner exception stack trace ---
   at System.Text.Json.ThrowHelper.ThrowNotSupportedException(ReadStack& state, Utf8JsonReader& reader, NotSupportedException ex)
   at System.Text.Json.ThrowHelper.ThrowNotSupportedException_CannotPopulateCollection(Type type, Utf8JsonReader& reader, ReadStack& state)
   at System.Text.Json.Serialization.Converters.IEnumerableOfTConverter`2.CreateCollection(Utf8JsonReader& reader, ReadStack& state, JsonSerializerOptions options)
   at System.Text.Json.Serialization.Converters.IEnumerableDefaultConverter`2.OnTryRead(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options, ReadStack& state, TCollection& value)
   at System.Text.Json.Serialization.JsonConverter`1.TryRead(Utf8JsonReader& reader, Type typeToConvert, JsonSerializerOptions options, ReadStack& state, T& value)
   at System.Text.Json.Serialization.JsonConverter`1.ReadCore(Utf8JsonReader& reader, JsonSerializerOptions options, ReadStack& state)
   at System.Text.Json.JsonSerializer.ReadCore[TValue](JsonConverter jsonConverter, Utf8JsonReader& reader, JsonSerializerOptions options, ReadStack& state)
   at System.Text.Json.JsonSerializer.ReadCore[TValue](Utf8JsonReader& reader, Type returnType, JsonSerializerOptions options)
   at System.Text.Json.JsonSerializer.Deserialize[TValue](String json, Type returnType, JsonSerializerOptions options)
   at System.Text.Json.JsonSerializer.Deserialize[TValue](String json, JsonSerializerOptions options)
   at <Program>$.<<Main>$>g__DeserializeJson|0_1(String json) in C:\github\jsonserializerwithienumerable\JsonSerializerIssue\JsonSerializerIssue\Program.cs:line 48
   at <Program>$.<Main>$(String[] args) in C:\github\jsonserializerwithienumerable\JsonSerializerIssue\JsonSerializerIssue\Program.cs:line 31
 * */
public record Item(string Title, string Text, decimal Price);
public record Category(string Title) : IList<Item>
{
    public Category() : this(string.Empty)
    {
    }
    public IList<Item> Items { get; init; } = new List<Item>();

    public int Count => Items.Count;

    public bool IsReadOnly => false;

    public Item this[int index] { get => Items[index]; set => Items[index] = value; }

    public IEnumerator<Item> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Items).GetEnumerator();

    public void Add(Item item) => Items.Add(item);

    public int IndexOf(Item item) => Items.IndexOf(item);

    public void Insert(int index, Item item) => Items.Insert(index, item);

    public void RemoveAt(int index) => Items.RemoveAt(index);

    public void Clear() => Items.Clear();

    public bool Contains(Item item) => Items.Contains(item);

    public void CopyTo(Item[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

    public bool Remove(Item item) => Items.Remove(item);
}
public record Card(string Title) : IList<Category>
{
    public Card() : this(string.Empty) { }

    public IList<Category> Categories { get; init; } = new List<Category>();

    public int Count => Categories.Count;

    public bool IsReadOnly => false;

    public Category this[int index] { get => Categories[index]; set => Categories[index] = value; }

    public IEnumerator<Category> GetEnumerator() => Categories.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Categories).GetEnumerator();

    public void Add(Category item) => Categories.Add(item);

    public int IndexOf(Category item) => Categories.IndexOf(item);

    public void Insert(int index, Category item) => Categories.Insert(index, item);

    public void RemoveAt(int index) => Categories.RemoveAt(index);

    public void Clear() => Categories.Clear();

    public bool Contains(Category item) => Categories.Contains(item);

    public void CopyTo(Category[] array, int arrayIndex) => Categories.CopyTo(array, arrayIndex);

    public bool Remove(Category item) => Categories.Remove(item);

}

//// this deserializes successfully:
//public record Item(string Title, string Text, decimal Price);
//public record Category(string Title)
//{
//    public IList<Item> Items { get; init; } = new List<Item>(); // init required for deserialization

//    public IEnumerator<Item> GetEnumerator() => Items.GetEnumerator();

//    // IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Items).GetEnumerator();

//    public void Add(Item item) => Items.Add(item);
//}
//public record Card(string Title)
//{
//    public IList<Category> Categories { get; init; } = new List<Category>();

//    public IEnumerator<Category> GetEnumerator() => Categories.GetEnumerator();

//    // IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Categories).GetEnumerator();

//    public void Add(Category category) => Categories.Add(category);
//}

